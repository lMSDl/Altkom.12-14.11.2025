using System.ComponentModel.DataAnnotations;

namespace Models.Settings
{
    public class Bogus
    {
        [Required]
        public int NumberOfResources { get; set; }
        [Range(1, 75)]
        public int NumberOfNestedResources { get; set; }

        public string Language { get; set; }
    }
}
