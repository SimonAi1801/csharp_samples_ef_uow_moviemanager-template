using MovieManager.Core.Entities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Utils;

namespace MovieManager.Core
{
    public class ImportController
    {
        const string Filename = "movies.csv";

        /// <summary>
        /// Liefert die Movies mit den dazugehörigen Kategorien
        /// </summary>
        public static IEnumerable<Movie> ReadFromCsv()
        {
            bool isFirstRow = true;
            string filePath = MyFile.GetFullNameInApplicationTree(Filename);
            string[] lines = File.ReadAllLines(filePath, Encoding.UTF8);
            IList<Movie> movies = new List<Movie>();
            IDictionary<string, Category> categories = new Dictionary<string, Category>();

            foreach (var item in lines)
            {
                if (!isFirstRow)
                {
                    string[] parts = item.Split(';');
                    string tile = parts[0];
                    int year = Convert.ToInt32(parts[1]);
                    string categoryName = parts[2];
                    int duration = Convert.ToInt32(parts[3]);
                    Movie movie = new Movie
                    {
                        Title = tile,
                        Duration = duration,
                        Year = year
                    };

                    Category tmp;
                    if (categories.TryGetValue(categoryName, out tmp))
                    {
                        tmp.Movies.Add(movie);
                        movie.Category = tmp;
                    }
                    else
                    {
                        Category newCategory = new Category 
                        { 
                            CategoryName = categoryName 
                        };
                        categories.Add(categoryName, newCategory);
                        movie.Category = newCategory;
                        newCategory.Movies.Add(movie);
                    }
                    movies.Add(movie);
                }
                else
                {
                    isFirstRow = false;
                }
            }
            return movies;
        }

    }
}
