using SpecialBankAPI.Data;
using SpecialBankAPI.Models;

namespace SpecialBankAPI.Services
{
    public class JwtService
    {
        private readonly SpecialBankDbContext _context;
        private readonly IConfiguration _configuration;

        public JwtService(SpecialBankDbContext dbContext, IConfiguration configuration)
        {
            _context = dbContext;
            _configuration = configuration;            
        }

    }
}
