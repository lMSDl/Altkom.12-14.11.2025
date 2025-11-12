using Models;
using Services.Interfaces;

namespace WebApi.Controllers
{
    public class ProductsController : GenericApiController<Product>
    {
        public ProductsController(IGenericService<Product> service) : base(service)
        {
        }
    }
}
