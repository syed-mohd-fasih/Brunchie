using Brunchie.Data;
using Brunchie.Models;
using Microsoft.EntityFrameworkCore;

namespace Brunchie.Services
{
    public class VendorService
    {
        private readonly AppDbContext _appDbContext;

        public VendorService(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async Task<List<Menu>> GetMenus()
        {
            var menus = await _appDbContext.Menus.ToListAsync();
            return menus;
        }

        public async void AddMenu(Menu menu)
        {
            await _appDbContext.Menus.AddAsync(menu);
            await _appDbContext.SaveChangesAsync();
        }

        public async void RemoveMenu(Menu menu)
        {
            _appDbContext.Menus.Remove(menu);
            await _appDbContext.SaveChangesAsync();
        }

        public void UpdateMenu(Menu menu)
        {

        }

        public void AddMenuItem(Menu menu, MenuItem menuItem)
        {

        }

        public void RemoveMenuItem(Menu menu, MenuItem menuItem)
        {

        }

        public void UpdateMenuItem(Menu menu, MenuItem menuItem)
        {

        }
    }
}
