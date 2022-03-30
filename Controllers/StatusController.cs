using api.Data;
using api.Extensions;
using api.Models;
using api.ViewModels;
using api.ViewModels.Result;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.Controllers
{
    [ApiController]
    public class StatusController : ControllerBase
    {
        [Authorize]
        [HttpGet("v1/status")]
        public async Task<IActionResult> GetAsync(
           [FromServices] DataBaseContext context,
           [FromQuery] int page = 0,
           [FromQuery] int pageSize = 25)
        {
            try
            {
                //Pega o total de registro no banco de dados para retornar para o front-end
                var count = await context.Status.AsNoTracking().CountAsync();

                //Monta Array Header da tabela
                var header = new
                {
                    codigo = "Código",
                    descricao = "Descrição"
                };

                //Monta Array de dados
                var data = await context.
                    Status
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
                return StatusCode(500, new ResultViewModel<List<EditStatusViewModel>>("05X01 - Falha interna no servidor"));
            }
        }


        [Authorize]
        [HttpGet("v1/status/{codigo}")]
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
                    .Status
                    .AsNoTracking()
                    .Select(x => new
                    {
                        codigo = x.codigo,
                        descricao = x.descricao,
                    })
                    .FirstOrDefaultAsync(x => x.codigo == codigo);

                if (data == null)
                    return NotFound(new ResultViewModel<EditStatusViewModel>("05X02 - Conteúdo não encontrado"));

                return Ok(new ResultViewModel<dynamic>(new
                {
                    header,
                    data
                }));
            }
            catch
            {
                return StatusCode(500, new ResultViewModel<EditStatusViewModel>("05X02 - Falha interna no servidor"));
            }
        }

        [Authorize]
        [Authorize(Roles = "admin")]
        [HttpPost("v1/status")]
        public async Task<IActionResult> PostAsync(
            [FromBody] EditStatusViewModel model,
            [FromServices] DataBaseContext context)
        {

            if (!ModelState.IsValid)
                return BadRequest(new ResultViewModel<EditStatusViewModel>(ModelState.GetErrors()));

            try
            {

                var data = new StatusModel
                {
                    descricao = model.descricao,
                };

                await context.Status.AddAsync(data);
                await context.SaveChangesAsync();

                return Ok(new ResultViewModel<dynamic>(new
                {
                    codigo = data.codigo,
                    descricao = data.descricao

                }));
            }
            catch (DbUpdateException error)
            {
                return StatusCode(500, new ResultViewModel<EditStatusViewModel>("05X03 - Não foi possível incluir os dados, esse usuário já está cadastrado."));
            }
            catch
            {
                return StatusCode(500, new ResultViewModel<EditStatusViewModel>("05X03 - Falha interna no servidor"));
            }
        }

        [Authorize]
        [Authorize(Roles = "admin")]
        [HttpPut("v1/status/{codigo}")]
        public async Task<IActionResult> PutAsync(
            [FromRoute] int codigo,
            [FromBody] EditStatusViewModel model,
            [FromServices] DataBaseContext context)
        {

            try
            {
                var data = await context
                   .Status
                   .FirstOrDefaultAsync(x => x.codigo == codigo);

                if (data == null)
                    return NotFound(new ResultViewModel<EditStatusViewModel>("05X03 - Conteúdo não encontrado."));

                data.descricao = model.descricao;

                context.Status.Update(data);
                await context.SaveChangesAsync();


                return Ok(new ResultViewModel<StatusModel>(data));
            }
            catch (DbUpdateException error)
            {
                return StatusCode(500, new ResultViewModel<EditStatusViewModel>("05X03 - Não foipossível alterar os dados."));
            }
            catch
            {
                return StatusCode(500, new ResultViewModel<EditStatusViewModel>("05X04 - Falha interna no servidor"));
            }



        }

        [Authorize]
        [Authorize(Roles = "admin")]
        [HttpDelete("v1/status/{codigo}")]
        public async Task<IActionResult> DeleteAsync(
            [FromRoute] int codigo,
            [FromServices] DataBaseContext context)
        {

            try
            {
                var data = await context
                   .Status
                   .FirstOrDefaultAsync(x => x.codigo == codigo);

                if (data == null)
                    return NotFound(new ResultViewModel<StatusModel>("05X03 - Conteúdo não encontrado."));

                context.Status.Remove(data);
                await context.SaveChangesAsync();


                return Ok(new ResultViewModel<StatusModel>(data));
            }
            catch (DbUpdateException error)
            {
                return StatusCode(500, new ResultViewModel<StatusModel>("05X03 - Não foipossível excluir os dados."));
            }
            catch
            {
                return StatusCode(500, new ResultViewModel<StatusModel>("05X05 - Falha interna no servidor"));
            }



        }
    }
}
