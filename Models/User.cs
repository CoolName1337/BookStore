namespace BookStore.Models
{
    public class User
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public DateTime? CreatedDate { get; set; } = DateTime.Now;
        public decimal Balance { get; set; }
        public List<Book> AvailableBooks { get; set; } = new();

    }
}
