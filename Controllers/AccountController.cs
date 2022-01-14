using Blog.Data;
using Blog.Extensions;
using Blog.Models;
using Blog.Services;
using Blog.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SecureIdentity.Password;

namespace Blog.Controllers
{
    [ApiController]
    //[Authorize]
    public class AccountController : ControllerBase
    {
        [HttpPost("v1.0/accounts")]
        public async Task<IActionResult> Post([FromServices] EmailService emailService, 
                                              [FromBody] RegisterViewModel model, 
                                              [FromServices] DataContext context)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ResultViewModel<string>(ModelState.GetErrors()));
            }
            var user = new User
            {
                Email = model.Email,
                Name = model.Name,
                Slug = model.Email.Replace("@", "-").Replace(".", "-"),
            };

            var password = PasswordGenerator.Generate(length: 25);
            user.PasswordHash = PasswordHasher.Hash(password);

            try
            {
                await context.Users.AddAsync(user);
                await context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                return StatusCode(400, new ResultViewModel<string>("Email já existe no banco de dados, impossível cadastrar"));
                throw;
            }
            catch
            {
                return StatusCode(500, new ResultViewModel<string>("aaaa Erro ao salvar no servidor"));
                throw;
            }

            var deuCerto = emailService.send(user.Name, user.Email, "Cadastro feito com sucesso", $"Sua senha é: {password}");

            if(deuCerto == true)
            {
                return Ok(new ResultViewModel<string>("Cadastro feito com sucesso, confira seu Email para obter sua senha", null));
            }
            return StatusCode(500, new ResultViewModel<string>("Erro ao salvar no servidor"));
        }

        [HttpPost("v1.0/accounts/login")]
        public async Task<IActionResult> Login([FromServices] DataContext context, [FromServices] TokenServices tokenService, [FromBody] LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ResultViewModel<string>(ModelState.GetErrors()));
            }

            var user = await context.Users.AsNoTracking().Include(x => x.Roles).FirstOrDefaultAsync(x => x.Email == model.Email);

            var IsTrue = PasswordHasher.Verify(user.PasswordHash, model.Password);
            if (IsTrue == false)
            {
                return StatusCode(401, new ResultViewModel<string>("Senha ou usuário inválido"));
            }
            try
            {
                var token = tokenService.GenerateToken(user);
                return Ok(new ResultViewModel<string>(token, null));
            }
            catch
            {
                return StatusCode(500, "Erro interno ao criar o Token de acesso");
                throw;
            }
        }
    }
}
