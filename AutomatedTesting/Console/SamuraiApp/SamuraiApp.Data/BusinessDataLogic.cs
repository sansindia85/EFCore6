using SamuraiApp.Domain;

namespace SamuraiApp.Data
{
    public class BusinessDataLogic
    {
        private SamuraiContext _context;

        public BusinessDataLogic(SamuraiContext context)
        {
            _context = context;
        }

        public BusinessDataLogic()
        {
            _context = new SamuraiContext();
        }

        public int AddSamurasiByName(params string[] names)
        {
            foreach (string name in names)
            {
                _context.Samurais.Add(new Samurai { Name = name });
            }
            var dbResult = _context.SaveChanges();
            return dbResult;                
        }

        public int InsertNewSamurai(Samurai samurai)
        {
            _context.Samurais.Add(samurai);
            var dbResult = _context.SaveChanges();
            return dbResult;
        }
    }
}
