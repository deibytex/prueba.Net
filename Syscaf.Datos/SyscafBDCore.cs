using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Syscaf.Data.Models.Auth;

namespace Syscaf.Data
{
    public class SyscafBDCore : IdentityDbContext<ApplicationUser>
    {

        public SyscafBDCore(DbContextOptions options) : base(options)
        {
        }

    }
}
