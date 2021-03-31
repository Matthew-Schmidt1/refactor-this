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

        private void LoadProductOptions(string where)
        {
            using (var conn = Helpers.DatabaseConnection)
            {
                if (where == null)
                {
                    Items = conn.Query<ProductOption>("select * from productoption").ToList();
                }
                else
                {
                    Items = conn.Query<ProductOption>("select * from productoption where productid = @productid", new { productid = where }).ToList();
                }
            }
        }
    }
}