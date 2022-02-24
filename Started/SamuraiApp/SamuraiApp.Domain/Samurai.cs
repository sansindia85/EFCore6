namespace SamuraiApp.Domain
{
    public class Samurai
    {
        public int Id { get; set; }
        public string Name { get; set; }

        //Avoid quotes to add it to the null list
        public List<Quote> Quotes { get; set; } = new List<Quote>();
    }
}