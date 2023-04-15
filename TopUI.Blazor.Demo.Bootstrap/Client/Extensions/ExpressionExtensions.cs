using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;
using System.Reflection;

namespace TopUI.Blazor.Demo.Bootstrap.Client.Extensions;
public static class ExpressionExtensions
{
    /// <summary>
    ///     Returns a list of <see cref="PropertyInfo" /> extracted from the given simple
    ///     <see cref="LambdaExpression" />.
    /// </summary>
    /// <remarks>
    ///     <para>
    ///         Only simple expressions are supported, such as those used to reference a property.
    ///     </para>
    ///     <para>
    ///         This method is typically used by database providers (and other extensions). It is generally
    ///         not used in application code.
    ///     </para>
    /// </remarks>
    /// <param name="propertyAccessExpression">The expression.</param>
    /// <returns>The list of referenced properties.</returns>
    public static IReadOnlyList<PropertyInfo> GetPropertyAccessList(this LambdaExpression propertyAccessExpression)
    {
        if (propertyAccessExpression.Parameters.Count != 1)
        {
            throw new ArgumentException(nameof(propertyAccessExpression));
        }

        var propertyPaths = propertyAccessExpression
            .MatchMemberAccessList((p, e) => e.MatchSimpleMemberAccess<PropertyInfo>(p));

        if (propertyPaths == null)
        {
            throw new ArgumentException(nameof(propertyAccessExpression));
        }

        return propertyPaths;
    }

    /// <summary>
    ///     This is an internal API that supports the Entity Framework Core infrastructure and not subject to
    ///     the same compatibility standards as public APIs. It may be changed or removed without notice in
    ///     any release. You should only use it directly in your code with extreme caution and knowing that
    ///     doing so can result in application failures when updating to a new Entity Framework Core release.
    /// </summary>
    public static TMemberInfo? MatchSimpleMemberAccess<TMemberInfo>(
        this Expression parameterExpression,
        Expression memberAccessExpression)
        where TMemberInfo : MemberInfo
    {
        var memberInfos = MatchMemberAccess<TMemberInfo>(parameterExpression, memberAccessExpression);

        return memberInfos?.Count == 1 ? memberInfos[0] : null;
    }

    private static IReadOnlyList<TMemberInfo>? MatchMemberAccess<TMemberInfo>(
        this Expression parameterExpression,
        Expression memberAccessExpression)
        where TMemberInfo : MemberInfo
    {
        var memberInfos = new List<TMemberInfo>();

        var unwrappedExpression = RemoveTypeAs(RemoveConvert(memberAccessExpression));
        do
        {
            var memberExpression = unwrappedExpression as MemberExpression;

            if (!(memberExpression?.Member is TMemberInfo memberInfo))
            {
                return null;
            }

            memberInfos.Insert(0, memberInfo);

            unwrappedExpression = RemoveTypeAs(RemoveConvert(memberExpression.Expression));
        }
        while (unwrappedExpression != parameterExpression);

        return memberInfos;
    }

    /// <summary>
    ///     This is an internal API that supports the Entity Framework Core infrastructure and not subject to
    ///     the same compatibility standards as public APIs. It may be changed or removed without notice in
    ///     any release. You should only use it directly in your code with extreme caution and knowing that
    ///     doing so can result in application failures when updating to a new Entity Framework Core release.
    /// </summary>
    public static Expression? RemoveTypeAs(this Expression? expression)
    {
        while (expression?.NodeType == ExpressionType.TypeAs)
        {
            expression = ((UnaryExpression)RemoveConvert(expression)).Operand;
        }

        return expression;
    }

    // <summary>
    ///     This is an internal API that supports the Entity Framework Core infrastructure and not subject to
    ///     the same compatibility standards as public APIs. It may be changed or removed without notice in
    ///     any release. You should only use it directly in your code with extreme caution and knowing that
    ///     doing so can result in application failures when updating to a new Entity Framework Core release.
    /// </summary>
    public static IReadOnlyList<TMemberInfo>? MatchMemberAccessList<TMemberInfo>(
        this LambdaExpression lambdaExpression,
        Func<Expression, Expression, TMemberInfo?> memberMatcher)
        where TMemberInfo : MemberInfo
    {
        //Check.DebugAssert(lambdaExpression.Body != null, "lambdaExpression.Body is null");
        //Check.DebugAssert(
        //    lambdaExpression.Parameters.Count == 1,
        //    "lambdaExpression.Parameters.Count is " + lambdaExpression.Parameters.Count + ". Should be 1.");

        var parameterExpression = lambdaExpression.Parameters[0];

        if (RemoveConvert(lambdaExpression.Body) is NewExpression newExpression)
        {
            var memberInfos
                = (List<TMemberInfo>)newExpression
                    .Arguments
                    .Select(a => memberMatcher(a, parameterExpression))
                    .Where(p => p != null)
                    .ToList()!;

            return memberInfos.Count != newExpression.Arguments.Count ? null : memberInfos;
        }

        var memberPath = memberMatcher(lambdaExpression.Body, parameterExpression);

        return memberPath != null ? new[] { memberPath } : null;
    }

    [return: NotNullIfNotNull("expression")]
    private static Expression? RemoveConvert(Expression? expression)
    {
        if (expression is UnaryExpression unaryExpression
            && (expression.NodeType == ExpressionType.Convert
                || expression.NodeType == ExpressionType.ConvertChecked))
        {
            return RemoveConvert(unaryExpression.Operand);
        }

        return expression;
    }
}
