using ControllerUnitTests_Demo;
using ControllerUnitTests_Demo.Controllers;
using ControllerUnitTests_Demo.Repository;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Web.Http;
using System.Web.Http.Results;

namespace UnitTest_Demo_Tests
{
    [TestClass]
    public class ProductController_Tests
    {
        private ProductController controller;
        private Mock<IProductData> repository;
        private List<Product> productList;
        private Product addProduct;



        [TestInitialize]
        public void TestInitalize()
        {
            repository = new Mock<IProductData>();

            productList = new List<Product>();

            controller = new ProductController(repository.Object)
            {
                Request = new HttpRequestMessage(),
                Configuration = new HttpConfiguration()
            };
            DataSetup();

        }

        #region ProductsController GetAllProducts

        [TestMethod]
        public void ProductController_GetAllProducts_Success()
        {
            repository.Setup(pl => pl.GetAllProducts()).Returns(productList.AsQueryable());
            var response = controller.GetAllProducts();

            Assert.IsNotNull(response);
            Assert.IsInstanceOfType(response, typeof(OkNegotiatedContentResult<IQueryable<Product>>));
            Assert.AreEqual(HttpStatusCode.OK, response.ExecuteAsync(new CancellationToken()).Result.StatusCode);


        }

        [TestMethod]
        public void ProductController_GetAllProducts_Exception()
        {
            repository.Setup(pl => pl.GetAllProducts()).Throws<NullReferenceException>();
            var response = controller.GetAllProducts();

            Assert.AreEqual(HttpStatusCode.InternalServerError, response.ExecuteAsync(new CancellationToken()).Result.StatusCode);
        }

        [TestMethod]
        public void ProductController_GetAllProducts_NoContent()
        {
            repository.Setup(pl => pl.GetAllProducts()).Returns(Enumerable.Empty<Product>().AsQueryable());
            var response = controller.GetAllProducts();
            Assert.AreEqual(HttpStatusCode.NoContent, response.ExecuteAsync(new CancellationToken()).Result.StatusCode);

        }

        #endregion

        #region ProductController GetProductsById
        [TestMethod]
        public void ProductController_GetProductById_Success()
        {
            repository.Setup(pl => pl.GetProductById(3)).Returns(productList.AsQueryable().Where(x => x.ProductId == 3).First);
            var response = controller.GetProductById(3);
            Assert.IsNotNull(response);
            Assert.IsInstanceOfType(response, typeof(OkNegotiatedContentResult<Product>));
            Assert.AreEqual(HttpStatusCode.OK, response.ExecuteAsync(new CancellationToken()).Result.StatusCode);
        }

        [TestMethod]
        public void ProductController_GetProductById_NotFound()
        {
            repository.Setup(pl => pl.GetProductById(10)).Returns((Product)null);
            var response = controller.GetProductById(10);

            Assert.IsInstanceOfType(response, typeof(NegotiatedContentResult<string>));
            Assert.AreEqual(HttpStatusCode.NotFound, response.ExecuteAsync(new CancellationToken()).Result.StatusCode);

        }

        [TestMethod]
        public void ProductController_GetProductsById_Exception()
        {
            repository.Setup(pl => pl.GetProductById(10)).Throws<NullReferenceException>();
            var response = controller.GetProductById(10);
            Assert.AreEqual(HttpStatusCode.InternalServerError, response.ExecuteAsync(new CancellationToken()).Result.StatusCode);

        }

        #endregion

        #region ProductController AddProduct

        [TestMethod]
        public void ProductController_AddProduct_Success()
        {
            repository.Setup(a => a.AddProduct(addProduct)).Returns(addProduct);
            var response = controller.AddProduct(addProduct);
            var conNegResult = response as OkNegotiatedContentResult<Product>;

            Assert.IsNotNull(conNegResult);
            Assert.AreEqual(addProduct, conNegResult.Content);
            Assert.IsInstanceOfType(conNegResult.Content, typeof(Product));
            Assert.AreEqual(HttpStatusCode.OK, response.ExecuteAsync(new CancellationToken()).Result.StatusCode);

        }

        [TestMethod]
        public void ProductController_AddProduct_BadRequest()
        {
            repository.Setup(a => a.AddProduct((Product)null));
            var response = controller.AddProduct((Product)null);

            Assert.IsInstanceOfType(response, typeof(BadRequestErrorMessageResult));
            Assert.AreEqual(HttpStatusCode.BadRequest, response.ExecuteAsync(new CancellationToken()).Result.StatusCode);

        }

        [TestMethod]
        public void ProductController_AddProduct_Exception()
        {
            repository.Setup(p => p.AddProduct(addProduct)).Throws<NullReferenceException>();
            var response = controller.AddProduct(addProduct);

            Assert.AreEqual(HttpStatusCode.InternalServerError, response.ExecuteAsync(new CancellationToken()).Result.StatusCode);
        }


        #endregion


        private void DataSetup()
        {
            productList.Add(new Product
            {
                ProductId = 1,
                ProductName = "Helical Gear",
                ProductSKU = "GR0001",
                ProductDescription = "25 tooth helical gear"
            });
            productList.Add(new Product
            {
                ProductId = 2,
                ProductName = "Spiral Gear",
                ProductSKU = "GR0002",
                ProductDescription = "34 tooth spiral gear"
            });
            productList.Add(new Product
            {
                ProductId = 3,
                ProductName = "Hollow Spindle",
                ProductSKU = "SP0010",
                ProductDescription = "4inch hollow threaded spindle"
            });

            addProduct = new Product
            {
                ProductName = "Spur Gear",
                ProductSKU = "GRSP0020",
                ProductDescription = "17 tooth 5in diameter spur gear"
            };
        }
    }
}
