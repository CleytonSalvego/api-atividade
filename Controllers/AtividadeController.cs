using api.Data;
using api.Extensions;
using api.Models;
using api.ViewModels;
using api.ViewModels.Result;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.Controllers
{
    [ApiController]
    public class AtividadeController : ControllerBase
    {
        [Authorize]
        [HttpGet("v1/atividades")]
        public async Task<IActionResult> GetAsync(
          [FromServices] DataBaseContext context,
          [FromQuery] int page = 0,
          [FromQuery] int pageSize = 25)
        {
            try
            {
               
                var count = await context.Atividade.AsNoTracking().CountAsync();

                var data = await context.
                    Atividade
                    .AsNoTracking()
                    .Include(x => x.status)
                    .Include(x => x.criador)
                    .OrderByDescending(p => p.codigo)
                    .Select(x => new
                    {
                        codigo = x.codigo,
                        numero_documento = x.numero_documento,
                        titulo = x.titulo,
                        descricao = x.descricao,
                        data_criacao = x.data_criacao,
                        solicitante = x.solicitante,
                        codigo_status = x.codigo_status,
                        status = x.status.descricao,
                        data_planejamento = x.data_planejamento,
                        codigo_criador = x.codigo_criador,
                        criador = x.criador.nome
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
                    data
                }));
            }
            catch
            {
                return StatusCode(500, new ResultViewModel<List<EditAtividadeViewModel>>("05X01 - Falha interna no servidor"));
            }
        }


        [Authorize]
        [HttpGet("v1/atividades/{codigo}")]
        public async Task<IActionResult> GetByIdAsync(
            [FromRoute] int codigo,
            [FromServices] DataBaseContext context)
        {

            try
            {
                var data = await context
                    .Atividade
                    .AsNoTracking()
                    .Include(x => x.status)
                    .Include(x => x.criador)
                    .Select(x => new
                    {
                        codigo = x.codigo,
                        numero_documento = x.numero_documento,
                        titulo = x.titulo,
                        descricao = x.descricao,
                        data_criacao = x.data_criacao,
                        solicitante = x.solicitante,
                        codigo_status = x.codigo_status,
                        status = x.status.descricao,
                        data_planejamento = x.data_planejamento,
                        codigo_criador = x.codigo_criador,
                        criador = x.criador.nome
                    })
                    .FirstOrDefaultAsync(x => x.codigo == codigo);

                if (data == null)
                    return NotFound(new ResultViewModel<EditAtividadeViewModel>("05X02 - Conteúdo não encontrado"));

                return Ok(new ResultViewModel<dynamic>(new
                {
                    data
                }));
            }
            catch
            {
                return StatusCode(500, new ResultViewModel<EditAtividadeViewModel>("05X02 - Falha interna no servidor"));
            }
        }

        [Authorize]
        [Authorize(Roles = "admin")]
        [HttpPost("v1/atividades")]
        public async Task<IActionResult> PostAsync(
            [FromBody] EditAtividadeViewModel model,
            [FromServices] DataBaseContext context)
        {

            if (!ModelState.IsValid)
                return BadRequest(new ResultViewModel<EditAtividadeViewModel>(ModelState.GetErrors()));

            try
            {
                //Pega o código máximo
                var codigo = await context.Atividade
                                          .AsNoTracking()
                                          .OrderByDescending(p => p.codigo)
                                          .Select(x => x.codigo)
                                          .FirstOrDefaultAsync() + 1;

                //Seta Dados
                var data = new AtividadeModel
                {

                    codigo = codigo,
                    data_criacao = DateTime.Now,
                    numero_documento = codigo,
                    titulo = model.titulo,
                    descricao = model.descricao,
                    solicitante = model.solicitante,
                    codigo_status = model.codigo_status,
                    data_planejamento = model.data_planejamento,
                    codigo_criador = model.codigo_criador
                };

                await context.Atividade.AddAsync(data);
                await context.SaveChangesAsync();

                return Ok(new ResultViewModel<AtividadeModel>(data));

            }
            catch (DbUpdateException error)
            {
                return StatusCode(500, new ResultViewModel<EditAtividadeViewModel>("05X03 - Não foi possível incluir os dados, esse usuário já está cadastrado."));
            }
            catch
            {
                return StatusCode(500, new ResultViewModel<EditAtividadeViewModel>("05X03 - Falha interna no servidor"));
            }
        }

        [Authorize]
        [Authorize(Roles = "admin")]
        [HttpPut("v1/atividades/{codigo}")]
        public async Task<IActionResult> PutAsync(
            [FromRoute] int codigo,
            [FromBody] EditAtividadeViewModel model,
            [FromServices] DataBaseContext context)
        {

            try
            {
                var data = await context
                   .Atividade
                   .FirstOrDefaultAsync(x => x.codigo == codigo);

                if (data == null)
                    return NotFound(new ResultViewModel<EditAtividadeViewModel>("05X03 - Conteúdo não encontrado."));


                data.data_alteracao = DateTime.Now;
                data.titulo = model.titulo;
                data.descricao = model.descricao;
                data.solicitante = model.solicitante;
                data.codigo_status = model.codigo_status;
                data.data_planejamento = model.data_planejamento;


                context.Atividade.Update(data);
                await context.SaveChangesAsync();


                return Ok(new ResultViewModel<AtividadeModel>(data));
            }
            catch (DbUpdateException error)
            {
                return StatusCode(500, new ResultViewModel<EditAtividadeViewModel>("05X03 - Não foipossível alterar os dados."));
            }
            catch
            {
                return StatusCode(500, new ResultViewModel<EditAtividadeViewModel>("05X04 - Falha interna no servidor"));
            }



        }

        [Authorize]
        [Authorize(Roles = "admin")]
        [HttpDelete("v1/atividades/{codigo}")]
        public async Task<IActionResult> DeleteAsync(
            [FromRoute] int codigo,
            [FromServices] DataBaseContext context)
        {

            try
            {
                var data = await context
                   .Atividade
                   .FirstOrDefaultAsync(x => x.codigo == codigo);

                if (data == null)
                    return NotFound(new ResultViewModel<AtividadeModel>("05X03 - Conteúdo não encontrado."));

                context.Atividade.Remove(data);
                await context.SaveChangesAsync();


                return Ok(new ResultViewModel<AtividadeModel>(data));
            }
            catch (DbUpdateException error)
            {
                return StatusCode(500, new ResultViewModel<AtividadeModel>("05X03 - Não foipossível excluir os dados."));
            }
            catch
            {
                return StatusCode(500, new ResultViewModel<AtividadeModel>("05X05 - Falha interna no servidor"));
            }



        }
    }
}
