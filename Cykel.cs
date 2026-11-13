using System;
using System.Collections.Generic;
using System.Text;

namespace Kajens_Cykeluthyrning
{
    public class Cykel : Fordon
    {
        public Cykel(int nummer)
        {
            Nummer = nummer;
            Timpris = 40.00m;
        }
        
       
    }
}
