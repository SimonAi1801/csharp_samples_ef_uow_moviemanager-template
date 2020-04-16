using System.Collections.Generic;
using MovieManager.Core.Entities;
namespace MovieManager.Core.Contracts
{
    public interface IMovieRepository
    {
        Movie GetLongestFilm();
        void AddRange(Movie[] movies);
        int GetYearOfTheMostActionsFilms();
    }
}
