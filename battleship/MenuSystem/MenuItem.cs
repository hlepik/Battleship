using System;

namespace MenuSystem
{

    public class MenuItem
    {
        public virtual string Title { get; set; }
        public virtual string UserChoice { get; set; }

        public virtual Func<string> MethodToExecute { get; set; }

        public MenuItem(string title, string userChoice, Func<string> methodToExecute )
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




