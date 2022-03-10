namespace SamuraiApp.Domain
{
    public class Battle
    {
        public int BattleId { get; set; }
        public string Name { get; set; } = null!;
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        //Many to Many
        public List<Samurai> Samurais { get; set; } = new List<Samurai>();
    }
}
