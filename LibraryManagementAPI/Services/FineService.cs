using LibraryManagementAPI.Data;
using LibraryManagementAPI.DTOS;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryManagementAPI.Services
{
    public class FineService : IFineService
    {
        private readonly AppDbContext _context;

        public FineService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<object>> GetFinesAsync(string userId, bool isAdminOrLibrarian)
        {
            return await _context.Fines
                .Include(f => f.BorrowRecord)
                    .ThenInclude(br => br.Book)
                .Where(f => isAdminOrLibrarian || f.BorrowRecord.UserId == userId)
                .Select(f => new
                {
                    f.Id,
                    f.Amount,
                    f.IsPaid,
                    f.IssuedAt,
                    f.PaidAt,
                    BookTitle = f.BorrowRecord.Book.Title,
                    BorrowedAt = f.BorrowRecord.BorrowedAt,
                    DueDate = f.BorrowRecord.DueDate,
                    ReturnedAt = f.BorrowRecord.ReturnedAt
                })
                .ToListAsync();
        }

        public async Task<string> PayFineAsync(int fineId, string userId, bool isAdminOrLibrarian)
        {
            var fine = await _context.Fines
                .Include(f => f.BorrowRecord)
                .FirstOrDefaultAsync(f => f.Id == fineId);

            if (fine == null)
                return "NotFound";

            if (fine.IsPaid)
                return "AlreadyPaid";

            if (!isAdminOrLibrarian && fine.BorrowRecord.UserId != userId)
                return "Forbidden";

            fine.IsPaid = true;
            fine.PaidAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return "Success";
        }
    }
}
