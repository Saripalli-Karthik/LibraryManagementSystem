namespace LibraryManagementAPI.DTOS
{
    public class BorrowRecordDto
    {
        public int Id { get; set; }
        public int BookId { get; set; }

        public string Title { get; set; }           // Book Title
        public string Author { get; set; }          // Book Author

        public DateTime BorrowedAt { get; set; }
        public DateTime DueDate { get; set; }
    }
}
