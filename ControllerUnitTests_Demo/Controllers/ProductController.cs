using ControllerUnitTests_Demo.Repository;
using System;
using System.Linq;
using System.Net;
using System.Web.Http;

namespace ControllerUnitTests_Demo.Controllers
{


    [RoutePrefix("api/products")]
    public class ProductController : ApiController
    {
        private readonly IProductData productRepo;

        public ProductController(IProductData _repo)
        {
            this.productRepo = _repo;
        }

        [HttpGet]
        [Route("AllProducts")]
        public IHttpActionResult GetAllProducts()
        {
            try
            {
                var products = productRepo.GetAllProducts();
                if (products.Any())
                {
                    return Ok(products);
                }
                else
                {
                    return Content(HttpStatusCode.NoContent, "No Products Found");
                }
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [HttpGet]
        [Route("{id}")]
        public IHttpActionResult GetProductById(int id)
        {
            try
            {
                var product = productRepo.GetProductById(id);
                if (product != null)
                {
                    return Ok(product);
                }
                else
                {
                    return Content(HttpStatusCode.NotFound, "No Product as found with that Id");
                }
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [HttpPost]
        [Route("Product")]
        public IHttpActionResult AddProduct([FromBody] Product newProduct)
        {
            try
            {
                if(newProduct != null)
                {
                    var addedProduct = productRepo.AddProduct(newProduct);
                    return Ok(addedProduct);
                }
                else
                {
                    return BadRequest("No valid product data was submitted in request");
                }
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

    }
}