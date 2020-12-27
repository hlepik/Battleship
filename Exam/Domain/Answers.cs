using System.Collections.Generic;

namespace Domain
{
    public class Answers
    {
        public int AnswersId { get; set; }
        public string? Answer { get; set; }
        public EChoices EChoices { get; set; }

        public ICollection<Question> Questions { get; set; } = null!;


    }

}
