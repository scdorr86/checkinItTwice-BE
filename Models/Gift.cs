namespace checkinItTwice_BE.Models
{
    public class Gift
    {
        public int Id { get; set; }
        public string GiftName { get; set; }
        public decimal Price { get; set; }
        public string ImageUrl { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
        public List<ChristmasList> ChristmasLists { get; set; }
        public string OrderedFrom { get; set; }
    }
}
