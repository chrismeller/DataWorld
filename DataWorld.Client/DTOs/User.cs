using System;

namespace DataWorld.Client.DTOs
{
    public class User
    {
        public Uri AvatarUrl { get; set; }
        public string DisplayName { get; set; }
        public string Id { get; set; }
        public DateTimeOffset Created { get; set; }
        public DateTimeOffset Updated { get; set; }
    }
}