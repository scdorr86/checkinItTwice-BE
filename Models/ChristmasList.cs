namespace checkinItTwice_BE.Models
{
    public class ChristmasList
    {
        public int Id { get; set; }
        public string ListName { get; set; }
        public int YearId { get; set; }
        public ChristmasYear ChristmasYear { get; set; }
        public int GifteeId { get; set; }
        public Giftee Giftee { get; set; }
        public List <Gift> Gifts { get; set; }
        public decimal ListTotal => Gifts?.Sum(g => g.Price) ?? 0;
        public int UserId { get; set; }
        public User User { get; set; }

    }
}
