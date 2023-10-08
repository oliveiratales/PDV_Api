using System;
using System.ComponentModel.DataAnnotations;

namespace PDV_Api.Models
{
    public class Produto
    {
        [Key]
        [RegularExpression(@"^[A-Z]\d+$", ErrorMessage = "O formato da referência deve ser uma letra seguida por números (por exemplo, C001).")]
        public string Referencia { get; set; }

        public string Nome { get; set; }
        public string Cor { get; set; }
        public string Tamanho { get; set; }
        public string Tipo { get; set; }
        public int Quantidade { get; set; }
        public decimal PrecoPago { get; set; }
        public decimal PrecoVenda { get; set; }
        public OrigemProduto Origem { get; set; }
        public int? ClienteId { get; set; }
        public Cliente Cliente { get; set; }
    }

    public enum OrigemProduto
    {
        Brecho,
        Consignado,
        Doacao
    }
}
