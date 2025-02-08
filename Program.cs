﻿using System.Diagnostics;
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
                // FIXA: Överskrev 'ToString'-metoden för att säkerställa korrekt formatering.
                return $"{category}|{group}|{name}|{descr}|{link}";
            }
        }
        static void Main(string[] args)
        {
            string filename = @"..\..\..\links\links.lis";
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
                            filename = $@"..\..\..\links\{arg[1]}";
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
                            filename = $@"..\..\..\links\{arg[1]}";
                        }
                        SaveLinks(filename, links);

                        break;
                    case "ta":
                        if (arg[1] == "bort")
                        {
                            RemoveLink(Int32.Parse(arg[2]));
                        }
                        break;
                    case "öppna":
                        if (arg[1] == "grupp")
                        {
                            OpenGroup(arg[2]);

                        }
                        else if (arg[1] == "länk")
                        {
                            OpenLink(Int32.Parse(arg[2]));

                        }
                        break;
                    default:    // Okänd kommando
                        Console.WriteLine($"Okänt kommando: '{command}'");
                        break;
                }
            } while (true);
        }

        static void LoadLinks(string filename, List<Link> links)
        {
            links.Clear();
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

        static void PrintHelp()
        {
            Console.WriteLine("hjälp           - skriv ut den här hjälpen");
            Console.WriteLine("sluta           - avsluta programmet");
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
        }

        static void RemoveLink(int index)
        {
            links.RemoveAt(index);
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