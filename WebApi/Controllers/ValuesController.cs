using Microsoft.AspNetCore.Mvc;
using System.Runtime.CompilerServices;

namespace WebApi.Controllers
{
    
    public class ValuesController : LegacyApiController
    {
        private readonly IList<int> _list;

        public ValuesController(IList<int> list, ILogger<ValuesController> logger)
        {
            _list = list;
        }

        private static int _counter = 0;

        [HttpGet]
        public async Task<IEnumerable<int>> Get(CancellationToken cancellationToken)
        {
            Interlocked.Increment(ref _counter);
            Console.WriteLine($"Counter: {_counter}");
            try
            {
                /*if (cancellationToken.IsCancellationRequested)
                    return [];*/

                await Task.Delay(5000, cancellationToken); //symulacja długotrwałej operacji

                return _list;
            }
            catch
            {
                Console.WriteLine("Request cancelled");
                throw;
            }
            finally
            {
                Interlocked.Decrement(ref _counter);
            }
        }

        [HttpDelete] // domyślny route to api/values - parametr w query requestu
        [HttpDelete("{value:int}")] // {value:int} - parametr w path z określonym typem
        [HttpDelete("del/{value:int}")] //jeśli route metody nie zaczyna się od / to jest doklejany do route kontrolera
        [HttpDelete("/wartosci/{value:int}")] //jeśli route metody zaczyna się od / to jest traktowany jako pełny nowy adres
        public void Delete(int value)
        {
            _list.Remove(value);
        }

        [HttpPost("{value:int:max(50)}")] //metody "walidacyjne" określone w route (np. max(50)) działają jak filtry przed wykonaniem metody (jeśli coś nie przejdzie to dostajemy 404)
        public void Post(int value)
        {
            _list.Add(value);
        }

        [HttpPut("{oldValue:int}")]
        public void Put(int oldValue, [FromQuery] int newValue) // [FromQuery] - parametr z query requestu
        {
            _list[_list.IndexOf(oldValue)] = newValue;
        }

    }
}
