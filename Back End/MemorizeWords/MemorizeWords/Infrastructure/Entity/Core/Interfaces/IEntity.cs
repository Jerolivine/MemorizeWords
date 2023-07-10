using System.ComponentModel.DataAnnotations.Schema;

namespace MemorizeWords.Infrastructure.Entity.Core.Interfaces
{
    public interface IEntity<T> where T : struct
    {
        public T Id { get; set; }
    }
}
