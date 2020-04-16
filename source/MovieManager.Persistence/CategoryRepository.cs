using MovieManager.Core.Contracts;
using MovieManager.Core.Entities;
using System.Linq;
using System;
using System.Collections.Generic;
using MovieManager.Core.DataTransferObjects;

namespace MovieManager.Persistence
{
    internal class CategoryRepository : ICategoryRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public CategoryRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IEnumerable<CategoryStatsDto> GetCategoryListOne()
        {
            return _dbContext.Categories
                    .Select(c => new CategoryStatsDto
                    {
                        Name = c.CategoryName,
                        Count = c.Movies.Count(),
                        Duration = c.Movies.Sum(m => m.Duration)
                    })
                    .OrderBy(c => c.Name)
                    .ToArray();
        }

        public IEnumerable<CategoryAVGStatsDto> GetCategoryListWithAverageDuration()
        {
            return _dbContext.Categories
                   .Select(c => new CategoryAVGStatsDto
                   {
                       Name = c.CategoryName,
                       AVG = c.Movies.Average(m => m.Duration)
                   })
                   .OrderByDescending(c => c.AVG)
                   .ThenBy(c => c.Name)
                   .ToArray();
        }

        public Category GetCategoryWithMostFilms()
        {
            return _dbContext.Categories
                   .OrderByDescending(c => c.Movies.Count())
                   .First();
        }
    }
}