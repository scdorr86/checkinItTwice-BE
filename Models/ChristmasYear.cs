namespace checkinItTwice_BE.Models
{
    public class ChristmasYear
    {
        public int Id { get; set; }
        public string ListYear { get; set; }
        public decimal YearBudget { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
        public List <ChristmasList> ChristmasLists { get; set; }
        public decimal ListsTotal => ChristmasLists?.Sum(l => l.ListTotal) ?? 0;
    }
}
