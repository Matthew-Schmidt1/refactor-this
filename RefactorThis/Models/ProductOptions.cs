using System;
using System.Collections.Generic;
using Dapper;
using System.Linq;

namespace refactor_this.Models
{
    public class ProductOptions
    {
        public List<ProductOption> Items { get; private set; }

        public ProductOptions()
        {
            LoadProductOptions(null);
        }

        public ProductOptions(Guid productId)
        {
            LoadProductOptions(productId.ToString());
        }

        private void LoadProductOptions(string where)
        {
            using (var conn = Helpers.NewConnection())
            {
                if (where == null)
                {
                    Items = conn.Query<ProductOption>("select id from productoption").ToList();
                }
                else
                {
                    Items = conn.Query<ProductOption>("select id from productoption where productid = @productid", new { productid = where }).ToList();
                }
            }
        }
    }

}