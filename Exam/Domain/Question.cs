namespace Domain
{
    public class Question
    {
        public int QuestionId { get; set; }

        public string? Questions { get; set; }

        public int QuizId { get; set; }
        public Quiz Quiz { get; set; } = null!;

        public int AnswerId { get; set; }
        public Answers Answers { get; set; } = null!;
    }
}