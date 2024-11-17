using FederationTask.Models.Base;

namespace FederationTask.Models
{
    public class team:baseAuditable
    {
        public string Name { get; set; } = null!;
        public string PhoneNumber { get; set; } = null!;
        public string Email { get; set; } = null!;
        public int ClubId { get; set; }
        public club? Club { get; set;}
    }
}
