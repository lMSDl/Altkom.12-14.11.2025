using Microsoft.Extensions.Options;
using Models;
using Services.Interfaces;

namespace Services.InMemory
{
    public class PeopleService : GenericService<Person>, IPeopleService
    {

        public PeopleService(Bogus.Faker<Person> faker, IOptions<Models.Settings.Bogus> options) : base(faker, options)
        {
            // Losowe powiązanie Parent -> Children
            if (_entities.Count > 1)
            {
                var random = new Random();
                for (int i = 1; i < _entities.Count - 5; i++)
                {
                    // Wybierz losowego rodzica spośród wcześniejszych osób
                    int parentIndex = random.Next(0, i);
                    var child = _entities[i];
                    var parent = _entities[parentIndex];
                    child.Parent = parent;
                    parent.Children.Add(child);
                }
            }
        }


        public Task<IEnumerable<Person>> ReadByName(string name)
        {
            var people = _entities.Where(p => p.FullName.Contains(name, StringComparison.OrdinalIgnoreCase));
            return Task.FromResult(people.AsEnumerable());
        }
    }
}
