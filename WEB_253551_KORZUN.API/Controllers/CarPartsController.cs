using Microsoft.AspNetCore.Mvc;
using WEB_253551_KORZUN.API.Services;
using WEB_253551_KORZUN.Domain.Entities;
using WEB_253551_KORZUN.Domain.Models;

namespace WEB_253551_KORZUN.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CarPartsController : ControllerBase
    {
        private readonly IProductService _productService;

        public CarPartsController(IProductService productService)
        {
            _productService = productService;
        }

        // GET: api/carparts
        // GET: api/carparts?pageNo=2
        [HttpGet]
        public async Task<ActionResult<ResponseData<ListModel<CarPart>>>> GetAllCarParts(
            [FromQuery] int pageNo = 1,
            [FromQuery] int pageSize = 3)
        {
            var response = await _productService.GetProductListAsync(
                null,  // Без категории = все запчасти
                pageNo,
                pageSize);

            if (!response.Successfull)
                return BadRequest(response.ErrorMessage);

            return Ok(response);
        }

        // GET: api/carparts/category/brakes
        // GET: api/carparts/category/brakes?pageNo=2
        [HttpGet("category/{category}")]
        public async Task<ActionResult<ResponseData<ListModel<CarPart>>>> GetCarPartsByCategory(
            string category,
            [FromQuery] int pageNo = 1,
            [FromQuery] int pageSize = 3)
        {
            var response = await _productService.GetProductListAsync(
                category,
                pageNo,
                pageSize);

            if (!response.Successfull)
                return BadRequest(response.ErrorMessage);

            return Ok(response);
        }

        // GET: api/carparts/5
        [HttpGet("{id:int}")]
        public async Task<ActionResult<ResponseData<CarPart>>> GetCarPart(int id)
        {
            var response = await _productService.GetProductByIdAsync(id);

            if (!response.Successfull)
                return NotFound(response.ErrorMessage);

            return Ok(response);
        }

        // PUT: api/carparts/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCarPart(int id, CarPart carPart)
        {
            if (id != carPart.Id)
                return BadRequest();

            try
            {
                await _productService.UpdateProductAsync(id, carPart);
            }
            catch
            {
                return BadRequest();
            }

            return NoContent();
        }

        // POST: api/carparts
        [HttpPost]
        public async Task<ActionResult<ResponseData<CarPart>>> PostCarPart(CarPart carPart)
        {
            var response = await _productService.CreateProductAsync(carPart);

            if (!response.Successfull)
                return BadRequest(response.ErrorMessage);

            return CreatedAtAction("GetCarPart", new { id = response.Data?.Id }, response);
        }

        // DELETE: api/carparts/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCarPart(int id)
        {
            try
            {
                await _productService.DeleteProductAsync(id);
            }
            catch
            {
                return BadRequest();
            }

            return NoContent();
        }
    }
}

