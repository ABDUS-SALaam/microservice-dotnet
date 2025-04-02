using eCommerce.SharedLibrary.Logs;
using eCommerce.SharedLibrary.Responses;
using ProductApi.Application.Interfaces;
using ProductApi.Domain.Entities;
using ProductApi.Infrastructure.Data;
using System.Linq.Expressions;

namespace ProductApi.Infrastructure.Repositories
{
    public class ProductRepository(ProductDbContext context) : IProduct
    {
        public async Task<Response> CreateAsync(Product entity)
        {
            try {
                var getProduct = await GetByAsync(_=>_.Name!.Equals(entity.Name));
                if (getProduct is not null && !string.IsNullOrEmpty(getProduct.Name))
                    return new Response(true,$"{entity.Name} already added");
                var currEntity=context.Products.Add(entity).Entity;
                await context.SaveChangesAsync();
                if (currEntity is not null && currEntity.Id > 0)
                      return new Response(true,$"{entity.Name} added !!");
                return new Response(false, "Failed to add !!");
            }
            catch(Exception ex) { 
                LogException.LogExceptions(ex);
                return new Response(false,"Error occured adding new Product");
            }
        }

        public Task<Response> DeleteAsync(Product entity)
        {
            throw new NotImplementedException();
        }

        public Task<Product> FindByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Product>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<Product> GetByAsync(Expression<Func<Product, bool>> predicate)
        {
            throw new NotImplementedException();
        }

        public Task<Response> UpdateAsync(Product entity)
        {
            throw new NotImplementedException();
        }
    }
}
