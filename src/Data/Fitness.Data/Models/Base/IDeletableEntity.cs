namespace Fitness.Data.Models.Base
{
    public interface IDeletableEntity
    {
        string Id { get; set; }

        DateTime CreatedOn { get; set; }

        DateTime? ModifiedOn { get; set; }

        bool IsDeleted { get; set; }

        DateTime? DeletedOn { get; set; }
    }
}
