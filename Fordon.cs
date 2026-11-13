using System;
using System.Collections.Generic;
using System.Text;

namespace Kajens_Cykeluthyrning
{
    public abstract class Fordon
    {
        public int Nummer { get; set; }
        public decimal Timpris { get; set; }
        public bool Arledig { get; set; } = true;
        public string UthyrTillNamn { get; set; } = "";
        public int HyrtidTimmar { get; set; }
    }
}
