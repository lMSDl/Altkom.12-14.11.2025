using Models;
using Services.Interfaces;

namespace Services.InMemory
{
    public class PeopleService : GenericService<Person>, IPeopleService
    {

        public PeopleService(Bogus.Faker<Person> faker) : base(faker)
        {
        }


        public Task<IEnumerable<Person>> ReadByName(string name)
        {
            var people = _entities.Where(p => p.FirstName.Contains(name, StringComparison.OrdinalIgnoreCase) ||
                                   p.LastName.Contains(name, StringComparison.OrdinalIgnoreCase));
            return Task.FromResult(people.AsEnumerable());
        }
    }
}
