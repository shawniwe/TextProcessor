using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TextProcessor.Abstract;

namespace TextProcessor.Application.Menu
{
    public delegate void KeyboardKeyPress(ConsoleKey key);
    // класс, который принимает нажатие с клавиатуры и решает, что должно произойти в меню
    public class ConsoleMenuController
    {
        private readonly BaseMenu menu;
        public KeyboardKeyPress OnKeyPressed;

        public ConsoleMenuController(BaseMenu menu)
        {
            this.menu = menu;
            OnKeyPressed += MenuController_OnKeyPressed;
        }

        private void MenuController_OnKeyPressed(ConsoleKey key)
        {
            switch (key)
            {
                case ConsoleKey.Enter:
                    menu.OnCurrentItemSelected?.Invoke();
                    break;
                case ConsoleKey.UpArrow:
                    menu.PreviousItem();
                    break;
                case ConsoleKey.DownArrow:
                    menu.NextItem();
                    break;
                default:
                    break;
            }
        }
    }
}