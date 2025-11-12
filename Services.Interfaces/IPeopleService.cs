using Models;

namespace Services.Interfaces
{
    public interface IPeopleService : IGenericService<Person>
    {
        Task<IEnumerable<Person>> ReadByName(string name);
    }
}
