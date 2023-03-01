using project.dal;

namespace project.DAL.Tests
{
    public class TagTests
    {
        private readonly ProjectDbContext _dbContextSUT;

        public TagTests()
        {
            _dbContextSUT = new ProjectDbContext();
        }
        [Fact]
        public void Tag_Add_Added()
        {
            var tag = new Tag()
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