using System;
using System.Collections.Generic;
using System.Text;

namespace MovieManager.Core.DataTransferObjects
{
    public class CategoryStatsDto
    {
        public string Name { get; set; }
        public int Count { get; set; }
        public int Duration { get; set; }
    }
}
