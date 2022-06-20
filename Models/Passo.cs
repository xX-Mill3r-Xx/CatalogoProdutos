using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CatalogoProdutos.Models
{
    public class Passo
    {
        public int Id { get; set; }
        public string Descricao { get; set; }
        public int ProdutoId { get; set; }
        public Produto Produto { get; set; }
    }
}
