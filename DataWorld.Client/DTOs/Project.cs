using System;
using System.Collections.Generic;

namespace DataWorld.Client.DTOs
{
    public class Project
    {
        public string AccessLevel { get; set; }
        public DateTimeOffset Created { get; set; }
        public List<File> Files { get; set; }
        public string Id { get; set; }
        public string License { get; set; }
        public List<LinkedDataset> LinkedDatasets { get; set; }
        public string Objective { get; set; }
        public string Owner { get; set; }
        public string Status { get; set; }
        public string Summary { get; set; }
        public List<string> Tags { get; set; }
        public string Title { get; set; }
        public DateTimeOffset Updated { get; set; }
        public string Version { get; set; }
        public string Visibility { get; set; }
    }
}