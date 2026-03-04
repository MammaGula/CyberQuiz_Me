using CyberQuiz.DAL.Data;
using CyberQuiz.DAL.Entities;
using CyberQuiz.DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Threading;

namespace CyberQuiz.DAL.Repositories;

public class CategoryRepository : ICategoryRepository
{
    private readonly CyberQuizDbContext _db;

    public CategoryRepository(CyberQuizDbContext db)
    {
		ArgumentNullException.ThrowIfNull(db);
        _db = db;
    }

    // Returns all categories from the database
    public async Task<List<Category>> GetAllAsync(CancellationToken cancellationToken = default)
        => await _db.Categories
            .AsNoTracking()
            .OrderBy(c => c.Id)
            .ToListAsync(cancellationToken);

    // Returns all categories including their related subcategories
    public async Task<List<Category>> GetAllWithSubCategoriesAsync(CancellationToken cancellationToken = default)
        => await _db.Categories
            .AsNoTracking()
            .OrderBy(c => c.Id)
            .Include(c => c.SubCategories)
            .AsSplitQuery()
            .ToListAsync(cancellationToken);

    // Returns a category by its ID
    public async Task<Category?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        => await _db.Categories.FindAsync([id], cancellationToken);

    public async Task AddAsync(Category category)
	{
		ArgumentNullException.ThrowIfNull(category);
		await _db.Categories.AddAsync(category);
	}

    public void Remove(Category category)
	{
		ArgumentNullException.ThrowIfNull(category);
		_db.Categories.Remove(category);
	}
}