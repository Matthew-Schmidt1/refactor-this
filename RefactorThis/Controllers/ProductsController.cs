using refactor_this.Models;
using Serilog;
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
            Log.Verbose(nameof(GetAll));
            return Ok(new Products());
        }

        [Route]
        [HttpGet]
        public IHttpActionResult SearchByName(string name)
        {
            Log.ForContext("name", name).Verbose(nameof(SearchByName));
            return Ok(new Products(name));
        }

        [Route("{id}")]
        [HttpGet]
        public IHttpActionResult GetProduct(Guid id)
        {
            Log.ForContext("id", id).Verbose(nameof(GetProduct));
            var product = Product.Load(id);
            if (product == null) return NotFound();
            return Ok(product);
        }

        [Route]
        [HttpPost]
        public IHttpActionResult Create(Product product)
        {
            var logger = Log.ForContext("product", product);
            logger.Verbose(nameof(Create));
            if (product == null) return BadRequest();
            if (product.Save())
            {
                return Ok(product);
            }
            else
            {
                logger.Error($"Error in Creation something wrong with the number of elements saved {nameof(Create)}");
                return FailedToSave();
            }
        }

        [Route("{id}")]
        [HttpPut]
        public IHttpActionResult Update(Guid id, Product product)
        {
            var logger = Log.ForContext("product", product).ForContext("id", id);
            logger.Verbose(nameof(Update));
            var orig = Product.Load(id);

            if (orig == null) return NotFound();

            if (orig.Update(product))
            {
                return Ok(orig);
            }
            else
            {
                logger.Error($"Error in Creation something wrong with the number of elements saved {nameof(Update)}");
                return FailedToSave();
            }
        }

        [Route("{id}")]
        [HttpDelete]
        public IHttpActionResult Delete(Guid id)
        {
            var logger = Log.ForContext("id", id);
            logger.Verbose(nameof(Delete));
            var product = Product.Load(id);

            if (product == null) return NotFound();

            if (product.Delete())
            {
                return Ok(product);
            }
            else
            {
                logger.Error($"Error in Creation something wrong with the number of elements saved {nameof(Delete)}");
                return FailedToSave();
            }
        }

        private IHttpActionResult FailedToSave()
        {
            return Content(HttpStatusCode.BadRequest, "Failed to Save the change!");
        }
    }
}