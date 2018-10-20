using System;
using Newtonsoft.Json;

namespace DataWorld.Client.DTOs
{
    public class Doi
    {
        [JsonProperty("doi")]
        public string Id { get; set; }
        public DateTimeOffset Created { get; set; }
    }
}