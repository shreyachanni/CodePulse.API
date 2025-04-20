using CodePulse.API.Data;
using CodePulse.API.Models.Domain;
using CodePulse.API.Models.DTO;
using CodePulse.API.Repositories.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CodePulse.API.Controllers
{
    //https://localhost:xxxx/api/Categories
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoryRepository categoryRepository;

        public CategoriesController(ICategoryRepository categoryRepository)
        {
            this.categoryRepository = categoryRepository;
        }


        //https://localhost:xxxx/api/Categories
        [HttpPost]
        public async Task<IActionResult> CreateCategory(CreateCategoryRequestDto request)
        {
            //Map DTO to Domain Model
            var category = new Category
            {
                Name = request.Name,
                UrlHandle = request.UrlHandle
            };

            await categoryRepository.CreateAsync(category);

            // Domain to DTO
            var response = new CategoryDto
            {
                Id = category.Id,
                Name = category.Name,
                UrlHandle = category.UrlHandle
            };

            return Ok(response);
        }

        //https://localhost:xxxx/api/Categories
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Category>>> GetAllCategories()
        {
            var categories = await categoryRepository.GetAllCategoryAsync();

            // Map Domain model to DTO
            var response = new List<CategoryDto>();
            foreach (var category in categories)
            {
                response.Add(new CategoryDto
                {
                    Id = category.Id,
                    Name = category.Name,
                    UrlHandle = category.UrlHandle
                });
            }

            if (response == null)
            {
                return NotFound();
            }

            return Ok(response);
        }


        //https://localhost:xxxx/api/Categories
        [HttpGet]
        [Route("{id:guid}")]
        public async Task<IActionResult> GetCategoryById([FromRoute] Guid id)
        {
            var category = await categoryRepository.GetCategoryByIdAsync(id);

            if (category == null)
            {
                return NotFound();
            }

            var response = new CategoryDto
            {
                Id = category.Id,
                Name = category.Name,
                UrlHandle = category.UrlHandle
            };

            return Ok(response);

        }

        [HttpPut]
        [Route("{id:guid}")]
        public async Task<IActionResult> UpdateCategory([FromRoute] Guid id, [FromBody] UpdateCategoryDto request)
        {
            // Convert DTO to Domain
            var category = new Category
            {
                Id = id,
                Name = request.Name,
                UrlHandle = request.UrlHandle
            };

            category = await categoryRepository.UpdateCategoryAsync(category);

            if (category == null)
            {
                return NotFound();
            }

            var response = new CategoryDto
            {
                Id = category.Id,
                Name = category.Name,
                UrlHandle = category.UrlHandle

            };

            return Ok(response);
        }


        [HttpPatch]
        [Route("{id:guid}")]
        public async Task<IActionResult> PatchUpdateCategory([FromRoute] Guid id, [FromBody] JsonPatchDocument<UpdateCategoryDto> patchDoc)
        {
            //check if patchdoc is null 
            if (patchDoc == null)
            {
                return BadRequest();
            }

            var existingCategory = await categoryRepository.GetCategoryByIdAsync(id);

            if (existingCategory == null)
            {
                return NotFound();
            }

            var response = new UpdateCategoryDto
            {
                Name = existingCategory.Name,
                UrlHandle = existingCategory.UrlHandle
            };

            patchDoc.ApplyTo(response);

            return Ok(response);

        }


        [HttpDelete]
        [Route("{id:guid}")]

        public async Task<IActionResult> DeleteCategory([FromRoute] Guid id)
        {
            var deleteditem = await categoryRepository.DeleteCategoryAsync(id);
            //return Ok(new { Message = "Category deleted successfully." });


            if(deleteditem == null)
    {
                return NotFound(new { Message = "Category not found." });
            }

            var response = new CategoryDto
            {
                Name = deleteditem.Name
            };

            return Ok(new
            {
                Message = $"{response.Name} deleted successfully."
            });
        }


    }
}
