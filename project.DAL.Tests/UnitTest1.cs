using project.DAL;
using Microsoft.EntityFrameworkCore;
using project.DAL.Entities;

namespace project.DAL.Tests
{
    public class TagTests
    {
        private readonly ProjectDbContext _dbContextSUT;
        private bool _seedTestingData;

        public TagTests()
        {
            DbContextOptionsBuilder<ProjectDbContext> builder = new();
            _dbContextSUT = new ProjectDbContext(builder.Options, _seedTestingData);
        }
        [Fact]
        public void Tag_Add_Added()
        {
            var tag = new TagEntity()
            {
                Name = "Test",
                Color = System.Drawing.Color.Blue,
                Activities = null
            };
            _dbContextSUT.Tags.Add(tag);
            _dbContextSUT.SaveChanges();
        }
    }
}