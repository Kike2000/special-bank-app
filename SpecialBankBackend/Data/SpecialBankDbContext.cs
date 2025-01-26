using Microsoft.EntityFrameworkCore;
using SpecialBankAPI.Models;

namespace SpecialBankAPI.Data
{
    public class SpecialBankDbContext : DbContext
    {
        public SpecialBankDbContext(DbContextOptions<SpecialBankDbContext> options): base (options)
        {
        
        }

        //Dbset
        public DbSet<Account> Account { get; set; }
        public DbSet<Transaction> Transaction { get; set; }
    }
}
