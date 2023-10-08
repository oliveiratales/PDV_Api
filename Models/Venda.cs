using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PDV_Api.Models
{
    public class Venda
    {
        [Key]
        public int IdVenda { get; set; }
        
        public DateTime Data { get; set; }
        public decimal Desconto { get; set; }
        public int? ClienteId { get; set; }
        public Cliente Cliente { get; set; }
        public FormaPagamento FormaPagamento { get; set; }
        public List<Produto> Produtos { get; set; }
    }

    public enum FormaPagamento
    {
        Dinheiro,
        Debito,
        Credito,
        Pix,
        TransferenciaBancaria
    }
}
