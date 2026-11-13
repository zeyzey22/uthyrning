namespace Kajens_Cykeluthyrning
{
    class Program
    {
        static void Main(string[] args)
        {
            // listor för fordon och kunder
            List<Fordon> fordonLista = new List<Fordon>();
            List<Kund> kundLista = new List<Kund>();

            // starta med minst två fordon av varje typ
            fordonLista.Add(new Cykel(1));
            fordonLista.Add(new Cykel(2));
            fordonLista.Add(new Elcykel(3, 80));
            fordonLista.Add(new Elcykel(4, 25));
          
            bool KorProgram = true;

            // huvudlopp för menyn
            while (KorProgram)
            {
                Console.WriteLine("=== Kajens Cykeluthyrning ===");
                Console.WriteLine("1. Visa fordon 2. Registerera kund 3. Hyr ut ");
                Console.WriteLine("4. Lämna tillbaka  5. Ladda  6.  Avsluta");
                Console.WriteLine("Val:");

                string val = Console.ReadLine();

                // kör funktion beroende på val
                switch (val)
                {
                    case "1":
                        VisaFordon(fordonLista);
                        break;
                    case "2":
                        RegistreraKund(kundLista);
                        break;
                    case "3":
                        HyrUt(fordonLista, kundLista);
                        break;
                    case "4":
                        LamnaTillbaka(fordonLista, kundLista);
                        break;
                    case "5":
                        LaddaElcykel(fordonLista);
                        break;
                    case "6":
                        KorProgram = false;
                        Console.WriteLine("programmet avslutas.");
                        break;

                }
            }
        }

          // visar alla fordon och status
         static void VisaFordon(List<Fordon> fordonLista)
        {
            Console.WriteLine("Nr  Typ   Status  Batteri");
            foreach (var fordon in fordonLista)
            {
                string typ = fordon is Elcykel ? "Elcykel" : "Cykel";
                string status = fordon.Arledig ? "ledig" : "uthyrd";
                string batteri = fordon is Elcykel elcykel ? $"{elcykel.BatteriNiva} %" : "";

               Console.WriteLine($"{fordon.Nummer,-3} {typ,-8} {status,-7} {batteri}");
            }
        }

        static void RegistreraKund(List<Kund> KundLista)
        {
            Console.Write("Namn:");
            string namn = Console.ReadLine();

            Console.Write("Ålder:");
            if (!int.TryParse(Console.ReadLine(), out int alder))
            {
                Console.WriteLine("Fel ålder");
                return;
            }

            Console.Write("Medlem (j/n): ");
            bool arMedlem = Console.ReadLine().ToLower() == "j";

            KundLista.Add(new Kund(namn, alder, arMedlem));
            Console.WriteLine($"{namn} är registrerad.");
        }

        static void HyrUt(List<Fordon> fordonList, List<Kund> KundLista)
        {
            Console.Write("Kund: ");
            string KundNamn = Console.ReadLine();
            Kund kund = KundLista.Find(K => K.Namn.Equals(KundNamn, StringComparison.OrdinalIgnoreCase));
            
            if (kund == null)
            {
                Console.WriteLine("kunden finns inte.");
                return;
            }

            Console.Write("Fordonsnummer: ");
            if (!int.TryParse(Console.ReadLine(), out int fordonsNummer))
            {
                Console.WriteLine("Fel nummer");
                return;
            }

            Fordon fordon = fordonList.Find(f => f.Nummer == fordonsNummer);
            if (fordon == null)
            {
                Console.WriteLine("Fordonet finns inte.");
                return;
            }

            if (!fordon.Arledig)
            {
                Console.WriteLine("Fordonet är redan uthyrt.");
                return;
            }

            if (fordon is Elcykel elcykel)
            {
                if (kund.Alder < 18)
                {
                    Console.WriteLine("Minst 18 år för elcykel.");
                    return;
                }
                if (elcykel.BatteriNiva < 30)
                {
                    Console.WriteLine($"Elcyckel {elcykel.Nummer} har för lite batteri ({elcykel.BatteriNiva} %). Ladda den först.");
                    return;
                }
            }
            else if (fordon is Cykel)
            {
                if (kund.Alder < 15)
                {
                    Console.WriteLine("Minst 15 år för cykel.");
                    return;
                }
            }
            Console.Write("Antal timmar: ");
            if (!int.TryParse(Console.ReadLine(), out int timmar) || timmar <= 0)
            {
                Console.WriteLine("Fel timmar");
                return;
            }
            // spara uthyrningen i fordenet
            fordon.Arledig = false;
           fordon. UthyrTillNamn = kund.Namn;
            fordon.HyrtidTimmar = timmar;


            string typ = fordon is Elcykel ? "Elcykel" : "Cykel";
            Console.WriteLine($"{kund.Namn} hyr {typ} {fordon.Nummer} i {timmar} timmar.");

        }

        static void LamnaTillbaka(List<Fordon> fordonLista, List<Kund>kundLista)
        {
            Console.Write("Fordonsnummer:");
            if (!int.TryParse(Console.ReadLine(), out int fordonsNummer))
            {
                Console.WriteLine("Fel nummer");
                return;

            }

            Fordon fordon = fordonLista.Find(f => f.Nummer == fordonsNummer);
            if (fordon == null || fordon.Arledig)
            {
                Console.WriteLine("Fordonet är inte uthyrt eller finns int.");
                return;
            }

            Kund kund = kundLista.Find(k => k.Namn.Equals(fordon.UthyrTillNamn, StringComparison.OrdinalIgnoreCase));
            int timmar = fordon.HyrtidTimmar;

            decimal brutto = fordon.Timpris * timmar;
            decimal rabatt = (kund != null && kund.ArMedlem) ? brutto * 0.10m : 0;
            decimal attBetala = brutto - rabatt;
            

             // dra batteri för elcykel 10% per timme
            if (fordon is Elcykel elcykel)
            {
                elcykel.BatteriNiva -= (timmar * 10);
                if (elcykel.BatteriNiva < 0) elcykel.BatteriNiva = 0;
            }
            
            string typ = fordon is Elcykel ? "Elcykel" : "Cykel";
            // gör fordonet ledigt
            fordon.Arledig = true;
            fordon.UthyrTillNamn = "";
            fordon.HyrtidTimmar = 0;


            Console.WriteLine("\nKVITTO");
            Console.WriteLine($"{typ}, {timmar} timmar à {fordon.Timpris} kr   {brutto,8:F2} kr");

            if (kund.ArMedlem)
            {
                Console.WriteLine($"Medlemmsrabatt (10 %)        -{rabatt,8:F2} kr");
            }

            Console.WriteLine($"Att betala                      {attBetala,8:F2} kr");

        }

        // laddar batteriet på elcykel till 100%
        static void LaddaElcykel(List<Fordon> fordonLista)
        {
            Console.WriteLine("FordonsNummer: ");
            if (!int.TryParse(Console.ReadLine(), out int fordonsNummer))
            {
                Console.WriteLine("Fel nummer.");
                return;

            }

            Fordon fordon = fordonLista.Find(f => f.Nummer == fordonsNummer);

            if (fordon is Elcykel elcykel)
            {
                elcykel.BatteriNiva = 100;
                Console.WriteLine($"Elcykel {elcykel.Nummer} är nu laddad till 100 %. ");
            }
            else if (fordon is Cykel)
            {
                Console.WriteLine("Vanliga cyklar har inget batteri.");
            }
            else
            {
                Console.WriteLine("Fordonet hittades inte.");
            }

        }
    }
}


            























