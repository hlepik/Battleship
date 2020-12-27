using System.Collections.Generic;

namespace Domain
{
    public class Answers
    {
        public int AnswersId { get; set; }
        public string? Answer { get; set; }
        public bool IsAnswerCorrect { get; set; }

        public int QuestionId { get; set; }
        public Question? Question { get; set; }



    }

}
