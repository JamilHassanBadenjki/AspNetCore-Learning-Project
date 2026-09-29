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

    public static Error InUse(int id)
    {
        return new Error(
            "Category.InUse",
            $"Category with id {id} cannot be deleted because it is used by one or more products.",
            ErrorType.Conflict);
    }
}