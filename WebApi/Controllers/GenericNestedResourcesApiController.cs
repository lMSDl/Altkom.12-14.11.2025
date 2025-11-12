using Microsoft.AspNetCore.Mvc;
using Services.Interfaces;

namespace WebApi.Controllers
{
    public abstract class GenericNestedResourcesApiController<T, TParent> : ApiController
    {
        private IGenericService<T> _service;
        private IGenericService<TParent> _parentService;

        public GenericNestedResourcesApiController(IGenericService<T> service, IGenericService<TParent> parentService)
        {
            _service = service;
            _parentService = parentService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<T>>> GetForParent(int parentId)
        {
            if (await _parentService.ReadByIdAsync(parentId) is null)
                return NotFound();

            var result = await _service.ReadAsync(x => GetParentId(x) == parentId);

            return Ok(result);
        }

        protected abstract int GetParentId(T item);
        protected abstract int SetParentId(T item, int id);

        [HttpPost]
        public async Task<ActionResult<int>> Post(int parentId, [FromBody] T item)
        {
            if (await _parentService.ReadByIdAsync(parentId) is null)
                return NotFound();

            SetParentId(item, parentId);

            int id = await _service.CreateAsync(item);
            return CreatedAtAction(id);
        }

        protected abstract ActionResult<int> CreatedAtAction(int id);
    }
}
