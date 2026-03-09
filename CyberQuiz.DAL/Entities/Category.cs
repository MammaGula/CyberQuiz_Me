using System.ComponentModel.DataAnnotations;

namespace CyberQuiz.DAL.Entities;

// one category can have many subcategories, but a subcategory belongs to only one category
public class Category
{
    public int Id { get; set; }

    [Required, MaxLength(200)]
    public string Name { get; set; } = string.Empty; // Name of the category
    public string? Description { get; set; } // Optional description of the category


    //Navigation property for related subcategories
    public ICollection<SubCategory> SubCategories { get; set; } = new List<SubCategory>(); 
}
