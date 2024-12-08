using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace ARM.DAL.Extensions;

public static class DbExtensions
{
    /// <summary>
    /// Получить полное название таблицы с кавычками
    /// </summary>
    /// <param name="entityType"></param>
    /// <returns></returns>
    public static string? GetSchemaTableName(this IReadOnlyEntityType entityType)
    {
        var tableName = entityType.GetTableName();
        if (tableName == null)
        {
            return null;
        }

        var schema = entityType.GetSchema();
        return (string.IsNullOrEmpty(schema) ? "" : schema + ".") + $"\"{tableName}\"";
    }
}