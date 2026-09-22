using FirstApi.Common;

namespace FirstApi.Errors;

public static class ProductErrors
{
    public static Error NotFound(int id)
    {
        return new Error(
            "Product.NotFound",
            $"Product with id {id} was not found.",
            ErrorType.NotFound);
    }

    public static Error SkuAlreadyExists(string sku)
    {
        return new Error(
            "Product.SkuAlreadyExists",
            $"A product with SKU '{sku}' already exists.",
            ErrorType.Conflict);
    }
}