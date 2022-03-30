using api.Data;
using api.Extensions;
using api.Models;
using api.ViewModels.Result;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using api.ViewModels.Perfil;
using System.Threading.Tasks;
using System.Linq;
using System.Collections.Generic;

namespace api.Controllers
{

    [ApiController]
    public class PerfilController : ControllerBase
    {

        [Authorize]
        [HttpGet("v1/perfils")]
        public async Task<IActionResult> GetAsync(
            [FromServices] DataBaseContext context,
            [FromQuery] int page = 0,
            [FromQuery] int pageSize = 25)
        {
            try
            {
                //Pega o total de registro no banco de dados para retornar para o front-end
                var count = await context.Perfil.AsNoTracking().CountAsync();

                //Monta Array Header da tabela
                var header = new
                {
                    codigo = "Código",
                    descricao = "Descrição"
                };

                //Monta Array de Dados
                var data = await context.
                    Perfil
                    .AsNoTracking()
                    .Select(x => new 
                    {
                        codigo = x.codigo,
                        descricao = x.descricao,
                    })
                    .Skip(page * pageSize) //Pega a pagina atual e pula de acordo com o tamanho da pagina
                    .Take(pageSize)
                    .ToListAsync();

                //Retorna dados
                return Ok(new ResultViewModel<dynamic>(new 
                {
                    total = count,
                    page,
                    pageSize,
                    header,
                    data
                }));
            }
            catch
            {
                return StatusCode(500, new ResultViewModel<List<EditPerfilViewModel>>("05X01 - Falha interna no servidor"));
            }
        }


        [Authorize]
        [HttpGet("v1/perfils/{codigo}")]
        public async Task<IActionResult> GetByIdAsync(
            [FromRoute] int codigo,
            [FromServices] DataBaseContext context)
        {

            try
            {
                //Monta Array Header da tabela
                var header = new
                {
                    codigo = "Código",
                    descricao = "Descrição"
                };

                //Monta Array de dados
                var data = await context
                    .Perfil
                    .AsNoTracking()
                    .Select(x => new
                    {
                        codigo = x.codigo,
                        descricao = x.descricao,
                    })
                    .FirstOrDefaultAsync(x => x.codigo == codigo);

                if (data == null)
                    return NotFound(new ResultViewModel<EditPerfilViewModel>("05X02 - Conteúdo não encontrado"));

                return Ok(new ResultViewModel<dynamic>(new
                {
                    header,
                    data
                }));
            }
            catch
            {
                return StatusCode(500, new ResultViewModel<EditPerfilViewModel>("05X02 - Falha interna no servidor"));
            }
        }

        [Authorize]
        [Authorize(Roles = "admin")]
        [HttpPost("v1/perfils")]
        public async Task<IActionResult> PostAsync(
            [FromBody] EditPerfilViewModel model,
            [FromServices] DataBaseContext context)
        {

            if (!ModelState.IsValid)
                return BadRequest(new ResultViewModel<EditPerfilViewModel>(ModelState.GetErrors()));

            try
            {

                var data = new PerfilModel
                {
                    descricao = model.descricao,
                };

                await context.Perfil.AddAsync(data);
                await context.SaveChangesAsync();

                return Ok(new ResultViewModel<dynamic>(new
                {
                    codigo = data.codigo,
                    descricao = data.descricao

                }));
            }
            catch (DbUpdateException error)
            {
                return StatusCode(500, new ResultViewModel<EditPerfilViewModel>("05X03 - Não foi possível incluir os dados, esse usuário já está cadastrado."));
            }
            catch
            {
                return StatusCode(500, new ResultViewModel<EditPerfilViewModel>("05X03 - Falha interna no servidor"));
            }
        }

        [Authorize]
        [Authorize(Roles = "admin")]
        [HttpPut("v1/perfils/{codigo}")]
        public async Task<IActionResult> PutAsync(
            [FromRoute] int codigo,
            [FromBody] EditPerfilViewModel model,
            [FromServices] DataBaseContext context)
        {

            try
            {
                var data = await context
                   .Perfil
                   .FirstOrDefaultAsync(x => x.codigo == codigo);

                if (data == null)
                    return NotFound(new ResultViewModel<EditPerfilViewModel>("05X03 - Conteúdo não encontrado."));

                data.descricao = model.descricao;

                context.Perfil.Update(data);
                await context.SaveChangesAsync();


                return Ok(new ResultViewModel<PerfilModel>(data));
            }
            catch (DbUpdateException error)
            {
                return StatusCode(500, new ResultViewModel<EditPerfilViewModel>("05X03 - Não foipossível alterar os dados."));
            }
            catch
            {
                return StatusCode(500, new ResultViewModel<EditPerfilViewModel>("05X04 - Falha interna no servidor"));
            }



        }

        [Authorize]
        [Authorize(Roles = "admin")]
        [HttpDelete("v1/perfils/{codigo}")]
        public async Task<IActionResult> DeleteAsync(
            [FromRoute] int codigo,
            [FromServices] DataBaseContext context)
        {

            try
            {
                var data = await context
                   .Perfil
                   .FirstOrDefaultAsync(x => x.codigo == codigo);

                if (data == null)
                    return NotFound(new ResultViewModel<PerfilModel>("05X03 - Conteúdo não encontrado."));

                context.Perfil.Remove(data);
                await context.SaveChangesAsync();


                return Ok(new ResultViewModel<PerfilModel>(data));
            }
            catch (DbUpdateException error)
            {
                return StatusCode(500, new ResultViewModel<PerfilModel>("05X03 - Não foipossível excluir os dados."));
            }
            catch
            {
                return StatusCode(500, new ResultViewModel<PerfilModel>("05X05 - Falha interna no servidor"));
            }



        }
    }
}

