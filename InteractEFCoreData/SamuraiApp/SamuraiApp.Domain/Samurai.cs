namespace SamuraiApp.Domain
{
    public class Samurai
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        //Avoid quotes to add it to the null list
        public List<Quote> Quotes { get; set; } = new List<Quote>();
        //Many to Many
        public List<Battle> Battles { get; set; } = null!;
        //one to one relationship
        public Horse Horse { get; set; }
    }
}