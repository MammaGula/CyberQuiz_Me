using CyberQuiz.DAL.Data;
using CyberQuiz.DAL.Entities;
using CyberQuiz.DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading;
using System.Linq;

namespace CyberQuiz.DAL.Repositories;

public class SubCategoryRepository : ISubCategoryRepository
{
    private readonly CyberQuizDbContext _db;

    public SubCategoryRepository(CyberQuizDbContext db)
    {
		ArgumentNullException.ThrowIfNull(db);
        _db = db;
    }

    // Returns all subcategories from the database(For small cases, big case is quite slow)
    public async Task<List<SubCategory>> GetAllAsync(CancellationToken cancellationToken = default)
        => await _db.SubCategories
            .AsNoTracking()
            .OrderBy(sc => sc.CategoryId)
            .ThenBy(sc => sc.SortOrder)
            .ThenBy(sc => sc.Id)
            .ToListAsync(cancellationToken);


    // Returns subcategories that belong to a specific category(Suitable for SubCategory List-Page)
    public async Task<List<SubCategory>> GetByCategoryAsync(int categoryId, CancellationToken cancellationToken = default)
        => await _db.SubCategories
            .AsNoTracking()
            .Where(sc => sc.CategoryId == categoryId)
            .OrderBy(sc => sc.SortOrder)
            .ToListAsync(cancellationToken);


    // Returns subcategories that belong to any of the given categories (Useful for category overview pages)
    public async Task<List<SubCategory>> GetByCategoryIdsAsync(IEnumerable<int> categoryIds, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(categoryIds);

        var categoryIdArray = categoryIds.Distinct().ToArray();
        if (categoryIdArray.Length == 0)
        {
            return new List<SubCategory>();
        }

        return await _db.SubCategories
            .AsNoTracking()
            .Where(sc => categoryIdArray.Contains(sc.CategoryId))
            .OrderBy(sc => sc.CategoryId)
            .ThenBy(sc => sc.SortOrder)
            .ThenBy(sc => sc.Id)
            .ToListAsync(cancellationToken);
    }


    // Returns all categories including their related subcategories
    public async Task<SubCategory?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        => await _db.SubCategories.FindAsync([id], cancellationToken);

    public async Task AddAsync(SubCategory subCategory)
	{
		ArgumentNullException.ThrowIfNull(subCategory);
		await _db.SubCategories.AddAsync(subCategory);
	}

    public void Remove(SubCategory subCategory)
	{
		ArgumentNullException.ThrowIfNull(subCategory);
		_db.SubCategories.Remove(subCategory);
	}
}




