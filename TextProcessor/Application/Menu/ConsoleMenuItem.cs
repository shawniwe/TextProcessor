using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TextProcessor.Abstract;

namespace TextProcessor.Application.Menu
{
    public class ConsoleMenuItem : BaseMenuItem
    {
        public ConsoleMenuItem() { }

        public ConsoleMenuItem(string name)
        {
            Name = name;
        }

        public ConsoleColor SelectedForegroundColor { get; set; } = ConsoleColor.White;
        public ConsoleColor SelectedBackgroundColor { get; set; }
    }
}
