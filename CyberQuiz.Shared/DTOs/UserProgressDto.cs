using System;
using System.Collections.Generic;
using System.Text;

namespace CyberQuiz.Shared.DTOs
{
    //För att visa generell överblick av progress 
    public class UserProgressDto
    {
        public int TotalSubCategories { get; set; }
        public int CompletedSubCategories { get; set; }
        public double OverallPercent { get; set; }
        public int TotalCategories { get; set; }
        public int CompletedCategories { get; set; }
    }
}
