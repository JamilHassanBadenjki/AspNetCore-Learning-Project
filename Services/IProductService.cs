using FirstApi.Common;
using FirstApi.Models;

namespace FirstApi.Services;

public interface IProductService
{
    Task<List<Product>> GetAllAsync();

    Task<Result<Product>> GetByIdAsync(int id);

    Task<Result<Product>> CreateAsync(Product product);

    Task<Result<Product>> UpdateAsync(
        int id,
        string name,
        string sku,
        decimal price);

    Task<Result> DeleteAsync(int id);
}