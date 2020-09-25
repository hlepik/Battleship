using System;
using System.Collections.Generic;

namespace MenuSystem
{
    public class MenuItem
    {
        public virtual string? UserChoice { get; set; }
        public string? Title { get; set; }

        public virtual Action? MethodToExecute { get; set; }


        public override string ToString()
        {
            return $"{UserChoice} {Title}";
        }


    }
}



