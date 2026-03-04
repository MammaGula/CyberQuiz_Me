using CyberQuiz.DAL.Entities;
using System.Threading;

namespace CyberQuiz.DAL.Repositories.Interfaces;

public interface ISubCategoryRepository
{

    // Returns all subcategories from the database(For small cases, big case is quite slow)
    Task<List<SubCategory>> GetAllAsync(CancellationToken cancellationToken = default);

    // Returns subcategories that belong to a specific category(Suitable for SubCategory List-Page)
    Task<List<SubCategory>> GetByCategoryAsync(int categoryId, CancellationToken cancellationToken = default);

    // Returns a specific subcategory by its ID(Suitable for SubCategory Edit-Page)
    Task<SubCategory?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task AddAsync(SubCategory subCategory);
    void Remove(SubCategory subCategory);
}
