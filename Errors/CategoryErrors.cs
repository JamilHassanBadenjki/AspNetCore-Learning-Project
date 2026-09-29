using FirstApi.Common;

namespace FirstApi.Errors;

public static class CategoryErrors
{
    public static Error NotFound(int id)
    {
        return new Error(
            "Category.NotFound",
            $"Category with id {id} was not found.",
            ErrorType.NotFound);
    }
}