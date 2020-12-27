using System.Collections.Generic;

namespace Domain
{
    public class PlayerAnswer
    {
        public int PlayerAnswerId { get; set; }

        public int CorrectAnswersCount { get; set; }
        public int Count { get; set; }

        public int PlayerId { get; set; }
        public Player Player { get; set; } = null!;
    }
}