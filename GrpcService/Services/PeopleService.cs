using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Services.Interfaces;

namespace GrpcService.Services
{
    public class PeopleService : GrpcService.Services.PeopleGrpcService.PeopleGrpcServiceBase
    {


        private readonly IPeopleService _service;
        public PeopleService(IPeopleService service)
        {
            _service = service;
        }


        public override async Task<People> Read(Empty request, ServerCallContext context)
        {
            var people = await _service.ReadAsync();

            var result = new People();
            foreach (var person in people)
            {
                result.Collection.Add(new Person
                {
                    Id = person.Id,
                    FirstName = person.FirstName,
                    LastName = person.LastName,
                    Age = person.Age
                });
            }
            return result;
        }

        public override async Task<Person> ReadById(Id request, ServerCallContext context)
        {
            var person = await _service.ReadByIdAsync(request.Value);
            if (person is null)
                throw new RpcException(new Status(StatusCode.NotFound, $"Person with Id={request.Value} not found"));

            return new Person
            {
                Id = person.Id,
                FirstName = person.FirstName,
                LastName = person.LastName,
                Age = person.Age
            };
        }

        public override async Task<OptionalPerson> ReadByIdOptional(Id request, ServerCallContext context)
        {
            var person = await _service.ReadByIdAsync(request.Value);
            if (person is null)
                return new OptionalPerson() { Void = new Void() };

            return new OptionalPerson()
            {
                Person = new Person
                {
                    Id = person.Id,
                    FirstName = person.FirstName,
                    LastName = person.LastName,
                    Age = person.Age
                }
            };
        }

        public override async Task<Id> Create(Person request, ServerCallContext context)
        {
            var person = new Models.Person { FirstName = request.FirstName, LastName = request.LastName, Age = request.Age };
            var id = await _service.CreateAsync(person);
            return new Id { Value = id };
        }


        public override async Task<Void> Update(ComplexPerson request, ServerCallContext context)
        {
            if (await _service.ReadByIdAsync(request.Id.Value) is null)
            {
                throw new RpcException(new Status(StatusCode.NotFound, $"Person with Id={request.Id.Value} not found"));
            }

            var person = new Models.Person
            {
                FirstName = request.Person.FirstName,
                LastName = request.Person.LastName,
                Age = request.Person.Age
            };
            await _service.UpdateAsync(request.Id.Value, person);
            return new Void();
        }

        public override async Task<Void> Delete(Id request, ServerCallContext context)
        {
            if (await _service.ReadByIdAsync(request.Value) is null)
            {
                throw new RpcException(new Status(StatusCode.NotFound, $"Person with Id={request.Value} not found"));
            }
            await _service.DeleteAsync(request.Value);
            return new Void();
        }
    }
}
