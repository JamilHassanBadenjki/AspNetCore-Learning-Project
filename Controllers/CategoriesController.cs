using FirstApi.Common;
using FirstApi.DTOs;
using FirstApi.Models;
using FirstApi.Services;
using Microsoft.AspNetCore.Mvc;
using FirstApi.Extensions;

namespace FirstApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CategoriesController : ControllerBase
{
    private readonly ICategoryService _categoryService;

    public CategoriesController(
        ICategoryService categoryService)
    {
        _categoryService = categoryService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var categories =
            await _categoryService.GetAllAsync();

        var response = categories
            .Select(ToResponse)
            .ToList();

        return Ok(response);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var result =
            await _categoryService.GetByIdAsync(id);

        if (result.IsFailure)
        {
            return this.ToProblem(result.Error!);
        }

        return Ok(ToResponse(result.Value!));
    }

    [HttpPost]
    public async Task<IActionResult> Create(
        CreateCategoryRequest request)
    {
        var category = new Category
        {
            Name = request.Name
        };

        var createdCategory =
            await _categoryService.CreateAsync(category);

        return Ok(ToResponse(createdCategory));
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(
        int id,
        UpdateCategoryRequest request)
    {
        var result =
            await _categoryService.UpdateAsync(
                id,
                request.Name);

        if (result.IsFailure)
        {
            return this.ToProblem(result.Error!);
        }

        return Ok(ToResponse(result.Value!));
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var result =
            await _categoryService.DeleteAsync(id);

        if (result.IsFailure)
        {
            return this.ToProblem(result.Error!);
        }

        return NoContent();
    }

    private static CategoryResponse ToResponse(
        Category category)
    {
        return new CategoryResponse
        {
            Id = category.Id,
            Name = category.Name
        };
    }

}