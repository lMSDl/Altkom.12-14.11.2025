using System.Text.Json.Serialization;

namespace Models
{
    public class Person : Entity
    {
        public Person? Parent { get; set; }
        public ICollection<Person> Children { get; set; } = [];

        public string FirstName { get; set; }
        public string LastName { get; set; }

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
