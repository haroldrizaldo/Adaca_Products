using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Adaca_ProductWebAPI.Data;
using Adaca_ProductWebAPI.Models;
using Microsoft.Extensions.Logging; 
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Adaca_ProductWebAPI.Controllers
{
    [Route("api/products")]
    [ApiController]
    public class ProductsApiController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly ILogger<ProductsApiController> _logger; 

        public ProductsApiController(AppDbContext context, ILogger<ProductsApiController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // GET: api/products
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Product>>> GetProducts()
        {
            try
            {
                return await _context.Products.ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in GetProducts action: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        // GET: api/products/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> GetProduct(int id)
        {
            try
            {
                var product = await _context.Products.FindAsync(id);

                if (product == null)
                {
                    return NotFound();
                }

                return product;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in GetProduct action: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        // POST: api/products
        [HttpPost]
        public async Task<ActionResult<Product>> CreateProduct(Product product)
        {
            try
            {
                _context.Products.Add(product);
                await _context.SaveChangesAsync();

                return CreatedAtAction(nameof(GetProduct), new { id = product.Id }, product);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in CreateProduct action: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        // PUT: api/products/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProduct(int id, Product updatedProduct)
        {
            try
            {
                if (id != updatedProduct.Id)
                {
                    return BadRequest();
                }

                _context.Entry(updatedProduct).State = EntityState.Modified;

                await _context.SaveChangesAsync();

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in UpdateProduct action: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        // DELETE: api/products/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            try
            {
                var product = await _context.Products.FindAsync(id);

                if (product == null)
                {
                    return NotFound();
                }

                _context.Products.Remove(product);
                await _context.SaveChangesAsync();

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in DeleteProduct action: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        private bool ProductExists(int id)
        {
            return _context.Products.Any(e => e.Id == id);
        }
    }
}
