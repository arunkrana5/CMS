using DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.ModelsMasterHelper
{
   public interface IProductHelper
    {
        List<Category.Admin.List> GetCategoryList(GetResponse modal);
        Category.Admin.Add GetCategory(GetResponse modal);
        PostResponse fnSetCategory(Category.Admin.Add model);

        List<Product.Admin.List> GetProductList(GetResponse modal);
        Product.Admin.Add GetProduct(GetResponse modal);
        PostResponse fnSetProduct(Product.Admin.Add model);
        List<ProductImages.Admin.List> GetProductImageList(GetResponse modal);
        ProductImages.Admin.Add GetProductImage(GetResponse modal);
        PostResponse fnSetProductImage(ProductImages.Admin.Add modal);
    }
}
