// See https://aka.ms/new-console-template for more information
using System;
using System.IO;
using System.Collections.Generic;
using System.Xml.Serialization;

// start program
public class Program {
    public static void Main() {
        MovieLog usr = new MovieLog();
        usr.Main();
    }

    [Serializable]
    [XmlRoot("Movie")]
    public class Movie {
        [XmlElement("Name")]
        public string Name { get; set; }

        [XmlElement("Duration")]
        public string Duration { get; set;}

        [XmlElement("Year")]
        public string Year { get; set; }
    }

    // movie log class
    public class MovieLog {
        // attributes
        public List<string[]> Movie = new List<string[]> ();
        public int minutes = 0;
        public List<Movie> loaded { get; set; }

        // main method
        public void Main() {
            this.instruction();
            while (true)
            {
                Console.Write("Input: ");
                int command = Convert.ToInt32(Console.ReadLine());

                if (command == 1) {
                    Console.WriteLine("Add movie");
                    this.Add_movie();
                    this.instruction();

                } else if (command == 2) {
                    Console.WriteLine("Remove movie");
                    this.Remove_movie();
                    this.instruction();

                } else if (command == 3) {
                    this.Report();
                    this.instruction();

                } else if (command == 4) {
                    string path = "database.xml";

                    if (File.Exists(path)) {
                        Console.WriteLine("Database loaded");
                        this.Load_database();

                    } else {
                        Console.WriteLine("Database doesn't exist");
                    }

                    this.instruction();

                } else if (command == 5) {
                    Console.WriteLine("Saved to database");
                    this.Save_database();
                    this.instruction();

                } else if (command == 6) {
                    break;
                } else {
                    Console.WriteLine("Unkown command");
                    this.instruction();
                }
            }
        }

        // add movie method
        public List<string[]> Add_movie() {
            Console.Write("Name: ");
            string name = Console.ReadLine();

            Console.Write("Duration (minutes): ");
            string duration = Console.ReadLine();

            this.minutes += Convert.ToInt32(duration);

            Console.Write("Year: ");
            string year = Console.ReadLine();

            this.Movie.Add(new string[3] {name, duration, year});
            return this.Movie;
        }

        // remove movie method
        public void Remove_movie() {
            for (int i = 0; i < this.Movie.Count; i++) {
                Console.WriteLine($"{i+1}) {this.Movie[i][0]} ({this.Movie[i][2]}), {this.Movie[i][1]} minutes.");
            }

            Console.Write("Input: ");
            int remove = Convert.ToInt32(Console.ReadLine());

            this.minutes -= Convert.ToInt32(this.Movie[remove-1][1]);
            this.Movie.RemoveAt(remove-1);
        }

        // show all movies in database
        public void Report() {
            for (int i = 0; i < this.loaded.Count; i++) {
                this.minutes += Convert.ToInt32(this.loaded[i].Duration);
                Console.Write($"{this.loaded[i].Name} ({this.loaded[i].Year}), {this.loaded[i].Duration} minutes. \n");
            }
            Console.WriteLine($"Movies watched in total {this.loaded.Count}, total duration {this.minutes} minutes.");
        }

        // save to database
        public void Save_database() {
            List<Movie> movie_data = new List<Movie> {};

            for (int i = 0; i < this.Movie.Count; i++) {
                movie_data.Add(
                    new Movie {
                        Name = this.Movie[i][0],
                        Duration = this.Movie[i][1],
                        Year = this.Movie[i][2]
                    }
                );
            }
            
            // serialize into xml file
            XmlSerializer serializer = new XmlSerializer(typeof(List<Movie>));
            using (var writer = new StreamWriter(@"database.xml")) {
                serializer.Serialize(writer, movie_data);
            }
        }

        // load all content from database
        public void Load_database() {
            string path = "database.xml";

            // deserialize xml file
            XmlSerializer deserializer = new XmlSerializer(typeof(List<Movie>));
            using (var reader = new StreamReader($@"{path}")) {
                this.loaded = (List<Movie>)deserializer.Deserialize(reader);
            }
        }

        // instructions
        public void instruction() {
            Console.WriteLine("Movie log");
            Console.WriteLine(new String('=', 20));
            Console.WriteLine("1) Add movie");
            Console.WriteLine("2) Remove movie");
            Console.WriteLine("3) Show report");
            Console.WriteLine("4) Load database");
            Console.WriteLine("5) Save to database");
            Console.WriteLine("6) Quit");
        }
    }
}