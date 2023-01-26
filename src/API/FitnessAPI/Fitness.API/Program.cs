using CryptographyServices.Contracts;
using CryptographyServices.Implementations;
using Fitness.Data;
using Fitness.InputModels;
using Fitness.ResponseModels;
using IdentityServices.Contracts;
using IdentityServices.Implementations;
using IdentityServices.Models;
using MappingServices;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Reflection;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers().AddXmlSerializerFormatters();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(x =>
{
    x.CustomSchemaIds(x => x.FullName);
    x.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
    {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "JWT Authorization header using the Bearer scheme. \r\n\r\n Enter 'Bearer' [space] and then your token in the text input below.\r\n\r\nExample: \"Bearer 1safsfsdfdfd\"",
    });
    x.AddSecurityRequirement(new OpenApiSecurityRequirement {
        {
            new OpenApiSecurityScheme {
                Reference = new OpenApiReference {
                    Type = ReferenceType.SecurityScheme,
                        Id = "Bearer"
                }
            },
            new string[] { }
        }
    });
});

#region Mapper Registration
var mapper = AutoMapperConfig.RegisterMappings(
                typeof(InputModelsMappingRegistration).GetTypeInfo().Assembly,
                typeof(ResponseModelsMappingRegistration).GetTypeInfo().Assembly,
                typeof(IdentityServicesModelsMappingRegistration).GetTypeInfo().Assembly);

builder.Services.AddSingleton(mapper);
#endregion

#region Db Registration
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DbConnection")));

builder.Services.AddScoped(typeof(DbRepository<>));
#endregion

#region Services Registration
// Common Services
builder.Services.AddTransient<IAuthenticationsService, AuthenticationsService>();
builder.Services.AddTransient<IUsersService, UsersService>();
builder.Services.AddTransient<IRolesService, RolesService>();
builder.Services.AddTransient<IEncryptService, EncryptService>();
// Project Services

#endregion 

var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JwtTokenValidation:Secret"]));
builder.Services
    .AddAuthentication(opts =>
    {
        opts.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        opts.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        opts.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(opts =>
    {
        opts.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = signingKey,
            ValidateIssuer = true,
            ValidIssuer = builder.Configuration["JwtTokenValidation:Issuer"],
            ValidateAudience = true,
            ValidAudience = builder.Configuration["JwtTokenValidation:Audience"],
            ValidateLifetime = true,
            ClockSkew = TimeSpan.Zero
        };
    });
builder.Services.AddAuthorization();

var app = builder.Build();

using (var serviceScope = app.Services.CreateScope())
{
    var identityDbContext = serviceScope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    identityDbContext.Database.Migrate();
}

// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
    app.UseSwagger();
    app.UseSwaggerUI(x =>
    {
        x.DocExpansion(Swashbuckle.AspNetCore.SwaggerUI.DocExpansion.None);
        x.DefaultModelsExpandDepth(-1);
    });
//}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
