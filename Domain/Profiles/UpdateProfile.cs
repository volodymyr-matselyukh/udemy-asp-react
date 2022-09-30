using System.ComponentModel.DataAnnotations;

namespace Domain.Profiles
{
    public class UpdateProfile
    {
        [Required]
        public string DisplayName { get; set; }
        public string Bio { get; set; }
    }
}
