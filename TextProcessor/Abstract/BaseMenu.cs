using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TextProcessor.Abstract.Interfaces;

namespace TextProcessor.Abstract
{
    public delegate void CurrentItemSelected();
    public abstract class BaseMenu : IMenu
    {
        public BaseMenu()
        {
            MenuItems = new List<BaseMenuItem>();
        }

        public BaseMenu(IEnumerable<BaseMenuItem> menuItems)
        {
            MenuItems = menuItems;
        }

        public CurrentItemSelected OnCurrentItemSelected;
        public virtual BaseMenuItem CurrentItem { get; protected set; }
        public virtual IEnumerable<BaseMenuItem> MenuItems { get; protected set; }
        public void RemoveMenuItem(BaseMenuItem menuItem)
        {
            MenuItems.ToList().Remove(menuItem);
        }
        public void AddMenuItem(BaseMenuItem menuItem)
        {
            MenuItems.ToList().Add(menuItem);
            if (!MenuItems.Any())
                CurrentItem = menuItem;
        }

        public abstract void NextItem();
        public abstract void PreviousItem();
    }
}