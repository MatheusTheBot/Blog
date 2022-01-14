using Blog.Data;
using Blog.Extensions;
using Blog.Models;
using Blog.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Blog.Controllers
{
    [ApiController]
    public class CategoryController : ControllerBase
    {
        [HttpPost("v1.0/categories/")]
        public async Task<IActionResult> PostAsync([FromServices] DataContext context, [FromBody] EditorCategoryViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ResultViewModel<Category>(ModelState.GetErrors()));
            }
            var category = new Category
            {
                Name = model.Name,
                Slug = model.Slug.ToLower(),
                Id = 0,
            };

            try
            {
                await context.Categories.AddAsync(category);
                await context.SaveChangesAsync();
            }
            catch (BadHttpRequestException)
            {
                BadRequest(new ResultViewModel<Category>("BR01 - Requisição não permitida ou não suportada"));
                throw;
            }
            catch (Exception)
            {
                StatusCode(500, new ResultViewModel<Category>("ED01 - Erro desconhecido"));
                throw;
            }

            return Created("v1.0/categories/{category.Id}", new ResultViewModel<Category>(category));
        }

        [HttpGet("v1.0/categories")]
        public async Task<IActionResult> Get([FromServices] DataContext context)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ResultViewModel<Category>(ModelState.GetErrors()));
            }

            List<Category>? categories;
            try
            {
                categories = await context.Categories.ToListAsync();
            }
            catch (BadHttpRequestException)
            {
                BadRequest(new ResultViewModel<List<Category>>("BR02 - Requisição não suportada ou não permitida"));
                throw;
            }
            catch (Exception)
            {
                StatusCode(500, new ResultViewModel<List<Category>>("ED02 - Erro desconhecido"));
                throw;
            }
            return Ok(new ResultViewModel<List<Category>>(categories));
        }

        [HttpGet("v1.0/categories/{Id:int}")]
        public async Task<IActionResult> GetById([FromServices] DataContext context, [FromRoute] int Id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ResultViewModel<Category>(ModelState.GetErrors()));
            }

            Category? category;
            try
            {
                category = await context.Categories.FirstOrDefaultAsync(x => x.Id == Id);
            }
            catch (BadHttpRequestException)
            {
                BadRequest(new ResultViewModel<Category>("BR03 - Requisição não suportada ou não permitida"));
                throw;
            }
            catch (Exception)
            {
                StatusCode(500, new ResultViewModel<Category>("ED03 - Erro desconhecido"));
                throw;
            }
            if (category == null)
            {
                return NotFound(new ResultViewModel<Category>("NF01 - Item não encontrado ou inválido"));
            }
            return Ok(new ResultViewModel<Category>(category));
        }

        [HttpPut("v1.0/categories/{Id:int}")]
        public async Task<IActionResult> PutAsync([FromServices] DataContext context, [FromRoute] int Id, [FromBody] EditorCategoryViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ResultViewModel<Category>(ModelState.GetErrors()));
            }

            Category? oldModel;
            try
            {
                oldModel = await context.Categories.FirstOrDefaultAsync(x => x.Id == Id);
            }
            catch (BadHttpRequestException)
            {
                BadRequest(new ResultViewModel<Category>("BR04 - Requisição não suportada ou não permitida"));
                throw;
            }
            catch (Exception)
            {
                StatusCode(500, new ResultViewModel<Category>("ED04 - Erro desconhecido"));
                throw;
            }
            if (oldModel == null)
            {
                return NotFound(new ResultViewModel<Category>("NF02 - Item não encontrado"));
            }

            try
            {
                oldModel.Name = model.Name;
                oldModel.Slug = model.Slug;

                context.Update(oldModel);
                await context.SaveChangesAsync();
            }
            catch (Exception)
            {
                StatusCode(500, new ResultViewModel<Category>("ED05 - Erro Desconhecido"));
                throw;
            }

            return Ok(new ResultViewModel<Category>(oldModel));
        }

        [HttpDelete("v1.0/categories/{Id:int}")]
        public async Task<IActionResult> DeleteAsync([FromServices] DataContext context, [FromRoute] int Id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ResultViewModel<Category>(ModelState.GetErrors()));
            }

            Category? modelToDelete;
            try
            {
                modelToDelete = await context.Categories.FirstOrDefaultAsync(x => x.Id == Id);
            }
            catch (BadHttpRequestException)
            {
                BadRequest(new ResultViewModel<Category>("BR06 - Requisição não suportada ou não permitida"));
                throw;
            }
            catch (Exception)
            {
                StatusCode(500, new ResultViewModel<Category>("ED06 - Erro desconhecido"));
                throw;
            }
            if (modelToDelete == null)
            {
                return NotFound(new ResultViewModel<Category>("NF03 - Item não encontrado"));
            }

            try
            {
                context.Categories.Remove(modelToDelete);
                await context.SaveChangesAsync();
            }
            catch (Exception)
            {
                StatusCode(500, new ResultViewModel<Category>("ED07 - Erro desconhecido"));
                throw;
            }

            return Ok(new ResultViewModel<Category>(modelToDelete));
        }
    }
}