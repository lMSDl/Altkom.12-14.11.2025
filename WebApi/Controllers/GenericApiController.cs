using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Services.Interfaces;

namespace WebApi.Controllers
{
    public class GenericApiController<T> : ApiController
    {
        private readonly IGenericService<T> _service;
        protected readonly IValidator<T>? _validator;

        public GenericApiController(IGenericService<T> service, IValidator<T>? validator = null)
        {
            _validator = validator;
            _service = service;
        }

        [HttpGet("{id:int}")]
        public virtual async Task<ActionResult<T>> GetById(int id)
        {
            var entity = await _service.ReadByIdAsync(id);

            if (entity is null)
                //metoda pomocnicza NotFound() - zwraca kod statusu 404
                return NotFound();

            return Ok(entity);
        }

        [HttpPut("{id:int}")]
        public virtual async Task<ActionResult> Put(int id, T entity)
        {
            var localEntity = await _service.ReadByIdAsync(id);
            if (localEntity is null)
            {
                return NotFound();
            }

            await _service.UpdateAsync(id, entity);

            return NoContent(); //204 No Content
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Delete(int id)
        {
            var entity = await _service.ReadByIdAsync(id);
            if (entity is null)
                return NotFound();

            await _service.DeleteAsync(id);

            return NoContent(); //204 No Content
        }
    }
}
