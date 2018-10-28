using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControllerUnitTests_Demo.Repository
{
    public interface IProductData
    {
        IQueryable<Product> GetAllProducts();
        Product GetProductById(int id);
        Product AddProduct(Product newProduct);
    }
}
