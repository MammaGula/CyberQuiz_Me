using System.ComponentModel.DataAnnotations;

namespace CyberQuiz.DAL.Entities;

// One AnwerOption: many Ressults
public class AnswerOption
{
    public int Id { get; set; }

    [Required]
    [MaxLength(500)]
    public string Text { get; set; } = string.Empty; // Text of the AnswerOption
    public bool IsCorrect { get; set; } // Is this the correct answer for the question?

    // The order to display ex: 1st option, 2nd option, etc. (for UI purposes)
    public int DisplayOrder { get; set; }
    public int QuestionId { get; set; } // FK to the related question 



    //Navigation property for the related question (one question-many options)
    public Question Question { get; set; } = null!;

    // This option can be selected in many user results > Check if it is popular choice, who selected it, etc.
    public ICollection<UserResult> UserResults { get; set; } = new List<UserResult>();
}






// Init : Can only set the value during object initialization 
// [Required] : For Database schema
// null! or ? : EF entity navigation properties, Tricking the compiler into not complaining that this property is null
// required : DTOs 

