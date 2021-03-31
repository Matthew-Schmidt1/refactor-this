using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;

namespace refactor_this.Models
{
    public class ProductOptions
    {
        public List<ProductOption> Items { get; private set; }

        public ProductOptions(Guid productId)
        {
            LoadProductOptions(productId.ToString());
        }

        private void LoadProductOptions(string productId)
        {
            using (var conn = Helpers.DatabaseConnection)
            {
                Items = conn.Query<ProductOption>("select * from productoption where productid = @productid", new { productid = productId }).ToList();
            }
        }
    }
}