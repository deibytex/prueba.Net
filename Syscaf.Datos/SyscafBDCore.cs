using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
namespace Syscaf.Data
{
    public class SyscafBDCore : IdentityDbContext
    {

        public SyscafBDCore(DbContextOptions options) : base(options)
        {
        }

    }
}
