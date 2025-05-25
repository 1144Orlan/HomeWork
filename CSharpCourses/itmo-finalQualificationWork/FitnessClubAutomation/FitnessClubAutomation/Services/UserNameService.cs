using FitnessClubAutomation.Data;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace FitnessClubAutomation.Services
{
    public class UserNameService
    {
        private readonly ApplicationDbContext _context;

        public UserNameService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<string> GetUserDisplayNameAsync(string email)
        {
            if (string.IsNullOrEmpty(email))
                return "User";
                        
            var client = await _context.Clients
                .FirstOrDefaultAsync(c => c.Email == email);

            if (client != null && !string.IsNullOrEmpty(client.FullName))
                return client.FullName;
                        
            var staff = await _context.Staff
                .FirstOrDefaultAsync(s => s.Email == email);

            if (staff != null && !string.IsNullOrEmpty(staff.FullName))
                return staff.FullName;
                        
            return email;
        }
    }
}