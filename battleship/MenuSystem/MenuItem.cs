using System;

namespace MenuSystem
{

    public sealed class MenuItem
    {

        public string Title { get; set; }
        public string UserChoice { get; set; }

        public Func<string>? MethodToExecute { get; set; }

        public MenuItem(string title, string userChoice, Func<string>? methodToExecute)
        {
            Title = title.Trim();
            UserChoice = userChoice.Trim();
            MethodToExecute = methodToExecute;
        }

        public override string ToString()
        {
            return $"{UserChoice} {Title}";
        }

    }
}




