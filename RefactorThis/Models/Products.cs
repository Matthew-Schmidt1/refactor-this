using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Newtonsoft.Json;
using Dapper;
using System.Linq;
using System.Threading.Tasks;

namespace refactor_this.Models
{
    public class Products
    {
        public List<Product> Items { get; private set; }

        public Products()
        {
            LoadProducts(string.Empty);
        }

        public Products(string name)
        {
            LoadProducts(name);
        }

        private void LoadProducts(string name)
        {
            Items = new List<Product>();

            using var conn = Helpers.NewConnection();

            if (name == null)
            {
                Items = conn.Query<Product>("select id from product").ToList();
            }
            else
            {
                Items = conn.Query<Product>("select id from product Where name like @name", new { name = $"%{name}%" }).ToList();
            }

        }
    }

    public class Product
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public decimal Price { get; set; }

        public decimal DeliveryPrice { get; set; }

        [JsonIgnore]
        public bool IsNew { get; }

        public Product()
        {
            Id = Guid.NewGuid();
            IsNew = true;
        }

        public async Task SaveAsync()
        {
            using var conn = Helpers.NewConnection();
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

        public void Save()
        {
            using var conn = Helpers.NewConnection();
            if (IsNew)
            {
                conn.Execute("insert into product (id, name, description, price, deliveryprice) values (@id, @name, @description, @price, @deliveryPrice )",
                new { id = Id, name = Name, description = Description, price = Price, DeliveryPrice = DeliveryPrice });
            }
            else
            {
                conn.Execute("update product set name = @Name, description =  @Description, , price = @Price, deliveryprice = @DeliveryPrice where id = @id )",
                new { id = Id, name = Name, description = Description, price = Price, DeliveryPrice = DeliveryPrice });
            }
        }

        public async Task DeleteAsync()
        {
            foreach (var option in new ProductOptions(Id).Items)
                option.Delete();

            using var conn = Helpers.NewConnection();
          
            await conn.ExecuteAsync("delete from product where id = @id",new { id = Id}).ConfigureAwait(false);
        }

        public void Delete()
        {
            foreach (var option in new ProductOptions(Id).Items)
                option.Delete();

            using var conn = Helpers.NewConnection();

            conn.Execute("delete from product where id = @id", new { id = Id });
        }
    }

    public class ProductOptions
    {
        public List<ProductOption> Items { get; private set; }

        public ProductOptions()
        {
            LoadProductOptions(null);
        }

        public ProductOptions(Guid productId)
        {
            LoadProductOptions($"where productid = '{productId}'");
        }

        private void LoadProductOptions(string where)
        {
            Items = new List<ProductOption>();
            using var conn = Helpers.NewConnection();
            var cmd = new SqlCommand($"select id from productoption {where}", conn);
            conn.Open();

            var rdr = cmd.ExecuteReader();
            while (rdr.Read())
            {
                var id = Guid.Parse(rdr["id"].ToString());
                Items.Add(new ProductOption(id));
            }
        }
    }

    public class ProductOption
    {
        public Guid Id { get; set; }

        public Guid ProductId { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        [JsonIgnore]
        public bool IsNew { get; }

        public ProductOption()
        {
            Id = Guid.NewGuid();
            IsNew = true;
        }

        public ProductOption(Guid id)
        {
            IsNew = true;
            using var conn = Helpers.NewConnection();
            var cmd = new SqlCommand($"select * from productoption where id = '{id}'", conn);
            conn.Open();

            var rdr = cmd.ExecuteReader();
            if (!rdr.Read())
                return;

            IsNew = false;
            Id = Guid.Parse(rdr["Id"].ToString());
            ProductId = Guid.Parse(rdr["ProductId"].ToString());
            Name = rdr["Name"].ToString();
            Description = (DBNull.Value == rdr["Description"]) ? null : rdr["Description"].ToString();
        }

        public void Save()
        {
            using var conn = Helpers.NewConnection();
            var cmd = IsNew ?
                new SqlCommand($"insert into productoption (id, productid, name, description) values ('{Id}', '{ProductId}', '{Name}', '{Description}')", conn) :
                new SqlCommand($"update productoption set name = '{Name}', description = '{Description}' where id = '{Id}'", conn);

            conn.Open();
            cmd.ExecuteNonQuery();
        }

        public void Delete()
        {
            using var conn = Helpers.NewConnection();
            conn.Open();
            var cmd = new SqlCommand($"delete from productoption where id = '{Id}'", conn);
            cmd.ExecuteReader();
        }
    }
}