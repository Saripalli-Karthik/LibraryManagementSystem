using LibraryManagementAPI.Data;
using LibraryManagementAPI.DTOS;
using LibraryManagementAPI.Models;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

public class BorrowService : IBorrowService
{
    private readonly AppDbContext _db;

    public BorrowService(AppDbContext db)
    {
        _db = db;
    }

    public async Task<BorrowRecord?> BorrowBookAsync(BorrowDto dto, ClaimsPrincipal user)
    {
        var userId = user.FindFirstValue(ClaimTypes.NameIdentifier)!;

        if ((user.IsInRole("Admin") || user.IsInRole("Librarian")) && !string.IsNullOrEmpty(dto.UserId))
            userId = dto.UserId;

        var book = await _db.Books.FindAsync(dto.BookId);
        if (book == null || book.AvailableCopies <= 0)
            return null;

        var record = new BorrowRecord
        {
            BookId = dto.BookId,
            UserId = userId,
            BorrowedAt = DateTime.UtcNow,
            DueDate = DateTime.UtcNow.AddDays(14)
        };

        book.AvailableCopies--;
        _db.BorrowRecords.Add(record);
        await _db.SaveChangesAsync();

        return record;
    }

    public async Task<BorrowRecord?> GetByIdAsync(int id)
    {
        return await _db.BorrowRecords
            .Include(r => r.Book)
            .Include(r => r.Fine)
            .FirstOrDefaultAsync(r => r.Id == id);
    }

    public async Task<bool> ReturnBookAsync(ReturnDto dto, ClaimsPrincipal user)
    {
        var rec = await _db.BorrowRecords
            .Include(r => r.Book)
            .FirstOrDefaultAsync(r => r.Id == dto.BorrowId);

        if (rec == null || rec.ReturnedAt != null)
            return false;

        var userId = user.FindFirstValue(ClaimTypes.NameIdentifier)!;

        if (!user.IsInRole("Admin") && !user.IsInRole("Librarian") && rec.UserId != userId)
            return false;

        rec.ReturnedAt = DateTime.UtcNow;
        rec.Book.AvailableCopies++;

        if (rec.ReturnedAt > rec.DueDate)
        {
            var daysLate = (rec.ReturnedAt.Value - rec.DueDate).Days;
            if (daysLate > 0)
            {
                var fineAmount = Math.Min(daysLate * 1.00m, 200);
                _db.Fines.Add(new Fine
                {
                    BorrowRecordId = rec.Id,
                    Amount = fineAmount,
                    IssuedAt = DateTime.UtcNow
                });
            }
        }

        await _db.SaveChangesAsync();
        return true;
    }

    public async Task<List<BorrowRecordDto>> GetMyHistoryAsync(ClaimsPrincipal user)
    {
        var userId = user.FindFirstValue(ClaimTypes.NameIdentifier)!;

        return await _db.BorrowRecords
            .Where(br => br.UserId == userId)
            .Include(br => br.Book)
            .Select(br => new BorrowRecordDto
            {
                Id = br.Id,
                BookId = br.BookId,
                Title = br.Book.Title,
                Author = br.Book.Author,
                BorrowedAt = br.BorrowedAt,
                DueDate = br.DueDate
            })
            .ToListAsync();
    }
}
