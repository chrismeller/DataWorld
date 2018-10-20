using System;

namespace DataWorld.Client.DTOs
{
    public class CreateProjectResponse
    {
        public string Message { get; set; }
        public Uri Uri { get; set; }
        public string Id { get; set; }
    }
}