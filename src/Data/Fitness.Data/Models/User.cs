using Fitness.Data.Models.Base;

namespace Fitness.Data.Models
{
    public class User : BaseDeletableModel
    {
        public User()
        {
            this.Roles = new HashSet<Role>();
        }

        public string? Username { get; set; }

        public string? FirstName { get; set; }

        public string? LastName { get; set; }

        public string? Email { get; set; }

        public bool EmailConfirmed { get; set; } = false;

        public string? Password { get; set; }

        public string? PhoneNumber { get; set; }

        public bool PhoneNumberConfirmed { get; set; } = false;

        public ICollection<Role> Roles { get; set; }
    }
}
