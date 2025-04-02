using ProductApi.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductApi.Application.DTO.Conversions
{
    public static class ProductConversion
    {
        public static Product ToEntity(ProductDTO product) => new()
        {
            Id = product.Id,
            Name = product.Name,
            Price = product.Price,
            Qunatity = product.Quantity,
        };

        public static (ProductDTO?, IEnumerable<ProductDTO>?) FromEntity(Product product,IEnumerable<Product>?products)
        {
            // return single
            if (product is not null && products is null)
            {
                var singleProduct = new ProductDTO(product!.Id,product.Name!,product.Qunatity,product.Price);
                return (singleProduct,null);
            }
            if(products is not null && product is null)
            {
                var _products = products.Select(p => {
                    return new ProductDTO(p.Id,p.Name!,p.Qunatity,p.Price);
                }).ToList();
                return (null,_products);
            }
            return (null,null);
        }
    }
}
