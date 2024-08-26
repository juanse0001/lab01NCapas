using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using BLL;
using Entities.Models;
using BLL.Exceptions;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Service.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase //CAPA DE SERVICIOS     
    {
        private readonly Products _bll;

        public ProductController(Products bll)
        {
            _bll = bll;
        }

        // GET: api/Product
        [HttpGet]
        public async Task<ActionResult<List<Product>>> GetAll()
        {
            try
            {
                var result = await _bll.RetrieveAllAsync();
                return Ok(result);
            }
            catch (ProductExceptions ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                // Log the exception
                // LogError(ex);
                return StatusCode(StatusCodes.Status500InternalServerError, "An unexpected error occurred.");
            }
        }

        // GET api/Product/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> RetrieveAsync(int id)
        {
            try
            {
                var product = await _bll.RetrieveByIDAsync(id);

                if (product == null)
                {
                    return NotFound("Product not found.");
                }

                return Ok(product);
            }
            catch (ProductExceptions ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                // Log the exception
                // LogError(ex);
                return StatusCode(StatusCodes.Status500InternalServerError, "An unexpected error occurred.");
            }
        }

        // POST: api/Product
        [HttpPost]
        public async Task<ActionResult<Product>> CreateAsync([FromBody] Product toCreate)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Invalid data.");
            }

            try
            {
                var product = await _bll.CreateAsync(toCreate);
                return CreatedAtAction(nameof(RetrieveAsync), new { id = product.Id }, product);
            }
            catch (ProductExceptions ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                // Log the exception
                // LogError(ex);
                return StatusCode(StatusCodes.Status500InternalServerError, "An unexpected error occurred.");
            }
        }

        // PUT api/Product/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAsync(int id, [FromBody] Product toUpdate)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Invalid data.");
            }

            toUpdate.Id = id;
            try
            {
                var result = await _bll.UpdateAsync(toUpdate);
                if (!result)
                {
                    return NotFound("Product not found or update failed.");
                }
                return NoContent();
            }
            catch (ProductExceptions ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                // Log the exception
                // LogError(ex);
                return StatusCode(StatusCodes.Status500InternalServerError, "An unexpected error occurred.");
            }
        }

        // DELETE api/Product/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            try
            {
                var result = await _bll.DeleteAsync(id);
                if (!result)
                {
                    return NotFound("Product not found or deletion failed.");
                }
                return NoContent();
            }
            catch (ProductExceptions ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                // Log the exception
                // LogError(ex);
                return StatusCode(StatusCodes.Status500InternalServerError, "An unexpected error occurred.");
            }
        }
    }
}
