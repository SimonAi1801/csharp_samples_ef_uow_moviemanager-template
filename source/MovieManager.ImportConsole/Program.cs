using MovieManager.Core;
using MovieManager.Core.Entities;
using MovieManager.Persistence;
using System;
using System.Linq;

namespace MovieManager.ImportConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            InitData();
            AnalyzeData();

            Console.WriteLine();
            Console.Write("Beenden mit Eingabetaste ...");
            Console.ReadLine();
        }

        private static void InitData()
        {
            Console.WriteLine("***************************");
            Console.WriteLine("          Import");
            Console.WriteLine("***************************");

            Console.WriteLine("Import der Movies und Categories in die Datenbank");
            using (UnitOfWork unitOfWork = new UnitOfWork())
            {
                Console.WriteLine("Datenbank löschen");
                unitOfWork.DeleteDatabase();
                Console.WriteLine("Datenbank migrieren");
                unitOfWork.MigrateDatabase();
                Console.WriteLine("Movies/Categories werden eingelesen");

                var movies = ImportController.ReadFromCsv().ToArray();
                if (movies.Length == 0)
                {
                    Console.WriteLine("!!! Es wurden keine Movies eingelesen");
                    return;
                }

                var categories = movies.GroupBy(c => c.Category);


                Console.WriteLine($"  Es wurden {movies.Count()} Movies in {categories.Count()} Kategorien eingelesen!");
                unitOfWork.MovieRepository.AddRange(movies);
                unitOfWork.Save();
                Console.WriteLine();
            }
        }

        private static void AnalyzeData()
        {
            Console.WriteLine("***************************");
            Console.WriteLine("        Statistik");
            Console.WriteLine("***************************");

            using (UnitOfWork uow = new UnitOfWork())
            {

                // Längster Film: Bei mehreren gleichlangen Filmen, soll jener angezeigt werden, dessen Titel im Alphabet am weitesten vorne steht.
                // Die Dauer des längsten Films soll in Stunden und Minuten angezeigt werden!
                //TODO
                var longestFilm = uow.MovieRepository.GetLongestFilm();

                Console.WriteLine($"Längster Film: {longestFilm.Title}; Länge:{GetDurationAsString(longestFilm.Duration, false)}");
                Console.WriteLine();
                // Top Kategorie:
                //   - Jene Kategorie mit den meisten Filmen.
                //TODO
                var categoryWithMostFilms = uow.CategoryRepository.GetCategoryWithMostFilms();

                Console.WriteLine($"Kategorie mit den meisten Filmen: '{categoryWithMostFilms.CategoryName}'; Filme: {categoryWithMostFilms.Movies.Count()}");
                Console.WriteLine();
                // Jahr der Kategorie "Action":
                //  - In welchem Jahr wurden die meisten Action-Filme veröffentlicht?
                var yearOfMostActionFilms = uow.MovieRepository.GetYearOfTheMostActionsFilms();
                Console.WriteLine($"Jahr der Action-Filme: {yearOfMostActionFilms}");
                Console.WriteLine();
                // Kategorie Auswertung (Teil 1):
                //   - Eine Liste in der je Kategorie die Anzahl der Filme und deren Gesamtdauer dargestellt wird.
                //   - Sortiert nach dem Namen der Kategorie (aufsteigend).
                //   - Die Gesamtdauer soll in Stunden und Minuten angezeigt werden!
                //TODO
                var categoryListOne = uow.CategoryRepository.GetCategoryListOne();
                Console.WriteLine($"{"Kategorie",-20}{"Anzahl",-10}{"Gesamtdauer",-10}");
                Console.WriteLine("==================================================");
                foreach (var item in categoryListOne)
                {
                    Console.WriteLine($"{item.Name,-20}{item.Count,-10}{GetDurationAsString(item.Duration, false),-10}");
                }
                Console.WriteLine();
                // Kategorie Auswertung (Teil 2):
                //   - Alle Kategorien und die durchschnittliche Dauer der Filme der Kategorie
                //   - Absteigend sortiert nach der durchschnittlichen Dauer der Filme.
                //     Bei gleicher Dauer dann nach dem Namen der Kategorie aufsteigend sortieren.
                //   - Die Gesamtdauer soll in Stunden, Minuten und Sekunden angezeigt werden!
                //TODO
                var categoryListTwo = uow.CategoryRepository.GetCategoryListWithAverageDuration();
                Console.WriteLine($"{"Kategorie",-20}{"durchschn. Gesamtdauer",-10}");
                Console.WriteLine("===============================================");
                foreach (var item in categoryListTwo)
                {
                    Console.WriteLine($"{item.Name,-20} {GetDurationAsString(item.AVG, true),-10}");
                }
            }
        }

        private static string GetDurationAsString(double minutes, bool withSeconds)
        {
            TimeSpan timespan = TimeSpan.FromMinutes(minutes);
            int hour = timespan.Hours;
            int min = timespan.Minutes;
            string result = $"{hour:D2} h {min:d2} min";
            if (withSeconds)
            {
                int sec = timespan.Seconds;
                result += $" {sec:d2} sec";
            }
            return result;
        }
    }
}
