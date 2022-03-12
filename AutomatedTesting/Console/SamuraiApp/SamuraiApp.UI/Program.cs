using SamuraiApp.Data;

namespace SamuraiApp.UI
{
    //This console app is "demo-ware", not architectural "best practices".
    internal class Program
    {
        private static SamuraiContext _context = new SamuraiContext();

        private static void Main(string[] args)
        {
            AddSamuraisByName("Shimada", "Okamoto", "Kikuchio", "Hayashida");
        }

        private static void AddSamuraisByName(params string[] names)
        {
           var businessData = new BusinessDataLogic();
           var newSamuraisCreatedCount = businessData.AddSamurasiByName(names);
        }
    }
} 

