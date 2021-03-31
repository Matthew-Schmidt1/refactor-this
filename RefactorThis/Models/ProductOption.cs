using Dapper;
using Newtonsoft.Json;
using System;
using System.Linq;

namespace refactor_this.Models
{
    public class ProductOption
    {
        public Guid Id { get; }

        public Guid ProductId { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        [JsonIgnore]
        public bool IsNew { get; private set; }

        public ProductOption()
        {
            Id = Guid.NewGuid();
            IsNew = true;
        }

        public static ProductOption Load(Guid id)
        {
            using (var conn = Helpers.DatabaseConnection)
            {
                var result = conn.Query<ProductOption>("Select * from productoption where id = @id", new { id }).FirstOrDefault();
                if (result == null) return null;
                result.IsNew = false;
                return result;
            }
        }

        public void Save()
        {
            using (var conn = Helpers.DatabaseConnection)
            {
                var Paramters = new { id = Id, productId = ProductId, name = Name, description = Description };
                string query;
                if (IsNew)
                {
                    query = "insert into productoption (id, productid, name, description) values (@Id, @ProductId, @Name, @Description)";
                }
                else
                {
                    query = "update productoption set name = @Name, description = @Description where id = @Id";
                }
                conn.Execute(query, Paramters);
            }
        }

        public void Delete()
        {
            using (var conn = Helpers.DatabaseConnection)
            {
                conn.Execute("delete from productoption where id = @id", new { id = Id });
            }
        }
    }
}