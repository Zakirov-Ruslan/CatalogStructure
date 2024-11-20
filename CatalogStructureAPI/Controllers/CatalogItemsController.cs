using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CatalogStructureAPI.Services;
using CatalogStructureDto;

namespace CatalogStructureAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CatalogItemsController : ControllerBase
    {
        private readonly CatalogItemsService _catalogItems;

        public CatalogItemsController(CatalogItemsService catalogItems)
        {
            _catalogItems = catalogItems;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CatalogItemDto>>> GetCatalogItems()
        {
            var catalogItemsDto = await _catalogItems.GetCatalogItemsAsync();

            return Ok(catalogItemsDto);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<CatalogItemDto>> GetCatalogItem(int id)
        {
            var catalogItemDto = await _catalogItems.GetCatalogItemAsync(id);

            if (catalogItemDto == null)
            {
                return NotFound();
            }

            return catalogItemDto;
        }

        [HttpPost]
        public async Task<ActionResult<CatalogItemDto>> PostCatalogItem(CatalogItemCreateDto catalogItemDto)
        {
            try
            {
                var createdCatalogItem = await _catalogItems.CreateCatalogItemAsync(catalogItemDto);

                return CreatedAtAction("GetCatalogItem", new { id = createdCatalogItem.Id }, createdCatalogItem);
            }
            catch (Exception ex)
            {
                if (ex is ArgumentException)
                    return BadRequest(ex.Message);
                else
                    return StatusCode(500, ex.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutCatalogItem(int id, CatalogItemUpdateDto catalogItemDto)
        {
            if (id != catalogItemDto.Id)
            {
                return BadRequest();
            }

            try
            {
                await _catalogItems.UpdateCatalogItemAsync(catalogItemDto);

                return NoContent();
            }
            catch (Exception ex)
            {
                if (ex is ArgumentException)
                    return BadRequest(ex.Message);
                if (ex is KeyNotFoundException)
                    return NotFound(ex.Message);
                else
                    return StatusCode(500, ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCatalogItem(int id)
        {
            try
            {
                await _catalogItems.DeleteCatalogItemAsync(id);

                return NoContent();
            }
            catch (Exception ex)
            {
                if (ex is KeyNotFoundException)
                    return NotFound(ex.Message);
                else
                    return StatusCode(500, ex.Message);
            }
        }
    }
}
