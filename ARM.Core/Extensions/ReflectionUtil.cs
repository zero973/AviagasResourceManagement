using System.Globalization;
using System.Linq.Expressions;
using System.Reflection;
using ARM.Core.Enums;
using ARM.Core.Models.UI;

namespace ARM.Core.Extensions;

public static class ReflectionUtil
{
    public static PropertyInfo? GetPropertyForExpression(Type type, string name)
    {
        PropertyInfo property = type.GetProperties().FirstOrDefault(x => x.Name.ToLower().Equals(name.ToLower()));
        if (property == null)
            return null;

        return property.DeclaringType.GetProperty(property.Name);
    }

    public static MemberExpression GetPropertyForExpression(Expression expr, string name)
    {
        PropertyInfo propertyForExpression = GetPropertyForExpression(expr.Type, name);
        return Expression.Property(expr, propertyForExpression);
    }

    public static Expression ReadComplexFilterExpression(this ComplexFilter item, ParameterExpression x)
    {
        if (item == null)
        {
            return null;
        }

        if (item.Field != null && item.Value != null)
        {
            MemberExpression propertyForExpression = GetPropertyForExpression(x, item.Field);

            return GetPropertyFilterExpression(propertyForExpression, item.Value, item.Operator);
        }

        throw new ArgumentException("Field или Value = null");
    }

