using System;
using System.Net;
using System.Web.Http;
using refactor_this.Models;

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
            product?.Save();
            return Ok(Product.Load(product.Id));
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
                orig.Save();
            return Ok();
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

        [Route("{productId}/options")]
        [HttpGet]
        public IHttpActionResult GetOptions(Guid productId)
        {
            return Ok(new ProductOptions(productId));
        }

        [Route("{productId}/options/{id}")]
        [HttpGet]
        public IHttpActionResult GetOption(Guid productId, Guid id)
        {
            if (Product.Load(productId) == null) return NotFound();
            var option = ProductOption.Load(id);
            if (option == null)
                return NotFound();

            return Ok(option);
        }

        [Route("{productId}/options")]
        [HttpPost]
        public IHttpActionResult CreateOption(Guid productId, ProductOption option)
        {
            if (Product.Load(productId) == null) return NotFound();
            option.ProductId = productId;
            option.Save();
            return Ok(ProductOption.Load(option.Id));
        }

        [Route("{productId}/options/{id}")]
        [HttpPut]
        public IHttpActionResult UpdateOption(Guid id, ProductOption option)
        {
            var orig = ProductOption.Load(id);
            if (orig == null) return NotFound();
            orig.Name = option.Name;
            orig.Description = option.Description;

            if (!orig.IsNew)
                orig.Save();
            return Ok(ProductOption.Load(option.Id));
        }

        [Route("{productId}/options/{id}")]
        [HttpDelete]
        public IHttpActionResult DeleteOption(Guid id)
        {
            var itemToBeDeleted = ProductOption.Load(id);
            if (itemToBeDeleted == null)
                return NotFound();
            itemToBeDeleted.Delete();
            return Ok(itemToBeDeleted);
        }
    }
}
