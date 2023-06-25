using System.ComponentModel.DataAnnotations.Schema;

namespace MemorizeWords.Entity.Interfaces
{
    public interface IEntity<T> where T : struct
    {
        public T Id { get; set; }
    }
}
