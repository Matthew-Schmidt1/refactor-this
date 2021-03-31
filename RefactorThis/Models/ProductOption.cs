using Dapper;
using Newtonsoft.Json;
using Serilog;
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

        /// <summary>
        /// Loads a already saved product option from the database.
        /// </summary>
        /// <param name="id">Id of the Product Option </param>
        /// <returns></returns>
        internal static ProductOption Load(Guid id)
        {
           
            using (var conn = Helpers.DatabaseConnection)
            {
                var result = conn.Query<ProductOption>("Select * from productoption where id = @id", new { id }).FirstOrDefault();
                if (result == null) return null;

                //As we are loading this we know it is not new.
                result.IsNew = false;
                Log.ForContext("ProductOption", result).Verbose(nameof(Load));
                return result;
            }
        }

        /// <summary>
        /// Saves this product option to the database
        /// </summary>
        internal bool Save()
        {
            Log.ForContext("ProductOption", this).Verbose(nameof(Save));
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
                //checking the number or rows changed is equal to 1.
                //So we can confirm there has been a change to the database.
                return conn.Execute(query, Paramters) == 1;
            }
        }

        /// <summary>
        /// Updates this Product option to have the same information as the supplied..
        /// </summary>
        /// <param name="option">The Supplied Product option.</param>
        /// <returns></returns>
        internal bool Update(ProductOption option)
        {
            Log.ForContext("ProductOption", this).Verbose(nameof(Update));
            Name = option.Name;
            Description = option.Description;
            return Save();
        }

        /// <summary>
        /// Deletes this Product option from the database
        /// </summary>
        internal bool Delete()
        {
            Log.ForContext("ProductOption", this).Verbose(nameof(Delete));
            using (var conn = Helpers.DatabaseConnection)
            {
                //checking the number or rows changed is equal to 1.
                //So we can confirm there has been a change to the database.
                return conn.Execute("delete from productoption where id = @id", new { id = Id }) == 1;
            }
        }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}