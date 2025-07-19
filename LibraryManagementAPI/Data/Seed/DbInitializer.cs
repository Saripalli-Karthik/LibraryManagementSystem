// File: Data/Seed/DbInitializer.cs
using LibraryManagementAPI.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagementAPI.Data.Seed
{
    public static class DbInitializer
    {
        public static async Task SeedAsync(AppDbContext context, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            await context.Database.MigrateAsync();

            await SeedRolesAsync(roleManager);
            await SeedUsersAsync(userManager);
            await SeedBooksAndBorrowRecordsAsync(context, userManager);
        }
        private static async Task SeedRolesAsync(RoleManager<IdentityRole> roleManager)
        {
            string[] roles = new[] { "Admin", "Librarian", "Student" };

            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    await roleManager.CreateAsync(new IdentityRole(role));
                }
            }
        }
        private static async Task SeedUsersAsync(UserManager<ApplicationUser> userManager)
        {
            var adminEmail = "admin@library.com";
            var adminUser = await userManager.FindByEmailAsync(adminEmail);

            if (adminUser == null)
            {
                adminUser = new ApplicationUser
                {
                    UserName = adminEmail,
                    Email = adminEmail,
                    FullName = "System Admin"
                };
                await userManager.CreateAsync(adminUser, "Admin@123");
                await userManager.AddToRoleAsync(adminUser, "Admin");
            }

            var studentEmail = "student@library.com";
            var studentUser = await userManager.FindByEmailAsync(studentEmail);

            if (studentUser == null)
            {
                studentUser = new ApplicationUser
                {
                    UserName = studentEmail,
                    Email = studentEmail,
                    FullName = "Student One"
                };
                await userManager.CreateAsync(studentUser, "Student@123");
                await userManager.AddToRoleAsync(studentUser, "Student");
            }
            //seeding Librarian
            var librarianEmail = "librarian@library.com";
            var librarianUser = await userManager.FindByEmailAsync(librarianEmail);

            if (librarianUser == null)
            {
                librarianUser = new ApplicationUser
                {
                    UserName = librarianEmail,
                    Email = librarianEmail,
                    FullName = "Library Manager"
                };
                await userManager.CreateAsync(librarianUser, "Librarian@123");
                await userManager.AddToRoleAsync(librarianUser, "Librarian");
            }

        }
        private static async Task SeedBooksAndBorrowRecordsAsync(AppDbContext context, UserManager<ApplicationUser> userManager)
        {
            var booksToSeed = new[]
               {
                    new Book { Title = "Clean Code", Author = "Robert Martin", ISBN = "9780132350884", PublishedYear = 2008 },
                    new Book { Title = "Pragmatic Programmer", Author = "Andrew Hunt", ISBN = "9780201616224", PublishedYear = 1999 },
                    new Book { Title = "Intro to Algorithms", Author = "Cormen", ISBN = "9780262033848", PublishedYear = 2009 }
                };

            foreach (var b in booksToSeed)
            {
                if (!await context.Books.AnyAsync(x => x.ISBN == b.ISBN))
                {
                    context.Books.Add(b);
                }
            }

            await context.SaveChangesAsync();

            if (!context.BorrowRecords.Any())
            {
                var student = await userManager.FindByEmailAsync("student@library.com");
                var firstBook = await context.Books.FirstOrDefaultAsync();

                if (firstBook != null && student != null)
                {
                    var borrowRecord = new BorrowRecord
                    {
                        BookId = firstBook.Id,
                        UserId = student.Id,
                        BorrowedAt = DateTime.UtcNow.AddDays(-3),
                        DueDate = DateTime.UtcNow.AddDays(7)
                    };

                    context.BorrowRecords.Add(borrowRecord);
                    await context.SaveChangesAsync();
                }
            }

            if (!context.Fines.Any())
            {
                var overdueBook = await context.Books.Skip(1).FirstOrDefaultAsync(); // second book
                var student = await userManager.FindByEmailAsync("student@library.com");

                if (overdueBook != null && student != null)
                {
                    var returnedLate = new BorrowRecord
                    {
                        BookId = overdueBook.Id,
                        UserId = student.Id,
                        BorrowedAt = DateTime.UtcNow.AddDays(-15),
                        DueDate = DateTime.UtcNow.AddDays(-7),
                        ReturnedAt = DateTime.UtcNow.AddDays(-2) // returned late by 5 days
                    };

                    context.BorrowRecords.Add(returnedLate);
                    await context.SaveChangesAsync();

                    // Fine logic (e.g., $1 per day)
                    var fine = new Fine
                    {
                        BorrowRecordId = returnedLate.Id,
                        Amount = 5m, // 5 days late × $1
                        IsPaid = false
                    };

                    context.Fines.Add(fine);
                    await context.SaveChangesAsync();
                }
            }

        }

    }
}
