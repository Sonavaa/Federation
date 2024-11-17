namespace FederationTask.Models.Base
{
    public class baseEntity
    {
        public int Id { get; set; }
        public bool isDeleted {  get; set; }
        public string TeamCategory { get; set; } = null!;
        public string Gender { get; set; } = null!;
        public string? Region { get; set; }
    }
}
