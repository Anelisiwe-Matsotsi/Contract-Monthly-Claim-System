using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System;
using Microsoft.EntityFrameworkCore;

namespace PROG6212_POE.Models
{ /*IIEVC, 2025*/
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        { }

        public DbSet<Lecturer> Lecturer { get; set; }
        public DbSet<Claim> Claim { get; set; }
        public DbSet<SupportingDocument> SupportingDocument { get; set; }
        public DbSet<Approval> Approval { get; set; }
        public DbSet<Account> Account { get; set; }
    }
}
/* Reference List:
IIEVC School of Computer Science.2025.Getting Started with MVC, Entity Framework, and SQL Azure Database for CLDV6211 Part 1.[video online].Available at: https://youtu.be/FaAVkUH2qSw?si=O6MhHU8AjR-xz_t9 .[Accessed  16 October 2025].
*/