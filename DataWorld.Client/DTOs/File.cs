using System;
using System.Collections.Generic;

namespace DataWorld.Client.DTOs
{
    public class File
    {
        public string Name { get; set; }
        public long SizeInBytes { get; set; }
        public List<string> Labels { get; set; }
        public string Description { get; set; }
        // @todo include Source
        public DateTimeOffset Created { get; set; }
        public DateTimeOffset Updated { get; set; }
    }
}