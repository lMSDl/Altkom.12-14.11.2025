using Bogus;
using Models;

namespace Services.InMemory.Fakers
{
    public abstract class EntityFaker<T> : Faker<T> where T : Entity
    {
        public EntityFaker(string language) : base(language)
        {
            RuleFor(x => x.Id, x => x.IndexFaker + 1);
        }
    }
}
