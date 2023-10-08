using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PDV_Api.Models
{
    public class Cliente
    {
        [Key]
        public int ClienteId { get; set; }

        [Required]
        public string Nome { get; set; }
        public string Email { get; set; }

        public string Telefone { get; set; }

        public List<Produto> Produtos { get; set; }
    }
}
