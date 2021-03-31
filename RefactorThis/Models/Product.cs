using Dapper;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace refactor_this.Models
{
    public class Product
    {
        public Guid Id { get; }

        public string Name { get; set; }

        public string Description { get; set; }

        public decimal Price { get; set; }

        public decimal DeliveryPrice { get; set; }

        [JsonIgnore]
        public bool IsNew { get; private set; }

        private Product()
        {
            Id = Guid.NewGuid();
            IsNew = true;
        }

        public static Product Load(Guid id)
        {
            using (var conn = Helpers.DatabaseConnection)
            {
                var result = conn.Query<Product>("select * from product Where id like @id", new { id = $"%{id}%" }).FirstOrDefault();
                if (result == null) return null;
                result.IsNew = false;
                return result;
            }
        }

        public void Save()
        {
            using (var conn = Helpers.DatabaseConnection)
            {
                string query;
                if (IsNew)
                {
                    query = "insert into product (id, name, description, price, deliveryprice) values (@id, @name, @description, @price, @deliveryPrice)";
                }
                else
                {
                    query = "update product set name = @Name, description =  @Description, price = @Price, deliveryprice = @DeliveryPrice where id = @id ";
                }
                conn.Execute(query, new { id = Id, name = Name, description = Description, price = Price, deliveryPrice = DeliveryPrice });
            }
        }

        public async Task DeleteAsync()
        {
            foreach (var option in new ProductOptions(Id).Items)
                option.Delete();

            using (var conn = Helpers.DatabaseConnection)
            {
                await conn.ExecuteAsync("delete from product where id = @id", new { id = Id }).ConfigureAwait(false);
            }
        }

        public void Delete()
        {
            foreach (var option in new ProductOptions(Id).Items)
                option.Delete();

            using (var conn = Helpers.DatabaseConnection)
            {
                conn.Execute("delete from product where id = @id", new { id = Id });
            }
        }
    }
}