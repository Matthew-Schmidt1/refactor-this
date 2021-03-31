using refactor_this.Models;
using System;
using System.Web.Http;

namespace refactor_this.Controllers
{
    [RoutePrefix("products")]
    public class ProductsController : ApiController
    {
        [Route()]
        [HttpGet]
        public IHttpActionResult GetAll()
        {
            return Ok(new Products());
        }

        [Route]
        [HttpGet]
        public IHttpActionResult SearchByName(string name)
        {
            return Ok(new Products(name));
        }

        [Route("{id}")]
        [HttpGet]
        public IHttpActionResult GetProduct(Guid id)
        {
            var product = Product.Load(id);
            if (product == null)
                return NotFound();

            return Ok(product);
        }

        [Route]
        [HttpPost]
        public IHttpActionResult Create(Product product)
        {
            if (product == null) return BadRequest();
            product.Save();
            return Ok(product);
        }

        [Route("{id}")]
        [HttpPut]
        public IHttpActionResult Update(Guid id, Product product)
        {
            var orig = Product.Load(id);
            if (orig == null) return NotFound();

            orig.Name = product.Name;
            orig.Description = product.Description;
            orig.Price = product.Price;
            orig.DeliveryPrice = product.DeliveryPrice;

            if (!orig.IsNew)
            {
                orig.Save();
                return Ok(orig);
            }
            else { return BadRequest(); }
        }

        [Route("{id}")]
        [HttpDelete]
        public IHttpActionResult Delete(Guid id)
        {
            var product = Product.Load(id);
            if (product == null) return NotFound();
            product.Delete();
            return Ok(product);
        }

    
    }
}