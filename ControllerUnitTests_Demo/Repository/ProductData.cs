using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ControllerUnitTests_Demo.Repository
{
    public class ProductData : IProductData
    {
        private readonly List<Product> productList = new List<Product>();
        public ProductData()
        {
            Product addProduct = new Product
            {
                ProductId = 1,
                ProductName = "Helical Gear",
                ProductSKU = "GR0001",
                ProductDescription = "35 tooth helical gear"
            };

            productList.Add(addProduct);

            addProduct = null;

            addProduct = new Product
            {
                ProductId = 2,
                ProductName = "Helical Gear",
                ProductSKU = "GR0002",
                ProductDescription = "43 tooth helical gear"
            };

            productList.Add(addProduct);

            addProduct = null;

            addProduct = new Product
            {
                ProductId = 3,
                ProductName = "Sprial Gear",
                ProductSKU = "GR0003",
                ProductDescription = "12 tooth spiral gear"
            };

            productList.Add(addProduct);
        }

        public IQueryable<Product> GetAllProducts()
        {
            return productList.AsQueryable<Product>();
        }

        public Product GetProductById(int id)
        {
            var productItem =
                (from item in productList
                .Where(x => x.ProductId == id)
                select item).FirstOrDefault();

            return productItem;
        }

        public Product AddProduct(Product newProduct)
        {
            int nextId = productList.Max(i => i.ProductId);
            newProduct.ProductId = nextId++;
            productList.Add(newProduct);
            return newProduct;
        }
    }
}