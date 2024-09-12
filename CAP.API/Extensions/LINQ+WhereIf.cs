using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;

namespace CAP.API.Extensions;

public static class LinqWhereIf
{
    /// <summary>
    /// Filters a <see cref="IQueryable{T}"/> by given predicate if given condition is true.
    /// </summary>
    /// <param name="query">Queryable to apply filtering</param>
    /// <param name="condition">A boolean value</param>
    /// <param name="predicate">Predicate to filter the query</param>
    /// <returns>Filtered or not filtered query based on <paramref name="condition"/></returns>
    public static IQueryable<T> WhereIf<T>(this IQueryable<T> query, bool condition,
        Expression<Func<T, bool>> predicate)
    {
        return condition
            ? query.Where(predicate)
            : query;
    }

    /// <summary>
    /// Filters a <see cref="IQueryable{T}"/> by given predicate if given condition is true.
    /// </summary>
    /// <param name="query">Queryable to apply filtering</param>
    /// <param name="condition">A boolean value</param>
    /// <param name="predicate">Predicate to filter the query</param>
    /// <returns>Filtered or not filtered query based on <paramref name="condition"/></returns>
    public static IQueryable<T> WhereIf<T>(this IQueryable<T> query, bool condition,
        Expression<Func<T, int, bool>> predicate)
    {
        return condition
            ? query.Where(predicate)
            : query;
    }

    /// <summary>
    /// Filters a <see cref="IQueryable{T}"/> by given predicate if given condition is true, else if given elseIfCondition is true.
    /// If both are true, it combines the two predicates with a logical OR. If neither are correct it returns the original query.
    /// </summary>
    /// <param name="query">
    /// Queryable to apply filtering
    /// </param>
    /// <param name="condition">
    /// A boolean value
    /// </param>
    /// <param name="predicate">
    /// Predicate to filter the query
    /// </param>
    /// <param name="elseCondition">
    /// Condition to check if the first condition is false
    /// </param>
    /// <param name="elseIfPredicate">
    /// Predicate to filter the query if condition is false
    /// </param>
    /// <typeparam name="T">
    /// Type of the query
    /// </typeparam>
    /// <returns>
    /// Filtered or not filtered query based on <paramref name="condition"/>
    /// </returns>
    public static IQueryable<T> WhereIfElseIf<T>(this IQueryable<T> query, bool condition,
        Expression<Func<T, bool>> predicate, bool elseCondition, Expression<Func<T, bool>> elseIfPredicate)
    {
        return condition switch
        {
            true when elseCondition => query.Where(MergePredicates(predicate, elseIfPredicate)),
            true => query.Where(predicate),
            _ => elseCondition ? query.Where(elseIfPredicate) : query
        };
    }

    /// <summary>
    /// Filters a <see cref="IQueryable{T}"/> by given raw sql if given condition is true.
    /// </summary>
    /// <param name="query">Queryable to apply filtering</param>
    /// <param name="condition">A boolean value</param>
    /// <param name="sql">Raw sql to run</param>
    /// <returns>Filtered or not filtered query based on <paramref name="condition"/></returns>
    public static IQueryable<T> FromSqlInterpolatedIf<T>(this DbSet<T> query, bool condition, FormattableString sql)
        where T : class
    {
        return condition
            ? query.FromSqlInterpolated(sql)
            : query;
    }


    /// <summary>
    ///  Merge two predicates using OrElse -> Logical OR
    /// </summary>
    /// <param name="predicate1">
    /// First predicate
    /// </param>
    /// <param name="predicate2">
    /// Second predicate
    /// </param>
    /// <typeparam name="T">
    /// Type of the query
    /// </typeparam>
    /// <returns>
    /// Merged predicate
    /// </returns>
    private static Expression<Func<T, bool>> MergePredicates<T>(Expression<Func<T, bool>> predicate1,
        Expression<Func<T, bool>> predicate2)
    {
        var param = Expression.Parameter(typeof(T), "x");

        // Merge the two predicates using AndAlso -> Logical AND
        var body = Expression.OrElse(
            Expression.Invoke(predicate1, param),
            Expression.Invoke(predicate2, param)
        );
        return Expression.Lambda<Func<T, bool>>(body, param);
    }
}