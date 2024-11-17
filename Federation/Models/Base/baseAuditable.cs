namespace FederationTask.Models.Base
{
    public class baseAuditable:baseEntity
    {
        public string CreatedBy { get; set; } = null!;
        public DateTime CreatedAt { get; set; } = default!;
        public string? UpdatedBy { get; set; }
        public DateTime? UpdatedAt { get; set; }

    }
}
