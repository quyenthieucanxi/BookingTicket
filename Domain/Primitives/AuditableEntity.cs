namespace Domain.Primitives;

public abstract class AuditableEntity
{
    public DateTime CreatedOn { get; set; }

    public string CreatedBy { get; set; } = "SQL";

    public DateTime ModifiedOn { get; set; }

    public string ModifiedBy { get; set; } = "SQL";
}