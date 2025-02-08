using System.Diagnostics;
using System.IO.Enumeration;
using System.Net.Http.Headers;
using System.Xml.Linq;

namespace MJU23v_DTP_T2
{
    internal class Program
    {
        static List<Link> links = new List<Link>();
        class Link
        {
            public string category, group, name, descr, link;
            public Link(string category, string group, string name, string descr, string link)
            {
                this.category = category;
                this.group = group;
                this.name = name;
                this.descr = descr;
                this.link = link;
            }

            public Link(string line)
            {
                string[] part = line.Split('|');
                category = part[0];
                group = part[1];
                name = part[2];
                descr = part[3];
                link = part[4];
            }
            public void DisplayDetails(int index)
            {
                Console.WriteLine($"|{index,-2}|{category,-10}|{group,-10}|{name,-20}|{descr,-40}|");
            }
            public void LaunchLink()
            {
                Process application = new Process();
                application.StartInfo.UseShellExecute = true;
                application.StartInfo.FileName = link;
                application.Start();
            }
            public override string ToString()
            {
                // FIXME: Åsidosatt 'ToString'-metod för att säkerställa korrekt formatering.
                return $"{category}|{group}|{name}|{descr}|{link}";
            }
        }
        static void Main(string[] args)
        {
            string filename = Path.Combine("..", "..", "..", "links", "links.lis");
            LoadLinks(filename, links);

            Console.WriteLine("Välkommen till länklistan! Skriv 'hjälp' för hjälp!");
            do
            {
                Console.Write("> ");
                string cmd = Console.ReadLine().Trim();
                string[] arg = cmd.Split();
                string command = arg[0];

                switch (command)
                {
                    case "sluta":
                        Console.WriteLine("Hej då! Välkommen åter!");
                        break;
                    case "hjälp":
                        PrintHelp();

                        break;
                    case "ladda":
                        if (arg.Length == 2)
                        {
                            filename = Path.Combine("..", "..", "..", "links", arg[1]);
                        }
                        LoadLinks(filename, links);

                        break;
                    case "lista":
                        ListLinks();

                        break;
                    case "ny":
                        CreateNewLink();

                        break;
                    case "spara":
                        if (arg.Length == 2)
                        {
                            filename = Path.Combine("..", "..", "..", "links", arg[1]);
                        }
                        SaveLinks(filename, links);

                        break;
                    case "ta":
                        if (arg[1] == "bort")
                        {
                            // FIXME: Lägg till felhantering för att analysera index.
                            if (int.TryParse(arg[2], out int index))
                            {
                                RemoveLink(index);
                            }
                            else
                            {
                                Console.WriteLine("Ogiltigt index för borttagning.");
                            }
                        }
                        break;
                    case "öppna":
                        if (arg[1] == "grupp")
                        {

                            if (int.TryParse(arg[2], out int index))
                            {
                                OpenLink(index);
                            }
                            else
                            {
                                Console.WriteLine("Ogiltigt index för öppning av länk.");
                            }
                        }
                        else if (arg[1] == "länk")
                        {
                            // FIXME: Lägg till felhantering för att analysera index.
                            if (int.TryParse(arg[2], out int index))
                            {
                                OpenLink(index);
                            }
                            else
                            {
                                Console.WriteLine("Ogiltigt index för öppning av länk.");
                            }
                        }
                        break;
                    default:    // Okänd kommando
                        Console.WriteLine($"Kommandot '{command}' Felaktigt kommando. Skriv 'hjälp' för en översikt av tillgängliga alternativ..");
                        break;
                }
            } while (true);
        }

        static void LoadLinks(string filename, List<Link> links)
        {
            links.Clear();
            try
            {
                using (StreamReader sr = new StreamReader(filename))
                {
                    int index = 0;
                    string line = sr.ReadLine();
                    while (line != null)
                    {
                        Link link = new Link(line);
                        link.DisplayDetails(index++);
                        links.Add(link);
                        line = sr.ReadLine();
                    }
                }
            }
            catch (Exception ex)
            {
                // FIXME: Lägg till korrekt felhantering för filläsning.
                Console.WriteLine($"Fel vid läsning av fil: {ex.Message}");
            }
        }

        static void PrintHelp()
        {
            Console.WriteLine("Välkommen till länkhanteraren!");
            Console.WriteLine("Använd något av följande kommandon för att hantera dina länkar:");
            Console.WriteLine("  lista                   - Lista alla länkar.");
            Console.WriteLine("  ny                      - Skapa en ny länk.");
            Console.WriteLine("  öppna grupp <gruppnamn> - Öppna alla länkar i en specifik grupp.");
            Console.WriteLine("                           Exempel: 'öppna grupp NyaLänkar'");
            Console.WriteLine("  öppna länk <index>      - Öppna en specifik länk med angivet index.");
            Console.WriteLine("                           Exempel: 'öppna länk 5'");
            Console.WriteLine("  ta bort <index>         - Ta bort en länk med angivet index.");
            Console.WriteLine("                           Exempel: 'ta bort 3'");
            Console.WriteLine("  ladda <fil>             - Ladda länkar från en fil.");
            Console.WriteLine("                           Exempel: 'ladda links.lis' eller 'ladda links.save.lis'");
            Console.WriteLine("  spara <fil>             - Spara länkar till en fil.");
            Console.WriteLine("                           Exempel: 'spara links.lis' eller 'spara links.save.lis'");
            Console.WriteLine("  hjälp                   - Skriv ut den här hjälpen.");
            Console.WriteLine("  sluta                   - Avsluta programmet.");
            Console.WriteLine("Vänligen välj ett kommando för att fortsätta.");
        }

        static void ListLinks()
        {
            int index = 0;
            foreach (Link link in links)
                link.DisplayDetails(index++);
        }

        static void CreateNewLink()
        {
            Console.WriteLine("Skapa en ny länk:");
            Console.Write("  ange kategori: ");
            string category = Console.ReadLine();
            Console.Write("  ange grupp: ");
            string group = Console.ReadLine();
            Console.Write("  ange namn: ");
            string name = Console.ReadLine();
            Console.Write("  ange beskrivning: ");
            string descr = Console.ReadLine();
            Console.Write("  ange länk: ");
            string link = Console.ReadLine();
            Link newLink = new Link(category, group, name, descr, link);
            links.Add(newLink);
            Console.WriteLine($"Länken '{name}' har skapats.");

        }

        static void SaveLinks(string filename, List<Link> links)
        {
            using (StreamWriter sw = new StreamWriter(filename))
            {
                foreach (Link link in links)
                {
                    sw.WriteLine(link.ToString());
                }
            }
            Console.WriteLine($"Länkar har sparats till '{filename}'.");

        }

        static void RemoveLink(int index)
        {
            Link removedLink = links[index];

            links.RemoveAt(index);
            Console.WriteLine($"Länken '{removedLink.name}' har tagits bort.");

        }

        static void OpenGroup(string group)
        {
            foreach (Link link in links)
            {
                if (link.group == group)
                {
                    link.LaunchLink();
                }
            }
        }

        static void OpenLink(int index)
        {
            links[index].LaunchLink();
        }
    }
}