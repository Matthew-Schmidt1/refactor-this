using System;
using Newtonsoft.Json;
using Dapper;
using System.Threading.Tasks;
using System.Linq;

namespace refactor_this.Models
{
    public class ProductOption
    {
        public Guid Id { get; set; }

        public Guid ProductId { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        [JsonIgnore]
        public bool IsNew { get; } = false;

        public ProductOption()
        {
            Id = Guid.NewGuid();
            IsNew = true;
        }

        public static ProductOption Load(Guid id)
        {
            using (var conn = Helpers.NewConnection())
            {
                return conn.Query<ProductOption>("Select * from productoption where id = @id", new { id }).FirstOrDefault();
            }
        }
        public async Task SaveAsync()
        {
            using (var conn = Helpers.NewConnection())
            {
                if (IsNew)
                {
                    await conn.ExecuteAsync("insert into productoption (id, productid, name, description) values (@Id, @ProductId, @Name, @Description)", new { id = Id, productId = ProductId, name = Name, description = Description }).ConfigureAwait(false);
                }
                else
                {
                    await conn.ExecuteAsync("update productoption set name = @Name, description = @Description where id = @Id", new { id = Id, productId = ProductId, name = Name, description = Description }).ConfigureAwait(false);
                }
            }
        }

        public void Save()
        {
            using (var conn = Helpers.NewConnection())
            {
                var Paramters = new { id = Id, productId = ProductId, name = Name, description = Description };

                if (IsNew)
                {
                    conn.ExecuteAsync("insert into productoption (id, productid, name, description) values (@Id, @ProductId, @Name, @Description)", Paramters);
                }
                else
                {
                    conn.ExecuteAsync("update productoption set name = @Name, description = @Description where id = @Id", Paramters);
                }
            }
        }

        public void Delete()
        {
            using (var conn = Helpers.NewConnection())
            {
                conn.Execute("delete from productoption where id = @id", new { id = Id });
            }
        }

        public async Task DeleteAsync()
        {
            using (var conn = Helpers.NewConnection())
            {
                await conn.ExecuteAsync("delete from productoption where id = @id", new { id = Id }).ConfigureAwait(false);
            }
        }

    }

}