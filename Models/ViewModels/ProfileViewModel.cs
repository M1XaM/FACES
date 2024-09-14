namespace FACES.Models
{
    public class ProfileViewModel
    {
        public User User { get; set; }
        public IEnumerable<Project> Projects { get; set; }
    }
}
