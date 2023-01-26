using Fitness.Data.Models.Base;

namespace Fitness.Data.Models
{
    public class Role : BaseDeletableModel
    {
        public Role()
        {
            this.Users = new HashSet<User>();
        }

        public string? Name { get; set; }

        public ICollection<User> Users { get; set; }
    }
}
