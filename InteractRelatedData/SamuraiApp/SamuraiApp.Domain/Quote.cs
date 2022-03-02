using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SamuraiApp.Domain
{
    public class Quote
    {
        public int Id { get; set; }
        public string Text { get; set; } = null!;

        //Reference property back to Samurai
        public Samurai Samurai { get; set; } = null!;

        //Explicit integer key that contains the foreign value
        public int SamuraiId { get; set; }
    }
}
