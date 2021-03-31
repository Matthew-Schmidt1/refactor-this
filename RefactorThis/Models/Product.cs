using Dapper;
using Newtonsoft.Json;
using Serilog;
using System;
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
        public bool IsNew { get; private set; }
        
        private Product()
        {
            Id = Guid.NewGuid();
            IsNew = true;
        }

        internal static Product Load(Guid id)
        {
            using (var conn = Helpers.DatabaseConnection)
            {
                var result = conn.Query<Product>("select * from product Where id like @id", new { id = $"%{id}%" }).FirstOrDefault();
                if (result == null) return null;

                //As we are loading this we know it is not new.
                result.IsNew = false;

                Log.ForContext("Product", result).Verbose(nameof(Load));
                return result;
            }
            
        }
        /// <summary>
        /// Saves this product to the database
        /// </summary>
        internal bool Save()
        {
            Log.ForContext("Product", this).Verbose(nameof(Save));
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
                //checking the number or rows changed is equal to 1.
                //So we can confirm there has been a change to the database.
                return conn.Execute(query, new { id = Id, name = Name, description = Description, price = Price, deliveryPrice = DeliveryPrice }) == 1;
            }
        }
        /// <summary>
        /// Updates this Product to have the same information as the supplied.
        /// </summary>
        /// <param name="option">The Supplied Product.</param>
        /// <returns></returns>
        internal bool Update(Product product)
        {
            Log.ForContext("Product", this).Verbose(nameof(Update));
            Name = product.Name;
            Description = product.Description;
            Price = product.Price;
            DeliveryPrice = product.DeliveryPrice;
            return Save();
        }
        /// <summary>
        /// Deletes this Product from the database.
        /// </summary>
        internal bool Delete()
        {
            Log.ForContext("Product", this).Verbose(nameof(Delete));
            foreach (var option in new ProductOptions(Id).Items)
                option.Delete();

            using (var conn = Helpers.DatabaseConnection)
            {
                //checking the number or rows changed is equal to 1.
                //So we can confirm there has been a change to the database.
                return conn.Execute("delete from product where id = @id", new { id = Id }) == 1;
            }
        }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }

    }

}