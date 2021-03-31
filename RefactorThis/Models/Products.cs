using System.Collections.Generic;
using System.Data.SqlClient;
using Dapper;
using System.Linq;

namespace refactor_this.Models
{
    public class Products
    {
        public List<Product> Items { get; private set; }

        public Products()
        {
            LoadProducts(null);
        }

        public Products(string name)
        {
            LoadProducts(name);
        }

        private void LoadProducts(string name)
        {
            Items = new List<Product>();

            using (var conn = Helpers.NewConnection())
            {
                if (name == null)
                {
                    Items = conn.Query<Product>("select * from product").ToList();
                }
                else
                {
                    Items = conn.Query<Product>("select * from product Where name like @name", new { name = $"%{name}%" }).ToList();
                }
            }
        }
    }
}