﻿using Microsoft.AspNetCore.Mvc;
using Website.Shared.Models;
using WesiteWebApi.Repositories.Products;

namespace WesiteWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductRepository _productRepository;

        public ProductsController(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        // GET: api/Products
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Product>>> GetProducts()
        {
            List<Product>? products = await _productRepository.GetAllAsync();
            return products is null ? NotFound() : products;
        }

        // GET: api/Products/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> GetProduct(int id)
        {
            Product? product = await _productRepository.GetByIdAsync(id);

            if (product == null)
            {
                return NotFound();
            }

            return product;
        }

        // PUT: api/Products/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutProduct(int id, Product product)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    int affectedRow = await _productRepository.UpdateAsync(id, product);
                    return affectedRow switch
                    {
                        0 => NotFound(),
                        -1 => BadRequest(),
                        _ => NoContent(),
                    };
                }
                catch (Exception ex)
                {
                    // Log Exception
                    return BadRequest(ex.Message);
                }
            }
            else
            {
                return BadRequest(ModelState);
            }
        }

        // POST: api/Products
        [HttpPost]
        public async Task<ActionResult<Product>> PostProduct(Product product)
        {
            if (ModelState.IsValid)
            {
                int affectedRow = await _productRepository.InsertAsync(product);
                return affectedRow switch
                {
                    0 => BadRequest("Error"),
                    _ => CreatedAtAction("GetCategory", new { id = product.Id }, product),

                };
            }
            else
            {
                return BadRequest(ModelState);
            }
        }

        // DELETE: api/Products/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            int affectedRow = await _productRepository.DeleteAsync(id);
            if (affectedRow == 0)
            {
                return NotFound();
            }
            else
            {
                return NoContent();
            }
        }


    }
}
