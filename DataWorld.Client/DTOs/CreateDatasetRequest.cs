using System.Collections.Generic;

namespace DataWorld.Client.DTOs
{
    public class CreateDatasetRequest
    {
        public string Description { get; set; }
        public string License { get; set; }
        public string Summary { get; set; }
        public List<string> Tags { get; set; }
        public string Title { get; set; }
        public string Visibility { get; set; }
    }
}