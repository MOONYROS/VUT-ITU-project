﻿    using System.Drawing;

namespace project.DAL.Entities
{
    public record ActivityEntity : IEntityID
    {
        public required Guid Id { get; set; }
        public required DateOnly DateFrom { get; set; }
        public required TimeOnly TimeFrom { get; set; }
        public required DateOnly DateTo { get; set; }
        public required TimeOnly TimeTo { get; set; }
        public required string Name { get; set; }
        public string? Description { get; set; }
        public required int Color { get; set; }
        public ICollection<ActivityTagListEntity> Tags { get; set; }
        public ProjectEntity? Project { get; set; }
        public UserEntity User { get; set; }
    }
}