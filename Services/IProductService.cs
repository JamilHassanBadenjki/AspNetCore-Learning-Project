using FirstApi.Models;

namespace FirstApi.Services;

public interface IProductService
{
    Task<List<Product>> GetAllAsync();
    Task<Product> CreateAsync(Product product);
    Task<Product?> GetByIdAsync(int id);
    Task<Product?> UpdateAsync(int id, string name, decimal price);
    Task<bool> DeleteAsync(int id);

}