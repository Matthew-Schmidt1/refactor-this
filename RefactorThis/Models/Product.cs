using System;
using Newtonsoft.Json;
using Dapper;
using System.Threading.Tasks;
using System.Linq;

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
        public bool IsNew { get; } = false;

        public Product()
        {
            Id = Guid.NewGuid();
            IsNew = true;
        }

        public static Product Load(Guid id)
        {
            using (var conn = Helpers.NewConnection())
            {
                return conn.Query<Product>("select id from product Where id like @id", new { id = $"%{id}%" }).FirstOrDefault();
            }
        }

        public async Task SaveAsync()
        {
            using (var conn = Helpers.NewConnection())
            {
                if (IsNew)
                {
                    await conn.ExecuteAsync("insert into product (id, name, description, price, deliveryprice) values (@id, @name, @description, @price, @deliveryPrice )",
                    new { id = Id, name = Name, description = Description, price = Price, DeliveryPrice = DeliveryPrice }).ConfigureAwait(false);
                }
                else
                {
                    await conn.ExecuteAsync("update product set name = @Name, description =  @Description, , price = @Price, deliveryprice = @DeliveryPrice where id = @id )",
                    new { id = Id, name = Name, description = Description, price = Price, DeliveryPrice = DeliveryPrice }).ConfigureAwait(false);
                }
            }
        }

        public void Save()
        {
            try
            {
                using (var conn = Helpers.NewConnection())
                {
                    if (IsNew)
                    {
                        conn.Execute("insert into product (id, name, description, price, deliveryprice) values (@id, @name, @description, @price, @deliveryPrice )",
                        new { id = Id, name = Name, description = Description, price = Price, deliveryPrice = DeliveryPrice });
                    }
                    else
                    {
                        conn.Execute("update product set name = @Name, description =  @Description, price = @Price, deliveryprice = @DeliveryPrice where id = @id )",
                        new { id = Id, name = Name, description = Description, price = Price, DeliveryPrice = DeliveryPrice });
                    }
                }
            }catch(Exception ex)
            {
                
            }
        }

        public async Task DeleteAsync()
        {
            foreach (var option in new ProductOptions(Id).Items)
                option.Delete();

            using (var conn = Helpers.NewConnection())
            {
                await conn.ExecuteAsync("delete from product where id = @id", new { id = Id }).ConfigureAwait(false);
            }
        }

        public void Delete()
        {
            foreach (var option in new ProductOptions(Id).Items)
                option.Delete();

            using (var conn = Helpers.NewConnection())
            {
                conn.Execute("delete from product where id = @id", new { id = Id });
            }
        }
    }

}