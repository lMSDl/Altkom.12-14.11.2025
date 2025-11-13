using Models.Annotations;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Models
{
    public class Person : Entity
    {
        public Person? Parent { get; set; }
        public ICollection<Person> Children { get; set; } = [];
        [Required]
        public string FirstName { get; set; }
        [StringLength(15)]
        public string LastName { get; set; }

        [Range(18, 65, ErrorMessage = "Age must me between 16-65")]
        public int Age { get; set; }

        [MyValidationAnnotation(Value = "!")]
        [MyValidationAnnotation(Value = "g")]
        [MyValidationAnnotation(Value = "ala")]
        [MyValidationAnnotation(Value = "0")]
        public string Secret { get; set; }

        //[JsonIgnore]
        public string FullName => $"{FirstName} {LastName}";

        public string? NullString { get; set; } = null;
        public string EmptyString { get; set; } = string.Empty;
        public int DefaultInt { get; set; }
        public DateTime DefaultDateTime { get; set; }

        public bool ShouldSerializeFullName()
        {
            return FullName.Contains('z');
        }


    }
}
