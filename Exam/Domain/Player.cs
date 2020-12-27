using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Domain
{
    public class Player
    {
        public int PlayerId { get; set; }
        [MaxLength(64)]
        public string? Name { get; set; }


        public int QuizId { get; set; }
        public Quiz? Quiz { get; set; }
        public ICollection<PlayerAnswer>? PlayerAnswers { get; set; }


    }
}