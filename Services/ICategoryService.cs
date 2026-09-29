using FirstApi.Common;
using FirstApi.Models;

namespace FirstApi.Services;

public interface ICategoryService
{
    Task<List<Category>> GetAllAsync();

    Task<Result<Category>> GetByIdAsync(int id);

    Task<Category> CreateAsync(Category category);

    Task<Result<Category>> UpdateAsync(
        int id,
        string name);

    Task<Result> DeleteAsync(int id);
}