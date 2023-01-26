using System.ComponentModel.DataAnnotations;

namespace Fitness.Data.Models.Base
{
    public abstract class BaseDeletableModel : IDeletableEntity
    {
        protected BaseDeletableModel()
        {
            Id = Guid.NewGuid().ToString();
        }

        [Key]
        public string Id { get; set; }

        public DateTime CreatedOn { get; set; }

        public DateTime? ModifiedOn { get; set; }

        public bool IsDeleted { get; set; }

        public DateTime? DeletedOn { get; set; }
    }
}
