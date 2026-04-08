using System;

namespace flexa.App.DTOs
{
    public class AppEntityRequest
    {
        public Guid Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
    }
}