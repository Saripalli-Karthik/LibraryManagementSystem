using LibraryManagementAPI.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagementAPI.Data
{
    public class AppDbContext:IdentityDbContext<ApplicationUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Book> Books { get; set; }
        public DbSet<BorrowRecord> BorrowRecords { get; set; }
        public DbSet<Fine> Fines { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Book -> BorrowRecords
            builder.Entity<Book>()
                   .HasMany(b => b.BorrowRecords)
                   .WithOne(br => br.Book)
                   .HasForeignKey(br => br.BookId)
                   .OnDelete(DeleteBehavior.Restrict);

            // User -> BorrowRecords
            builder.Entity<ApplicationUser>()
                   .HasMany(u => u.BorrowRecords)
                   .WithOne(br => br.User)
                   .HasForeignKey(br => br.UserId)
                   .OnDelete(DeleteBehavior.Restrict);

            // BorrowRecord -> Fine
            builder.Entity<BorrowRecord>()
                   .HasOne(br => br.Fine)
                   .WithOne(f => f.BorrowRecord)
                   .HasForeignKey<Fine>(f => f.BorrowRecordId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
