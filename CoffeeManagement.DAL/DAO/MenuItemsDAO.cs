using CoffeeManagement.DAL.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoffeeManagement.DAL.DAO
{
    public class MenuItemsDAO : IMenuItemsDAO
    {
        private readonly CoffeeManagementDbContext _context;

        public MenuItemsDAO(CoffeeManagementDbContext db)
        {
            _context = db ?? throw new ArgumentNullException(nameof(db));
        }
        public MenuItem CreateMenuItems(MenuItem menuItem)
        {
            if (menuItem == null) throw new ArgumentNullException(nameof(menuItem));
            _context.MenuItems.Add(menuItem);
            _context.SaveChanges();
            return menuItem;
        }

        public bool DeleteMenuItems(int id)
        {
            var item = _context.MenuItems.Find(id);
            if (item == null) throw new InvalidOperationException("Menu item not found.");
            _context.MenuItems.Remove(item);
            _context.SaveChanges();
            return true;
        }

        public IEnumerable<MenuItem> GetAllMenuItems()
        {
            List<MenuItem> menuItems = _context.MenuItems.Include("Category").ToList();
            return menuItems;
        }

        public MenuItem? GetMenuItemsById(int id)
        {
            return _context.MenuItems.Include("Category").FirstOrDefault(mi => mi.Id == id);
        }

        public void UpdateMenuItems(MenuItem menuItem)
        {
            if(menuItem == null) throw new ArgumentNullException(nameof(menuItem));
            var existing = _context.MenuItems.FirstOrDefault(mi => mi.Id == menuItem.Id);
            if (existing == null) throw new InvalidOperationException("Menu item not found");
            existing.Name = menuItem.Name;
            existing.Description = menuItem.Description;
            existing.Price = menuItem.Price;
            existing.CategoryId = menuItem.CategoryId;
            existing.ImgUrl = menuItem.ImgUrl;
            existing.IsAvailable = menuItem.IsAvailable;

            _context.SaveChanges();

        }
        public IEnumerable<Category> GetAllCategories()
        {
            return _context.Categories.ToList();
        }
    }
}
