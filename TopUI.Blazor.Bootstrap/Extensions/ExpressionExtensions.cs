using System.Linq.Expressions;
using System.Reflection;

namespace TopUI.Blazor.Bootstrap.Extensions;
public static class ExpressionExtensions
{
    // Get the Member name from an expression.
    // (customer => customer.Name) returns "Name"
    public static string? GetMemberName<T>(this Expression<T> expression)
    {
        return expression.GetMemberInfo()?.Name;
    }

    public static MemberInfo? GetMemberInfo<T>(this Expression<T> expression)
    {
        return expression.Body switch
        {
            MemberExpression m => m.Member,
            UnaryExpression u when u.Operand is MemberExpression m => m.Member,
            _ => null
        };
    }

    public static string? GetDisplayName<T>(this Expression<T> expression)
    {
        var member = expression.GetMemberInfo();

        return member?.GetDisplayName();
    }

    public static string? GetPrompt<T>(this Expression<T> expression)
    {
        var member = expression.GetMemberInfo();

        return member?.GetPrompt();
    }


}
