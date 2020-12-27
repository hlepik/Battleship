using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Domain
{
    public class Question
    {
        public int QuestionId { get; set; }

        [MaxLength(500)]
        public string? Questions { get; set; }

        public int QuizId { get; set; }
        public Quiz? Quiz { get; set; }

        public ICollection<Answers>? Answer { get; set; }

    }
}
