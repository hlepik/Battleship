using System.Collections.Generic;

namespace Domain
{
    public class Answer
    {
        public int AnswerId { get; set; }
        public string? QuestionAnswer { get; set; }
        public bool IsAnswerCorrect { get; set; }

        public int QuestionId { get; set; }
        public Question? Question { get; set; }



    }

}
