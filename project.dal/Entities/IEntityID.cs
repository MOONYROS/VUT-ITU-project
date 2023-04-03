using System.ComponentModel.DataAnnotations;

namespace project.DAL.Entities
{
    public interface IEntityID
    {
        public Guid Id { get; set; }
    }
}