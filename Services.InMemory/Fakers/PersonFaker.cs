using Models;

namespace Services.InMemory.Fakers
{
    public class PersonFaker : EntityFaker<Person>
    {
        public PersonFaker(Models.Settings.Bogus settings) : base(settings.Language)
        {
            RuleFor(x => x.FirstName, f => f.Name.FirstName());
            RuleFor(x => x.LastName, f => f.Name.LastName());
            RuleFor(x => x.Age, f => (DateTime.Now.Year - f.Person.DateOfBirth.Year));
        }
    }
}
