using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;


namespace CasinoConsoleApp
{
    class SessionsContext : DbContext
    {
        public SessionsContext(DbContextOptions<SessionsContext> options)
            :base(options)
        {

        }

        public DbSet<Session> Sessions { get; set; }
        public DbSet<Client> Clients { get; set; }

    }
}
