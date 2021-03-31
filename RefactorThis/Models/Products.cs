using Dapper;
using Newtonsoft.Json;
using Serilog;
using System.Collections.Generic;
using System.Linq;

namespace refactor_this.Models
{
    public class Products
    {
        public List<Product> Items { get; private set; }

        public Products()
        {
            LoadAllProducts();
        }

        public Products(string name)
        {
            LoadProducts(name);
        }

        private void LoadProducts(string name)
        {
            Log.ForContext("Products", this).Verbose(nameof(LoadProducts));
            Items = new List<Product>();

            using (var conn = Helpers.DatabaseConnection)
            {
                Items = conn.Query<Product>("select * from product Where name like @name", new { name = $"%{name}%" }).ToList();
            }
        }

        private void LoadAllProducts()
        {
            Log.ForContext("Products", this).Verbose(nameof(LoadAllProducts));
            Items = new List<Product>();

            using (var conn = Helpers.DatabaseConnection)
            {
                Items = conn.Query<Product>("select * from product").ToList();
            }
        }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}