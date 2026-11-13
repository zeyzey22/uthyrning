using System;
using System.Collections.Generic;
using System.Text;

namespace Kajens_Cykeluthyrning
{
    public class Elcykel : Fordon
    {
        public int BatteriNiva { get; set; }
        
        public Elcykel(int nummer, int batteriNiva)
        {
            Nummer = nummer;
            Timpris = 70.00m;
            BatteriNiva = batteriNiva;
        }
    }
}
