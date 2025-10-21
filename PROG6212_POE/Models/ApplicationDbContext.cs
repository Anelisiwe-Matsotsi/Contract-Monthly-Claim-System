using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System;
using Microsoft.EntityFrameworkCore;

namespace PROG6212_POE.Models
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<AppUser> AppUser { get; set; } //Table for User
        public DbSet<Claim> Claim { get; set; } //Table for Claim
        public DbSet<Lecturer> Lecturer { get; set; } //Table for Lecturer
        public DbSet<SupportingDocument> SupportingDocument { get; set; } //Table for SupportingDocument
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
        { }

    }
}
/* Reference List:
IIEVC School of Computer Science.2025.Getting Started with MVC, Entity Framework, and SQL Azure Database for CLDV6211 Part 1.[video online].Available at: https://youtu.be/FaAVkUH2qSw?si=O6MhHU8AjR-xz_t9 .[Accessed  16 October 2025].
*/