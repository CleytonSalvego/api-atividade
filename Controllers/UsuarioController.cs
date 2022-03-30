using api.Data;
using api.Extensions;
using api.Models;
using api.Services;
using api.ViewModels.Result;
using api.ViewModels.Users;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SecureIdentity.Password;
using Microsoft.AspNetCore.Authorization;
using api.ViewModels;
using System.Threading.Tasks;
using System.Linq;
using System.Collections.Generic;
using System;

namespace api.Controllers
{

    [ApiController]
    public class UsuarioController : ControllerBase
    {

        [HttpPost("v1/login")]
        public async Task<IActionResult> LoginAsync(
            [FromBody] LoginViewModel model,
            [FromServices] DataBaseContext context,
            [FromServices] TokenService tokenService)
        {

            if (!ModelState.IsValid)
                return BadRequest(new ResultViewModel<string>(ModelState.GetErrors()));

            var usuario = await context
                   .Usuario
                   .AsNoTracking()
                   .Include(x => x.perfil) //SubSelect via ORM adiciona dados perfil
                   .FirstOrDefaultAsync(x => x.usuario == model.usuario);

            if (usuario == null)
                return StatusCode(401, new ResultViewModel<string>("Usuário inválido"));

            if (!PasswordHasher.Verify(usuario.senha, model.senha + Configuration.JwtKey))
                return StatusCode(401, new ResultViewModel<string>("Senha inválido"));

            try
            {
                var token = tokenService.GenerateToken(usuario);

                return Ok(new ResultViewModel<dynamic>(new
                {
                    
                    codigo = usuario.codigo,
                    nome = usuario.nome,
                    perfil = usuario.perfil.descricao,
                    token = token,
                }));
            }
            catch (Exception)
            {

                throw;
            }
            
            
           


        }

        //Autorização
        [Authorize]
        //Exemplo para inserir mais de um perfil 
        //[Authorize(Roles = "admin, usuario, estagio")]
        [HttpGet("v1/usuarios")]
        public async Task<IActionResult> GetAsync(
            [FromServices] DataBaseContext context,
            [FromQuery] int page = 0,
            [FromQuery] int pageSize = 25)
        {
            try
            {
                //Pega o total de registro no banco de dados para retornar para o front-end
                var count = await context.Usuario.AsNoTracking().CountAsync();

                //Monta Arrya de dados
                var data = await context.
                    Usuario
                    .AsNoTracking()
                    .Include(x => x.perfil)
                    .Select(x => new ListUsuarioViewModel
                    {
                        codigo = x.codigo,
                        nome = x.nome,
                        codigo_perfil = x.codigo_perfil,
                        perfil = x.perfil.descricao
                    })
                    .Skip(page*pageSize) //Pega a pagina atual e pula de acordo com o tamanho da pagina
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
                return StatusCode(500, new ResultViewModel<List<UsuarioModel>>("05X01 - Falha interna no servidor"));
            }
        }

        
        [Authorize]
        [HttpGet("v1/usuarios/{codigo}")]
        public async Task<IActionResult> GetByIdAsync(
            [FromRoute] int codigo,
            [FromServices] DataBaseContext context)
        {

            try
            {

                //Monta Array de dados
                var data = await context
                    .Usuario
                    .AsNoTracking()
                    .Include(x => x.perfil)
                    .Select(x=> new ListUsuarioViewModel
                    {
                        codigo = x.codigo,
                        nome = x.nome,
                        codigo_perfil = x.codigo_perfil,
                        perfil = x.perfil.descricao
                    

                    })
                    .FirstOrDefaultAsync(x => x.codigo == codigo);

                if (data == null)
                    return NotFound(new ResultViewModel<UsuarioModel>("05X02 - Conteúdo não encontrado"));

                return Ok(new ResultViewModel<dynamic>(new
                {
                    data
                }));
            }
            catch
            {
                return StatusCode(500, new ResultViewModel<UsuarioModel>("05X02 - Falha interna no servidor"));
            }
        }

        

        //[Authorize]
        //[Authorize(Roles = "admin")]
        [HttpPost("v1/usuarios")]
        public async Task<IActionResult> PostAsync(
            [FromBody] EditUsuarioViewModel model,
            [FromServices] DataBaseContext context)
        {

            if (!ModelState.IsValid)
                return BadRequest(new ResultViewModel<UsuarioModel>(ModelState.GetErrors()));

            try
            {

                //Gera Senha padrão + chave de segurança
                var password = PasswordHasher.Hash(Configuration.PasswordKey + Configuration.JwtKey);


                var user = new UsuarioModel
                {
                    usuario = model.usuario,
                    senha = password,
                    nome = model.nome,
                    codigo_perfil = model.codigo_perfil,
                    ativo = model.ativo
            };

                await context.Usuario.AddAsync(user);
                await context.SaveChangesAsync();

                return Created("", new ResultViewModel<dynamic>(new
                {
                    usuario = user.usuario,
                    nome = user.nome

                }));
            }
            catch (DbUpdateException error)
            {
                return StatusCode(500, new ResultViewModel<UsuarioModel>("05X03 - Não foi possível incluir os dados, esse usuário já está cadastrado."));
            }
            catch
            {
                return StatusCode(500, new ResultViewModel<UsuarioModel>("05X03 - Falha interna no servidor"));
            }
        }

        [Authorize]
        [Authorize(Roles = "admin")]
        [HttpPut("v1/usuarios/{codigo}")]
        public async Task<IActionResult> PutAsync(
            [FromRoute] int codigo,
            [FromBody] EditUsuarioViewModel model,
            [FromServices] DataBaseContext context)
        {

            try
            {
                var usuario = await context
                   .Usuario
                   .FirstOrDefaultAsync(x => x.codigo == codigo);

                if (usuario == null)
                    return NotFound(new ResultViewModel<UsuarioModel>("05X03 - Conteúdo não encontrado."));

                usuario.usuario = model.usuario;
                usuario.nome = model.nome;
                usuario.codigo_perfil = model.codigo_perfil;
                usuario.ativo = model.ativo;

                context.Usuario.Update(usuario);
                await context.SaveChangesAsync();


                return Ok(new ResultViewModel<UsuarioModel>(usuario));
            }
            catch (DbUpdateException error)
            {
                return StatusCode(500, new ResultViewModel<UsuarioModel>("05X03 - Não foipossível alterar os dados."));
            }
            catch
            {
                return StatusCode(500, new ResultViewModel<UsuarioModel>("05X04 - Falha interna no servidor"));
            }



        }

        [Authorize]
        [Authorize(Roles = "admin")]
        [HttpDelete("v1/usuarios/{codigo}")]
        public async Task<IActionResult> DeleteAsync(
            [FromRoute] int codigo,
            [FromServices] DataBaseContext context)
        {

            try
            {
                var usuario = await context
                   .Usuario
                   .FirstOrDefaultAsync(x => x.codigo == codigo);

                if (usuario == null)
                    return NotFound(new ResultViewModel<UsuarioModel>("05X03 - Conteúdo não encontrado."));

                context.Usuario.Remove(usuario);
                await context.SaveChangesAsync();


                return Ok(new ResultViewModel<UsuarioModel>(usuario));
            }
            catch (DbUpdateException error)
            {
                return StatusCode(500, new ResultViewModel<UsuarioModel>("05X03 - Não foipossível excluir os dados."));
            }
            catch
            {
                return StatusCode(500, new ResultViewModel<UsuarioModel>("05X05 - Falha interna no servidor"));
            }



        }
    }
}

