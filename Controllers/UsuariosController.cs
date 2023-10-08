using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PDV_Api.Models;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace PDV_Api.Controllers
{
    [Route("api/usuarios")]
    [ApiController]
    public class UsuariosController : ControllerBase
    {
        private readonly AppDbContext _context;

        public UsuariosController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet("verusuarios")]
        public async Task<IActionResult> ListarUsuarios()
        {
            var usuarios = await _context.Usuarios.ToListAsync();
            return Ok(usuarios);
        }

        [HttpGet("verusuario/{id}")]
        public async Task<IActionResult> ObterUsuario(int id)
        {
            var usuario = await _context.Usuarios.FindAsync(id);

            if (usuario == null)
            {
                return NotFound();
            }

            return Ok(usuario);
        }

        [HttpPost("registrar")]
        public async Task<IActionResult> RegistrarUsuario(Usuario usuario)
        {
            try
            {
                // Verifique se o email já está em uso.
                if (await _context.Usuarios.AnyAsync(u => u.Email == usuario.Email))
                {
                    return BadRequest(new { message = "O email já está em uso." });
                }

                // Se o email não está em uso, adicione o novo usuário ao banco de dados.
                _context.Usuarios.Add(usuario);
                await _context.SaveChangesAsync();

                // Retorne um status 201 (Created) com os detalhes do usuário registrado.
                return CreatedAtAction(nameof(ObterUsuario), new { id = usuario.Id }, usuario);
            }
            catch
            {
                // Se ocorrer um erro durante o registro, retorne um status 400 (Bad Request) com uma mensagem de erro.
                return BadRequest(new { message = "Ocorreu um erro durante o registro do usuário." });
            }
        }

        [HttpPut("atualizar/{id}")]
        public async Task<IActionResult> AtualizarUsuario(int id, Usuario usuario)
        {
            if (id != usuario.Id)
            {
                return BadRequest();
            }

            _context.Entry(usuario).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UsuarioExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        [HttpDelete("excluir/{id}")]
        public async Task<IActionResult> ExcluirUsuario(int id)
        {
            var usuario = await _context.Usuarios.FindAsync(id);
            if (usuario == null)
            {
                return NotFound();
            }

            _context.Usuarios.Remove(usuario);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool UsuarioExists(int id)
        {
            return _context.Usuarios.Any(e => e.Id == id);
        }
    }
}
