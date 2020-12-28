using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Domain
{
    public class Quiz
    {
        public int QuizId { get; set; }

        [MaxLength(64)]
        public string? Title { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;


        public ICollection<Player>? Players { get; set; }
        public ICollection<Question>? Questions { get; set; }


    }
}