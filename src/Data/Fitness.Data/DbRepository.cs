namespace Fitness.Data
{
    using System;
    using System.Linq;
    using Fitness.Data.Models.Base;
    using Microsoft.EntityFrameworkCore;

    public sealed class DbRepository<TEntity>
        where TEntity : class, IDeletableEntity
    {
        public DbRepository(ApplicationDbContext context)
        {
            this.Context = context ?? throw new ArgumentNullException(nameof(context));
            this.DbSet = this.Context.Set<TEntity>();
        }

        protected DbSet<TEntity> DbSet { get; set; }

        protected ApplicationDbContext Context { get; set; }

        public Task AddAsync(TEntity entity) => this.DbSet.AddAsync(entity).AsTask();

        public IQueryable<TEntity> All() => this.DbSet.Where(x => !x.IsDeleted);

        public IQueryable<TEntity> AllAsNoTracking() => this.DbSet.AsNoTracking().Where(x => !x.IsDeleted);

        public IQueryable<TEntity> AllWithDeleted() => this.DbSet;

        public IQueryable<TEntity> AllAsNoTrackingWithDeleted() => this.DbSet.AsNoTracking();

        public void Undelete(TEntity entity)
        {
            entity.IsDeleted = false;
            entity.DeletedOn = null;
            this.Update(entity);
        }

        public void Delete(TEntity entity)
        {
            entity.IsDeleted = true;
            entity.DeletedOn = DateTime.UtcNow;
            this.Update(entity);
        }

        public void Update(TEntity entity)
        {
            var entry = this.Context.Entry(entity);
            if (entry.State == EntityState.Detached)
            {
                this.DbSet.Attach(entity);
            }

            entry.State = EntityState.Modified;
        }

        public Task<int> SaveChangesAsync() => this.Context.SaveChangesAsync();
    }
}