    public static Expression GetPropertyFilterExpression(MemberExpression propertyExpression, object value, ComplexFilterOperators op)
    {
        Type type = propertyExpression.Type;

        // только для массивов
        // пример использования: когда мы ходим выгрузить все сущности, где Id находится в массиве
        if (op == ComplexFilterOperators.In)
        {
            return CreateArrayExpression(propertyExpression, value);
        }
        
        if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>))
        {
            type = type.GetGenericArguments()[0];
        }

        if (type.BaseType == typeof(Enum))
        {
            return CreateEnumExpression(propertyExpression, op, value.ToString(), type);
        }

        if (type.Is<Guid>())
        {
            return CreateGuidExpression(propertyExpression, op, value);
        }

        switch (Type.GetTypeCode(type))
        {
            case TypeCode.String:
                return CreateStringExpression(propertyExpression, op, value.ToString());
            case TypeCode.Boolean:
                return CreateBoolExpression(propertyExpression, op, value.ToString());
            case TypeCode.DateTime:
                return CreateDateTimeExpression(propertyExpression, op, value);
            case TypeCode.SByte:
            case TypeCode.Byte:
            case TypeCode.Int16:
            case TypeCode.UInt16:
            case TypeCode.Int32:
            case TypeCode.UInt32:
            case TypeCode.Int64:
            case TypeCode.UInt64:
                return CreateNumericExpression(propertyExpression, op, value);
            case TypeCode.Single:
            case TypeCode.Double:
            case TypeCode.Decimal:
                return CreateFloatNumericExpression(propertyExpression, op, value);
            default:
                throw new Exception("Не найден обработчик для такого типа объекта");
        }
    }

    private static Expression CreateArrayExpression(MemberExpression propertyExpression, object value)
    {
        // Преобразуем Value в IEnumerable
        var values = value as IEnumerable<object>;

        var enumerable = values as object[];
        if (values == null || !enumerable.Any())
        {
            throw new ArgumentException("В массиве нет элементов");
        }

        // Создаём выражение, которое вызывает Enumerable.Cast<T>()
        var castMethod = typeof(Enumerable)
            .GetMethods()
            .First(m => m.Name == nameof(Enumerable.Cast) && m.GetParameters().Length == 1)
            .MakeGenericMethod(propertyExpression.Type);

        // Создаём выражение для IEnumerable<T>
        var castValuesExpression = Expression.Call(
            null, // Статический метод
            castMethod,
            Expression.Constant(values)
        );

        // Теперь выражение для Contains
        var containsMethod = typeof(Enumerable)
            .GetMethods()
            .First(m => m.Name == nameof(Enumerable.Contains) && m.GetParameters().Length == 2)
            .MakeGenericMethod(propertyExpression.Type);  // Используем тот же тип для Contains

        var containsExpression = Expression.Call(
            null, // Статический метод
            containsMethod,
            castValuesExpression,  // Преобразованная коллекция
            propertyExpression      // Поле, которое мы проверяем
        );

        return containsExpression;
    }

    private static Expression CreateEnumExpression(MemberExpression propertyExpression, ComplexFilterOperators op, string value, Type valueType)
    {
        object obj;
        if (string.IsNullOrEmpty(value) || value == "-")
        {
            obj = null;
        }
        else
        {
            try
            {
                obj = Enum.Parse(valueType, value);
            }
            catch
            {
                obj = null;
            }
        }

        if (obj != null)
        {
            return Expression.Equal(propertyExpression, Expression.Convert(Expression.Constant(obj), propertyExpression.Type));
        }

        return Expression.Constant(false);
    }

    private static Expression CreateGuidExpression(MemberExpression expression, ComplexFilterOperators op, object value)
    {
        Guid guid;
        switch (op)
        {
            case ComplexFilterOperators.Equals:
                if (!Guid.TryParse(value.ToString(), out guid))
                {
                    throw new ArgumentException("{0} fail cast to Guid");
                }

                return Expression.Equal(expression,
                       Expression.Convert(Expression.Constant(guid), expression.Type));
            case ComplexFilterOperators.NotEquals:
                if (!Guid.TryParse(value.ToString(), out guid))
                {
                    throw new ArgumentException("{0} fail cast to Guid");
                }

                return Expression.NotEqual(
                    expression,
                    Expression.Convert(Expression.Constant(guid), expression.Type));
        }
        throw new Exception($"Не найден оператор ({op}) для работы с GUID");
    }

    private static Expression CreateStringExpression(MemberExpression expression, ComplexFilterOperators op, string value)
    {
        switch (op)
        {
            case ComplexFilterOperators.Contains:
                return Expression.AndAlso(
                    Expression.NotEqual(expression, Expression.Constant(null, typeof(object))),
                    Expression.Call(
                        Expression.Call(expression, typeof(string).GetMethod("ToLower", new Type[0])),
                            typeof(string).GetMethods().First(x => x.Name == "Contains"), Expression.Constant(value.ToString().ToLower()))); // value.ToString().ToLower() убрал ту ловер, т.к. культуру в БД нельзя поменять
            case ComplexFilterOperators.StartsWith:
                return Expression.AndAlso(
                    Expression.NotEqual(expression, Expression.Constant(null, typeof(object))),
                    Expression.Call(Expression.Call(Expression.Call(expression, typeof(string).GetMethod("Trim", new Type[0])), typeof(string).GetMethod("ToLower", new Type[0])),
                        typeof(string).GetMethod("StartsWith", new[] { typeof(string) }), Expression.Constant(value.ToString().ToLower())));
            case ComplexFilterOperators.EndsWith:
                return Expression.AndAlso(
                    Expression.NotEqual(expression, Expression.Constant(null, typeof(object))),
                    Expression.Call(
                        Expression.Call(Expression.Call(expression, typeof(string).GetMethod("Trim", new Type[0])), typeof(string).GetMethod("ToLower", new Type[0])),
                            typeof(string).GetMethod("EndsWith", new[] { typeof(string) }), Expression.Constant(value.ToString().ToLower())));
            case ComplexFilterOperators.Equals:
                return Expression.AndAlso(
                    Expression.NotEqual(expression, Expression.Constant(null, typeof(object))),
                    Expression.Equal(
                        Expression.Call(expression, typeof(string).GetMethod("Trim", new Type[0])),
                        Expression.Convert(Expression.Constant(value), expression.Type)));
            case ComplexFilterOperators.NotEquals:
                return Expression.AndAlso(
                    Expression.NotEqual(expression, Expression.Constant(null, typeof(object))),
                    Expression.NotEqual(Expression.Call(expression, typeof(string).GetMethod("Trim", new Type[0])), Expression.Convert(Expression.Constant(value), expression.Type)));
        }
        throw new Exception($"Не найден оператор ({op}) для работы со строками");
    }

    private static Expression CreateBoolExpression(MemberExpression property, ComplexFilterOperators op, string value)
    {
        bool flag;
        switch (value.ToLower().Trim())
        {
            case "true":
            case "on":
            case "1":
                flag = true;
                break;
            case "false":
            case "off":
            case "0":
                flag = false;
                break;
            default:
                return Expression.Constant(false);
        }

        switch (op)
        {
            case ComplexFilterOperators.Equals:
                return Expression.Equal(property, Expression.Convert(Expression.Constant(flag), property.Type));
            case ComplexFilterOperators.NotEquals:
                return Expression.NotEqual(property, Expression.Convert(Expression.Constant(flag), property.Type));
        }

        throw new Exception($"Не найден оператор ({op}) для работы с типом bool");
    }

    private static Expression CreateDateTimeExpression(MemberExpression propertyExpression, ComplexFilterOperators op, object value)
    {
        DateTime dateTime = value.ToDateTime();
        if (dateTime != DateTime.MinValue)
        {
            Type type = propertyExpression.Type;
            MemberExpression memberExpression = ((type == typeof(DateTime)) ? ReflectionUtil.GetPropertyForExpression(propertyExpression, "Date") : ReflectionUtil.GetPropertyForExpression(ReflectionUtil.GetPropertyForExpression(propertyExpression, "Value"), "Date"));
            Expression expression = null;
            switch (op)
            {
                case ComplexFilterOperators.Equals:
                    expression = Expression.Equal(memberExpression, Expression.Convert(Expression.Constant(dateTime), memberExpression.Type));
                    break;
                case ComplexFilterOperators.NotEquals:
                    expression = Expression.NotEqual(memberExpression, Expression.Convert(Expression.Constant(dateTime), memberExpression.Type));
                    break;
                case ComplexFilterOperators.GreaterThan:
                    expression = Expression.GreaterThan(memberExpression, Expression.Convert(Expression.Constant(dateTime), memberExpression.Type));
                    break;
                case ComplexFilterOperators.GreaterThanOrEqual:
                    expression = Expression.GreaterThanOrEqual(memberExpression, Expression.Convert(Expression.Constant(dateTime), memberExpression.Type));
                    break;
                case ComplexFilterOperators.LessThan:
                    expression = Expression.LessThan(memberExpression, Expression.Convert(Expression.Constant(dateTime), memberExpression.Type));
                    break;
                case ComplexFilterOperators.LessThanOrEqual:
                    expression = Expression.LessThanOrEqual(memberExpression, Expression.Convert(Expression.Constant(dateTime), memberExpression.Type));
                    break;
                case ComplexFilterOperators.Contains:
                    {
                        BinaryExpression left = Expression.GreaterThanOrEqual(memberExpression, Expression.Convert(Expression.Constant(dateTime), memberExpression.Type));
                        BinaryExpression right = Expression.LessThan(memberExpression, Expression.Convert(Expression.Constant(dateTime.AddDays(1.0)), memberExpression.Type));
                        expression = Expression.AndAlso(left, right);
                        break;
                    }
            }

            if (expression != null)
            {
                if (type == typeof(DateTime?))
                {
                    expression = Expression.AndAlso(GetPropertyForExpression(propertyExpression, "HasValue"), expression);
                }

                return expression;
            }
        }

        return Expression.Constant(false);
    }

    private static Expression CreateNumericExpression(MemberExpression propertyExpression, ComplexFilterOperators op, object value)
    {
        long num = value.ToLong();
        return op switch
        {
            ComplexFilterOperators.Equals => Expression.Equal(propertyExpression, Expression.Convert(Expression.Constant(num), propertyExpression.Type)),
            ComplexFilterOperators.NotEquals => Expression.NotEqual(propertyExpression, Expression.Convert(Expression.Constant(num), propertyExpression.Type)),
            ComplexFilterOperators.GreaterThan => Expression.GreaterThan(propertyExpression, Expression.Convert(Expression.Constant(num), propertyExpression.Type)),
            ComplexFilterOperators.GreaterThanOrEqual => Expression.GreaterThanOrEqual(propertyExpression, Expression.Convert(Expression.Constant(num), propertyExpression.Type)),
            ComplexFilterOperators.LessThan => Expression.LessThan(propertyExpression, Expression.Convert(Expression.Constant(num), propertyExpression.Type)),
            ComplexFilterOperators.LessThanOrEqual => Expression.LessThanOrEqual(propertyExpression, Expression.Convert(Expression.Constant(num), propertyExpression.Type)),
            _ => throw new Exception($"Не найден оператор ({op}) для работы с числовым типом"),
        };
    }

    private static Expression CreateFloatNumericExpression(MemberExpression propertyExpression, ComplexFilterOperators op, object value)
    {
        decimal num = value.ToDecimal();
        return op switch
        {
            ComplexFilterOperators.Equals => Expression.Equal(propertyExpression, Expression.Convert(Expression.Constant(num), propertyExpression.Type)),
            ComplexFilterOperators.NotEquals => Expression.NotEqual(propertyExpression, Expression.Convert(Expression.Constant(num), propertyExpression.Type)),
            ComplexFilterOperators.GreaterThan => Expression.GreaterThan(propertyExpression, Expression.Convert(Expression.Constant(num), propertyExpression.Type)),
            ComplexFilterOperators.GreaterThanOrEqual => Expression.GreaterThanOrEqual(propertyExpression, Expression.Convert(Expression.Constant(num), propertyExpression.Type)),
            ComplexFilterOperators.LessThan => Expression.LessThan(propertyExpression, Expression.Convert(Expression.Constant(num), propertyExpression.Type)),
            ComplexFilterOperators.LessThanOrEqual => Expression.LessThanOrEqual(propertyExpression, Expression.Convert(Expression.Constant(num), propertyExpression.Type)),
            _ => throw new Exception($"Не найден оператор ({op}) для работы с плавающим числовым типом"),
        };
    }

    public static bool Is<TType>(this Type type)
    {
        Type typeFromHandle = typeof(TType);
        return type.Is(typeFromHandle);
    }

    public static bool Is(this Type type, Type baseType)
    {
        if (type == null || baseType == null)
            throw new ArgumentNullException("Null something !");

        if (type == baseType || baseType.IsAssignableFrom(type))
        {
            return true;
        }

        bool flag = baseType.IsInterface && type.GetInterface(baseType.FullName, ignoreCase: true) != null;

        if (!flag)
        {
            return type.IsSubclassOf(baseType);
        }

        return true;
    }

    private static DateTime ToDateTime(this object obj)
    {
        if (obj == null)
        {
            return DateTime.MinValue;
        }

        if (obj is DateTime)
        {
            return (DateTime)obj;
        }

        if (DateTime.TryParse(obj.ToString(), out var result))
        {
            return result;
        }

        return DateTime.MinValue;
    }

    private static long ToLong(this object obj)
    {
        if (obj is bool)
        {
            return ((bool)obj) ? 1 : 0;
        }

        return obj.ToLong(0L);
    }

    private static long ToLong(this object obj, long defaultValue)
    {
        if (obj == null || obj == DBNull.Value)
        {
            return defaultValue;
        }

        if (obj is long)
        {
            return (long)obj;
        }

        if (long.TryParse(obj.ToString(), out var result))
        {
            return result;
        }

        return defaultValue;
    }

    public static decimal ToDecimal(this object obj)
    {
        if (obj == null)
        {
            return 0m;
        }

        if (obj is decimal)
        {
            return (decimal)obj;
        }

        string text = obj.ToString();
        if (text == string.Empty)
        {
            return 0m;
        }

        if (decimal.TryParse(text.Replace(CultureInfo.InvariantCulture.NumberFormat.CurrencyDecimalSeparator, CultureInfo.CurrentCulture.NumberFormat.CurrencyDecimalSeparator), out var result))
        {
            return result;
        }

        return 0m;
    }

}