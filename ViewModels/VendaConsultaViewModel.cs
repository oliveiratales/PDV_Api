using System;
using System.Collections.Generic;

namespace PDV_Api.ViewModels
{
    public class VendaConsultaViewModel
    {
        public int IdVenda { get; set; }
        public DateTime Data { get; set; }
        public decimal Desconto { get; set; }
        public string FormaPagamento { get; set; }
        public List<string> Produtos { get; set; }
    }
}