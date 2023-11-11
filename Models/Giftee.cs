namespace checkinItTwice_BE.Models
{
    public class Giftee
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public List <ChristmasList> ChristmasLists { get; set; }
        public int NumOfLists => ChristmasLists.Count;
        public int UserId { get; set; }
        public User User { get; set; }
    }
}
