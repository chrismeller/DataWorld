using System.Collections.Generic;

namespace DataWorld.Client.DTOs
{
    public class PaginatedResult<T>
    {
        public long Count { get; set; }
        public string Next { get; set; }

        public List<T> Records { get; set; }
    }
}