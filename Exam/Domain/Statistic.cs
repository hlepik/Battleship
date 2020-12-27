using System.Runtime.InteropServices;

namespace Domain
{
    public class Statistic
    {
        public int StatisticId { get; set; }

        public string? QuizName { get; set; }
        public int Count { get; set; }
        public double CorrectAnswers { get; set; }

        public int QuizId { get; set; }
        public Quiz Quiz { get; set; } = null!;
    }
}