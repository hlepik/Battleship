using System;
using System.Collections.Generic;

namespace Domain
{
    public class Quiz
    {
        public int QuizId { get; set; }

        public string? Title { get; set; }

        public int PlayerId { get; set; }
        public Player Players { get; set; } = null!;

        public ICollection<Question>? Questions { get; set; }


    }
}