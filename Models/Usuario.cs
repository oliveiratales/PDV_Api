using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PDV_Api.Models
{
    public class Usuario
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Email { get; set; }
        public string Senha { get; set; }
    }
}