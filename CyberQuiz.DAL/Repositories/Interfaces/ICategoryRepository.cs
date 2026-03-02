using CyberQuiz.DAL.Entities;
using System.Threading;

namespace CyberQuiz.DAL.Repositories.Interfaces;

public interface ICategoryRepository
{
    // Returns all categories from the database
    Task<List<Category>> GetAllAsync();


    // Returns all categories including their related subcategories
    Task<List<Category>> GetAllWithSubCategoriesAsync(CancellationToken cancellationToken = default);
   
    // Returns a category by its ID
    Task<Category?> GetByIdAsync(int id);
    Task AddAsync(Category category);
    void Remove(Category category);
}