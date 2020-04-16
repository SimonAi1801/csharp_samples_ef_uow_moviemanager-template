using MovieManager.Core.Entities;
using System.Collections.Generic;
using MovieManager.Core.DataTransferObjects;
namespace MovieManager.Core.Contracts
{
    public interface ICategoryRepository
    {
        Category GetCategoryWithMostFilms();
        IEnumerable<CategoryStatsDto> GetCategoryListOne();
        IEnumerable<CategoryAVGStatsDto> GetCategoryListWithAverageDuration();
    }
}
