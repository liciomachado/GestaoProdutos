using GestaoProdutos.Application.Dtos;
using GestaoProdutos.Application.Interfaces;
using GestaoProdutos.Application.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace GestaoProdutos.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ProductResponseViewModel>> GetById(long id)
        {
            var output = await _productService.GetById(id);
            if (output is null) return NotFound();

            return Ok(output);
        }

        [HttpGet]
        public async Task<ActionResult<ProductResponseViewModel>> GetWithFilters([FromQuery] string? description,
            [FromQuery] DateTime? startDateCreated, [FromQuery] DateTime? finishDateCreated,
            [FromQuery] DateTime? startDateValid, [FromQuery] DateTime? finishDateValid, int size = 10, int page = 1)
        {
            var filter = new FilterProductDTO(description, startDateCreated, finishDateCreated, startDateValid, finishDateValid, size, page);
            var output = await _productService.GetByFilters(filter);
            if (output is null) return NotFound();

            return Ok(output);
        }

        [HttpPost]
        public async Task<ActionResult<ProductResponseViewModel>> Create(ProductRequestViewModel productDTO)
        {
            var output = await _productService.SaveNewProduct(productDTO);
            return Created($"/{output.Id}", output);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> InactiveProduct(long id)
        {
            await _productService.InactiveProduct(id);
            return NoContent();
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<ProductResponseViewModel>> Create(long id, ProductRequestUpdateViewModel productDTO)
        {
            var output = await _productService.UpdateProduct(id, productDTO);
            return Created($"/{output.Id}", output);
        }
    }
}
