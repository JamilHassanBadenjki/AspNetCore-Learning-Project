using FirstApi.DTOs;
using FirstApi.Models;
using FirstApi.Services;
using Microsoft.AspNetCore.Mvc;
using FirstApi.Common;

namespace FirstApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductsController : ControllerBase
{
    private readonly IProductService _productService;

    public ProductsController(IProductService productService)
    {
        _productService = productService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var products = await _productService.GetAllAsync();

        var response = products
        .Select(ToResponse)
        .ToList();

        return Ok(response);
    }

    private static ProductResponse ToResponse(Product product)
    {
        return new ProductResponse
        {
            Id = product.Id,
            Name = product.Name,
            Price = product.Price
        };
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateProductRequest request)
    {
        var product = new Product
        {
            Name = request.Name,
            Price = request.Price
        };

        var createdProduct =
            await _productService.CreateAsync(product);

        return Ok(ToResponse(product));
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var result =
            await _productService.GetByIdAsync(id);

        if (result.IsFailure)
        {
            return ToProblem(result.Error!);
        }

        return Ok(ToResponse(result.Value!));
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(
        int id,
        UpdateProductRequest request)
    {
        var result = await _productService.UpdateAsync(
            id,
            request.Name,
            request.Price);

        if (result.IsFailure)
        {
            return ToProblem(result.Error!);
        }

        return Ok(ToResponse(result.Value!));
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var result = await _productService.DeleteAsync(id);

        if (result.IsFailure)
        {
            return ToProblem(result.Error!);
        }

        return NoContent();
    }




    private IActionResult ToProblem(Error error)
    {
        var statusCode = error.Type switch
        {
            ErrorType.Validation
                => StatusCodes.Status400BadRequest,

            ErrorType.NotFound
                => StatusCodes.Status404NotFound,

            ErrorType.Conflict
                => StatusCodes.Status409Conflict,

            _ => StatusCodes.Status500InternalServerError
        };

        return Problem(
            statusCode: statusCode,
            title: error.Code,
            detail: error.Message);
    }
}