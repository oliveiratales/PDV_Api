using System;
using System.Collections.Generic;

namespace PDV_Api.Models.DTOs
{
    public class VendaInputDTO
    {
        public DateTime Data { get; set; }
        public decimal Desconto { get; set; }
        public string FormaPagamento { get; set; }
        public List<string> Produtos { get; set; }
        public int? ClienteId { get; set; }
    }
}
