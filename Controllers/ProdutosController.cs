using Microsoft.AspNetCore.Mvc;
using PDV_Api.Models;
using Microsoft.EntityFrameworkCore;

namespace PDV_Api.Controllers
{
    [Route("api/produto")]
    [ApiController]
    public class ProdutosController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ProdutosController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet("verprodutos")]
        public ActionResult<IEnumerable<Produto>> GetProdutos()
        {
            return _context.Produtos.ToList();
        }

        [HttpGet("verproduto/{referencia}")]
        public ActionResult<Produto> GetProduto(string referencia)
        {
            var produto = _context.Produtos.Find(referencia);

            if (produto == null)
            {
                return NotFound();
            }

            return produto;
        }

        [HttpPost("cadastrarproduto")]
        public IActionResult PostProduto(Produto produto)
        {
            _context.Produtos.Add(produto);
            _context.SaveChanges();

            return CreatedAtAction(nameof(GetProduto), new { referencia = produto.Referencia }, produto);
        }

        [HttpPut("atualizarproduto/{referencia}")]
        public IActionResult PutProduto(string referencia, Produto produto)
        {
            if (referencia != produto.Referencia)
            {
                return BadRequest();
            }

            _context.Entry(produto).State = EntityState.Modified;
            _context.SaveChanges();

            return NoContent();
        }

        [HttpDelete("excluirproduto/{referencia}")]
        public IActionResult DeleteProduto(string referencia)
        {
            var produto = _context.Produtos.Find(referencia);

            if (produto == null)
            {
                return NotFound();
            }

            _context.Produtos.Remove(produto);
            _context.SaveChanges();

            return NoContent();
        }
    }
}
