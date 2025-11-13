using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CoffeeManagement.DAL.Models;

namespace CoffeeManagement.DAL.DAO
{
    public interface IMenuItemsDAO
    {
        IEnumerable<MenuItem> GetAllMenuItems();
        MenuItem CreateMenuItems(MenuItem menuItem);
        MenuItem? GetMenuItemsById(int id);
        bool DeleteMenuItems(int id);
        void UpdateMenuItems(MenuItem menuItem);
        IEnumerable<Category> GetAllCategories();
    }
}
