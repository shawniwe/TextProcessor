using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TextProcessor.Abstract;

namespace TextProcessor.Application.Menu
{
    public class ConsoleMenu : BaseMenu
    {
        public ConsoleMenu() : base() { }
        public ConsoleMenu(IEnumerable<BaseMenuItem> menuItems) : base(menuItems) { }
        public override void NextItem()
        {
            var index = MenuItems.ToList().IndexOf(CurrentItem);
            CurrentItem = MenuItems.ToList().Count - 1 == index ? MenuItems.First() : MenuItems.ElementAt(index + 1);
        }

        public override void PreviousItem()
        {
            var index = MenuItems.ToList().IndexOf(CurrentItem);
            CurrentItem = index - 1 < 0 ? MenuItems.Last() : MenuItems.ElementAt(index - 1);
        }

        public bool IsCurrentItem(BaseMenuItem item)
        {
            return item == CurrentItem;
        }
    }
}
