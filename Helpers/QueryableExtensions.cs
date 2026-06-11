using System.Linq.Dynamic.Core;
using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Org.BouncyCastle.Ocsp;
using TraineeManagementApi.Models;

namespace TraineeManagementApi.Helpers;

public static class QueryableExtensions
{
    public static IQueryable<Trainee> ApplyPagination(this IQueryable<Trainee> query, int pageNumber, int pageSize)
    {
        return query
            .OrderBy(t => t.CreatedDate)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize);
    }

    public static IQueryable<Trainee> ApplySearch(this IQueryable<Trainee> query, string? search)
    {
        if (string.IsNullOrWhiteSpace(search))
            return query;

        return query.Where(t =>
            EF.Functions.Like(t.FirstName, $"%{search}%") ||
            EF.Functions.Like(t.LastName, $"%{search}%") ||
            EF.Functions.Like(t.TechStack, $"%{search}%") ||
            EF.Functions.Like(t.Email, $"%{search}%"));
    }

    public static IQueryable<Trainee> ApplyFilter(this IQueryable<Trainee> query, string? requiredStatus)
    {
        if (string.IsNullOrWhiteSpace(requiredStatus))
            return query;

        return query.Where(t => t.Status.Equals(requiredStatus));
    }
}