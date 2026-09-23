using FirstApi.Common;
using FirstApi.Data;
using FirstApi.Errors;
using FirstApi.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Data.SqlClient;

namespace FirstApi.Services;

public class ProductService : IProductService
{
    private readonly AppDbContext _db;

    public ProductService(AppDbContext db)
    {
        _db = db;
    }

    public async Task<List<Product>> GetAllAsync()
    {
        return await _db.Products.ToListAsync();
    }

    public async Task<Result<Product>> CreateAsync(Product product)
    {
        product.Sku = product.Sku.Trim().ToUpperInvariant();

        var skuExists = await _db.Products
            .AnyAsync(p => p.Sku == product.Sku);

        if (skuExists)
        {
            return Result<Product>.Failure(
                ProductErrors.SkuAlreadyExists(product.Sku));
        }

        _db.Products.Add(product);

        try
        {
            await _db.SaveChangesAsync();
        }
        catch (DbUpdateException ex)
            when (IsUniqueConstraintViolation(ex))
        {
            return Result<Product>.Failure(
                ProductErrors.SkuAlreadyExists(product.Sku));
        }

        return Result<Product>.Success(product);
    }

    public async Task<Result<Product>> GetByIdAsync(int id)
    {
        var product = await _db.Products.FindAsync(id);

        if (product is null)
        {
            return Result<Product>.Failure(
                ProductErrors.NotFound(id));
        }

        return Result<Product>.Success(product);
    }

    public async Task<Result<Product>> UpdateAsync(
        int id,
        string name,
        string sku,
        decimal price)
    {
        var product = await _db.Products.FindAsync(id);

        if (product is null)
        {
            return Result<Product>.Failure(
                ProductErrors.NotFound(id));
        }

        sku = sku.Trim().ToUpperInvariant();

        var skuExists = await _db.Products
            .AnyAsync(p => p.Sku == sku && p.Id != id);

        if (skuExists)
        {
            return Result<Product>.Failure(
                ProductErrors.SkuAlreadyExists(sku));
        }

        product.Name = name;
        product.Sku = sku;
        product.Price = price;

        try
        {
            await _db.SaveChangesAsync();
        }
        catch (DbUpdateException ex)
            when (IsUniqueConstraintViolation(ex))
        {
            return Result<Product>.Failure(
                ProductErrors.SkuAlreadyExists(sku));
        }

        return Result<Product>.Success(product);
    }
    public async Task<Result> DeleteAsync(int id)
    {
        var product = await _db.Products.FindAsync(id);

        if (product is null)
        {
            return Result.Failure(
                ProductErrors.NotFound(id));
        }

        _db.Products.Remove(product);

        await _db.SaveChangesAsync();

        return Result.Success();
    }

    private static bool IsUniqueConstraintViolation(
        DbUpdateException exception)
    {
        return exception.InnerException is SqlException sqlException
            && (sqlException.Number == 2601
                || sqlException.Number == 2627);
    }
}