using Models;

namespace Services.InMemory.Fakers
{
    public class PersonFaker : EntityFaker<Person>
    {
        public PersonFaker()
        {
            RuleFor(x => x.FirstName, f => f.Name.FirstName());
            RuleFor(x => x.LastName, f => f.Name.LastName());
        }
    }
}
