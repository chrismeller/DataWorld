using System.Collections.Generic;
using DataWorld.Client.DTOs.Generic;

namespace DataWorld.Client.DTOs
{
    public class CreateProjectRequest
    {
        public string License { get; set; }
        public string Objective { get; set; }
        public string Summary { get; set; }
        public List<string> Tags { get; set; }
        public string Title { get; set; }
        public string Visibility { get; set; }
        public List<IdOwner> LinkedDatasets { get; set; }
    }
}