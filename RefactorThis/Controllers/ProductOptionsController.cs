using refactor_this.Models;
using Serilog;
using System;
using System.Net;
using System.Web.Http;

namespace refactor_me.Controllers
{
    [RoutePrefix("products")]
    public class ProductOptionsController : ApiController
    {
        [Route("{productId}/options")]
        [HttpGet]
        public IHttpActionResult GetOptions(Guid productId)
        {
            var logger = Log.ForContext("productId", productId);
            logger.Verbose(nameof(GetOptions));
            return Ok(new ProductOptions(productId));
        }

        [Route("{productId}/options/{id}")]
        [HttpGet]
        public IHttpActionResult GetOption(Guid productId, Guid id)
        {
            var logger = Log.ForContext("productId", productId).ForContext("id", id);
            logger.Verbose(nameof(GetOption));
            if (Product.Load(productId) == null) return NotFound();
            var option = ProductOption.Load(id);
            if (option == null) return NotFound();
            return Ok(option);
        }

        [Route("{productId}/options")]
        [HttpPost]
        public IHttpActionResult CreateOption(Guid productId, ProductOption option)
        {
            var logger = Log.ForContext("productId", productId).ForContext("ProductOption", option);
            logger.Verbose(nameof(CreateOption));
            if (Product.Load(productId) == null) return NotFound();
            option.ProductId = productId;

            if (option.Save())
            {
                return Ok(option);
            }
            else
            {
                logger.Error($"Error in Creation something wrong with the number of elements saved {nameof(CreateOption)}");
                return FailedToSave();
            }
        }

        [Route("{productId}/options/{id}")]
        [HttpPut]
        public IHttpActionResult UpdateOption(Guid productId, Guid id, ProductOption option)
        {
            var logger = Log.ForContext("productId",productId).ForContext("id", id).ForContext("ProductOption",option);
            logger.Verbose(nameof(UpdateOption));
            if (Product.Load(productId) == null) return NotFound();
            var orig = ProductOption.Load(id);
            if (orig == null) return NotFound();

            if (orig.Update(option))
            {
                return Ok(orig);
            }
            else
            {
                logger.Error($"Error in Creation something wrong with the number of elements saved {nameof(UpdateOption)}");
                return FailedToSave();
            }
        }

        [Route("{productId}/options/{id}")]
        [HttpDelete]
        public IHttpActionResult DeleteOption(Guid productId, Guid id)
        {
            var logger = Log.ForContext("id", id);
            logger.Verbose(nameof(DeleteOption));
            if (Product.Load(productId) == null) return NotFound();
            var itemToBeDeleted = ProductOption.Load(id);
            if (itemToBeDeleted == null) return NotFound();

            if (itemToBeDeleted.Delete())
            {
                return Ok(itemToBeDeleted);
            }
            else
            {
                logger.Error($"Error in Creation something wrong with the number of elements saved {nameof(DeleteOption)}");
                return FailedToSave();
            }
        }

        private IHttpActionResult FailedToSave()
        {
            return Content(HttpStatusCode.BadRequest, "Failed to Save the change!");
        }
    }
}