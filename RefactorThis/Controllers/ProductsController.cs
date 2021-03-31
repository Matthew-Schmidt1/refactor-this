using refactor_this.Models;
using System;
using System.Net;
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
            if (product == null) return NotFound();
            return Ok(product);
        }

        [Route]
        [HttpPost]
        public IHttpActionResult Create(Product product)
        {
            if (product == null) return BadRequest();
            if (product.Save())
            {
                return Ok(product);
            }
            else
            {
                return FailedToSave();
            }
        }

        [Route("{id}")]
        [HttpPut]
        public IHttpActionResult Update(Guid id, Product product)
        {
            var orig = Product.Load(id);
            if (orig == null) return NotFound();
            if (orig.Update(product))
            {
                return Ok(orig);
            }
            else
            {
                return FailedToSave();
            }
        }

        [Route("{id}")]
        [HttpDelete]
        public IHttpActionResult Delete(Guid id)
        {
            var product = Product.Load(id);
            if (product == null) return NotFound();
            if (product.Delete())
            {
                return Ok(product);
            }
            else
            {
                return FailedToSave();
            }
        }

        private IHttpActionResult FailedToSave()
        {
            return Content(HttpStatusCode.BadRequest, "Failed to Save the change!");
        }
    }
}