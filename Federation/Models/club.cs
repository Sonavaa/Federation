using FederationTask.Models.Base;

namespace FederationTask.Models
{
    public class club:baseAuditable
    {
        public string NameOfClubCreator { get; set; } = null!;
        public string PhoneNumberOfClubCreator { get; set; } = null!;
        public string ClubCreatorsEmail { get; set; } = null!;
        public string Director { get; set; } = null!;
        public string DirectorsEmail { get; set; } = null!;
        public string ClubName { get; set; } = null!;
        public string Vöen { get; set; } = null!;
        public string? HallName { get; set; }
        public List<team>? Teams { get; set; } = new List<team>();

    }
}
