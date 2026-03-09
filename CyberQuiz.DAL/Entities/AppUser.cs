
//==================================================================================
// For Identity User in case of seeding from Server-side
//==================================================================================




//using Microsoft.AspNetCore.Identity;
//using System.ComponentModel.DataAnnotations;

//namespace CyberQuiz.DAL.Entities;

//// One User : many Results
//public class AppUser : IdentityUser
//{
//    [MaxLength(200)]
//    public string FullName { get; set; } = string.Empty;
//    public DateTime LastLogIn { get; set; }
//    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
//    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
//    public bool IsActive { get; set; } = true; // If account is active or deactivated (soft delete)
//    public int TotalQuizzesTaken { get; set; } // How many quizzes the user has taken
//    public int HighestScore { get; set; }  // The highest score the user has achieved across all quizzes

//    [Url, MaxLength(1000)]
//    public string? ProfilePictureUrl { get; set; } // URL to the user's profile picture


//    // Navigation property for the related user results (one user-many results)
//    public ICollection<UserResult> Results { get; set; } = new List<UserResult>();
//}
