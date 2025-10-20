using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System;
using Microsoft.EntityFrameworkCore;

namespace PROG6212_POE.Models
{
    public class ApplicationDbContext
    {
        public DbSet<User> User { get; set; } //Table for Venue
        public DbSet<Claim> Claim { get; set; } //Table for Event
        public DbSet<Lecturer> Lecturer { get; set; } //Table for Booking
        public DbSet<SupportingDocument> SupportingDocument { get; set; } //Table for EventType
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
        { }

    }
}
/* Reference List:
IIEVC School of Computer Science.2025.Getting Started with MVC, Entity Framework, and SQL Azure Database for CLDV6211 Part 1.[video online].Available at: https://youtu.be/FaAVkUH2qSw?si=O6MhHU8AjR-xz_t9 .[Accessed  16 October 2025].
*/