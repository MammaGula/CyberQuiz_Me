using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace CyberQuiz.DAL.Entities;


// UserResult: Record of userAction > correct?, score in each section,
// History of quiz attempts by User
// [Who] + [Which question] + [Which choice was selected] = [UserResult (Answer result)]



[Index(nameof(UserId), Name = "IX_UserResult_UserId")]
[Index(nameof(QuestionId), Name = "IX_UserResult_QuestionId")]
[Index(nameof(AnsweredAt), Name = "IX_UserResult_AnsweredAt")]


// Composite index: queries like WHERE UserId = ? ORDER BY AnsweredAt DESC
[Index(nameof(UserId), nameof(AnsweredAt), Name = "IX_UserResult_UserId_AnsweredAt")]

// Composite index: queries like WHERE UserId = ? AND QuestionId = ? ORDER BY Id DESC (latest answer per question)
[Index(nameof(UserId), nameof(QuestionId), nameof(Id), Name = "IX_UserResult_UserId_QuestionId_Id")]
public class UserResult
{
    public int Id { get; set; }  // PK

    [Required]
    public string UserId { get; set; } = null!; // FK to the related User (Who answered the question)

    public int QuestionId { get; set; } // FK to the related Question (Which question user answered)

    public int AnswerOptionId { get; set; } // FK to the related AnswerOption (Which answerOption User selected)

    public bool IsCorrect { get; set; } // Is the user's answer correct?


    // Timestamp for when the answer was submitted
    public DateTime AnsweredAt { get; set; } = DateTime.UtcNow;


    // Navigation property for the related question (one question-many userResults)
    public Question Question { get; set; } = null!;

    // Navigation property for the related answerOption(one answerOption can be in many userResults)
    public AnswerOption AnswerOption { get; set; } = null!;

    // Navigation property for the related user (1 User-many UserResult)
    public AppUser User { get; set; } = null!;
}




