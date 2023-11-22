using System.ComponentModel.DataAnnotations;

namespace EasyTest.DAL.Entities
{
    public abstract class BaseEntity<TKey>
    {
        [Key]
        public TKey Id { get; set; }
    }
}
