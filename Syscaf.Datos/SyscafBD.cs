using Microsoft.EntityFrameworkCore;
namespace Syscaf.Data
{
    public class SyscafBD : DbContext
    {
        public SyscafBD() { }
        public SyscafBD(DbContextOptions<SyscafBD> options) : base(options) { }
    }
}
