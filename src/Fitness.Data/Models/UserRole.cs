using Fitness.Data.Models.Base;

namespace Fitness.Data.Models
{
    public class UserRole : BaseDeletableModel
    {
        public string UserId { get; set; }

        public User User { get; set; }

        public string RoleId { get; set; }

        public Role Role { get; set; }

        public string? HotelId { get; set; }

        public string? RestaurantId { get; set; }
    }
}
