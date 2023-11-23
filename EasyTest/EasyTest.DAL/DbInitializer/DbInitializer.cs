using EasyTest.DAL.Entities;
using EasyTest.Shared.Constants;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace EasyTest.DAL.DbInitializer
{
    public class DbInitializer : IDbInitializer
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole<Guid>> _roleManager;
        private readonly ApplicationDbContext _context;

        public DbInitializer(UserManager<User> userManager, RoleManager<IdentityRole<Guid>> roleManager, ApplicationDbContext context)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _context = context;
        }

        public void Initialize()
        {
            try
            {
                if (_context.Database.GetPendingMigrationsAsync().GetAwaiter().GetResult().Count() > 0)
                {
                    _context.Database.Migrate();
                }
            }
            catch (Exception)
            {
                throw;
            }

            if (!_roleManager.RoleExistsAsync(UserRoles.AdminRole).GetAwaiter().GetResult())
            {
                _roleManager.CreateAsync(new IdentityRole<Guid>(UserRoles.AdminRole)).GetAwaiter().GetResult();
                _roleManager.CreateAsync(new IdentityRole<Guid>(UserRoles.TeacherRole)).GetAwaiter().GetResult();
                _roleManager.CreateAsync(new IdentityRole<Guid>(UserRoles.StudentRole)).GetAwaiter().GetResult();
            }

            return;
        }
    }
}
