using FirstApi.Models;
using FirstApi.Common;

namespace FirstApi.Services;

public interface IProductService
{
    Task<List<Product>> GetAllAsync();
    Task<Product> CreateAsync(Product product);
    Task<Result<Product?>> GetByIdAsync(int id);
    Task<Result<Product>> UpdateAsync(
    int id,
    string name,
    decimal price);
    Task<Result> DeleteAsync(int id);

}