using System.Collections.Generic;

namespace Domain
{
    public class Player
    {
        public int PlayerId { get; set; }
        public string? Name { get; set; }

        public int Count { get; set; }

        public ICollection<Quiz> Quizzes { get; set; } = null!;


    }
}