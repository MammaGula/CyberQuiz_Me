using System.ComponentModel.DataAnnotations;

namespace CyberQuiz.DAL.Entities;

// One question : many answer options
public class Question
{
    public int Id { get; set; }

    [Required, MaxLength(1000)]
    public string Text { get; set; } = string.Empty; // Text of the question

    public int SubCategoryId { get; set; }  // FK : to the related subcategory
    public int Points { get; set; } = 1; // Points awarded for answering this question correctly



    //Navigation property for the related subcategory(many Questions-one SubCategory)
    public SubCategory SubCategory { get; set; } = null!;

    // Navigation property for related answer options (one Question-many Options)
    public ICollection<AnswerOption> AnswerOptions { get; set; } = new List<AnswerOption>();

    // Navigation property for related user results (one Question-many UserResults)
    public ICollection<UserResult> Results { get; set; } = new List<UserResult>();
}