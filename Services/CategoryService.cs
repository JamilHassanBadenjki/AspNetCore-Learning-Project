using FirstApi.Common;
using FirstApi.Data;
using FirstApi.Errors;
using FirstApi.Models;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace FirstApi.Services;

public class CategoryService : ICategoryService
{
    private readonly AppDbContext _db;

    public CategoryService(AppDbContext db)
    {
        _db = db;
    }

    public async Task<List<Category>> GetAllAsync()
    {
        return await _db.Categories.ToListAsync();
    }

    public async Task<Result<Category>> GetByIdAsync(int id)
    {
        var category = await _db.Categories.FindAsync(id);

        if (category is null)
        {
            return Result<Category>.Failure(
                CategoryErrors.NotFound(id));
        }

        return Result<Category>.Success(category);
    }

    public async Task<Category> CreateAsync(Category category)
    {
        _db.Categories.Add(category);

        await _db.SaveChangesAsync();

        return category;
    }

    public async Task<Result<Category>> UpdateAsync(
        int id,
        string name)
    {
        var category = await _db.Categories.FindAsync(id);

        if (category is null)
        {
            return Result<Category>.Failure(
                CategoryErrors.NotFound(id));
        }

        category.Name = name;

        await _db.SaveChangesAsync();

        return Result<Category>.Success(category);
    }

    public async Task<Result> DeleteAsync(int id)
    {
        var category = await _db.Categories.FindAsync(id);

        if (category is null)
        {
            return Result.Failure(
                CategoryErrors.NotFound(id));
        }

        var hasProducts = await _db.Products
            .AnyAsync(p => p.CategoryId == id);

        if (hasProducts)
        {
            return Result.Failure(
                CategoryErrors.InUse(id));
        }

        _db.Categories.Remove(category);

        try
        {
            await _db.SaveChangesAsync();
        }
        catch (DbUpdateException ex)
            when (IsForeignKeyConstraintViolation(ex))
        {
            return Result.Failure(
                CategoryErrors.InUse(id));
        }

        return Result.Success();
    }

    private static bool IsForeignKeyConstraintViolation(
        DbUpdateException exception)
    {
        return exception.InnerException is SqlException sqlException
            && sqlException.Number == 547;
    }
}