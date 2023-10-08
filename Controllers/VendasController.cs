using Microsoft.AspNetCore.Mvc;
using PDV_Api.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using PDV_Api.Models.DTOs;
using PDV_Api.ViewModels;

namespace PDV_Api.Controllers
{
    [Route("api/venda")]
    [ApiController]
    public class VendasController : ControllerBase
    {
        private readonly AppDbContext _context;

        public VendasController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet("vervendas")]
        public ActionResult<IEnumerable<VendaConsultaViewModel>> GetVendas()
        {
            var vendas = _context.Vendas
                .Include(v => v.Produtos)
                .ToList();

            var vendasViewModel = vendas.Select(v => new VendaConsultaViewModel
            {
                IdVenda = v.IdVenda,
                Data = v.Data,
                Desconto = v.Desconto,
                FormaPagamento = v.FormaPagamento.ToString(),
                Produtos = v.Produtos.Select(p => p.Referencia).ToList()
            }).ToList();

            return vendasViewModel;
        }

        [HttpGet("vervenda/{idVenda}")]
        public ActionResult<VendaConsultaViewModel> GetVenda(int idVenda)
        {
            var venda = _context.Vendas
                .Include(v => v.Produtos)
                .FirstOrDefault(v => v.IdVenda == idVenda);

            if (venda == null)
            {
                return NotFound();
            }

            var vendaViewModel = new VendaConsultaViewModel
            {
                IdVenda = venda.IdVenda,
                Data = venda.Data,
                Desconto = venda.Desconto,
                FormaPagamento = venda.FormaPagamento.ToString(),
                Produtos = venda.Produtos.Select(p => p.Referencia).ToList()
            };

            return vendaViewModel;
        }

        [HttpPost("cadastrarvenda")]
        public IActionResult PostVenda(VendaInputDTO vendaInputDTO)
        {
            // Validação: Verifica se a lista de produtos está vazia
            if (vendaInputDTO.Produtos == null || vendaInputDTO.Produtos.Count == 0)
            {
                return BadRequest("A lista de produtos não pode estar vazia.");
            }

            // Validação: Verifica se todos os produtos existem no banco de dados
            var produtosExistentes = _context.Produtos.Select(p => p.Referencia).ToList();
            foreach (var referencia in vendaInputDTO.Produtos)
            {
                if (!produtosExistentes.Contains(referencia))
                {
                    return BadRequest($"Produto com referência '{referencia}' não encontrado.");
                }
            }

            // Conversão da forma de pagamento para enum
            if (!Enum.TryParse(vendaInputDTO.FormaPagamento, true, out FormaPagamento formaPagamento))
            {
                return BadRequest("Forma de pagamento inválida.");
            }

            var venda = new Venda
            {
                Data = vendaInputDTO.Data,
                Desconto = vendaInputDTO.Desconto,
                FormaPagamento = formaPagamento
            };

            foreach (var referencia in vendaInputDTO.Produtos)
            {
                var produto = _context.Produtos.FirstOrDefault(p => p.Referencia == referencia);
                if (produto != null)
                {
                    venda.Produtos.Add(produto);
                }
            }

            _context.Vendas.Add(venda);
            _context.SaveChanges();

            return CreatedAtAction(nameof(GetVenda), new { idVenda = venda.IdVenda }, venda);
        }

        // Atualização da venda usando VendaInputDTO no PUT
        [HttpPut("atualizarvenda/{idVenda}")]
        public IActionResult PutVenda(int idVenda, VendaInputDTO vendaInputDTO)
        {
            // Validação: Verifica se a venda com o ID especificado existe
            var vendaExistente = _context.Vendas
                .Include(v => v.Produtos)
                .FirstOrDefault(v => v.IdVenda == idVenda);

            if (vendaExistente == null)
            {
                return NotFound("Venda não encontrada.");
            }

            // Validação: Verifica se a lista de produtos está vazia
            if (vendaInputDTO.Produtos == null || vendaInputDTO.Produtos.Count == 0)
            {
                return BadRequest("A lista de produtos não pode estar vazia.");
            }

            // Validação: Verifica se todos os produtos existem no banco de dados
            var produtosExistentes = _context.Produtos.Select(p => p.Referencia).ToList();
            foreach (var referencia in vendaInputDTO.Produtos)
            {
                if (!produtosExistentes.Contains(referencia))
                {
                    return BadRequest($"Produto com referência '{referencia}' não encontrado.");
                }
            }

            // Conversão da forma de pagamento para enum
            if (!Enum.TryParse(vendaInputDTO.FormaPagamento, true, out FormaPagamento formaPagamento))
            {
                return BadRequest("Forma de pagamento inválida.");
            }

            // Atualiza os campos da venda
            vendaExistente.Data = vendaInputDTO.Data;
            vendaExistente.Desconto = vendaInputDTO.Desconto;
            vendaExistente.FormaPagamento = formaPagamento;

            // Associa os produtos à venda
            vendaExistente.Produtos.Clear();
            foreach (var referencia in vendaInputDTO.Produtos)
            {
                var produto = _context.Produtos.FirstOrDefault(p => p.Referencia == referencia);
                if (produto != null)
                {
                    vendaExistente.Produtos.Add(produto);
                }
            }

            _context.SaveChanges();

            return NoContent();
        }

        [HttpDelete("excluirvenda/{idVenda}")]
        public IActionResult DeleteVenda(int idVenda)
        {
            var venda = _context.Vendas.Find(idVenda);

            if (venda == null)
            {
                return NotFound("Venda não encontrada.");
            }

            _context.Vendas.Remove(venda);
            _context.SaveChanges();

            return NoContent();
        }
    }
}
