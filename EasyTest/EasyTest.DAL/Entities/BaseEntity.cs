using System.ComponentModel.DataAnnotations;

namespace EasyTest.DAL.Entities
{
    public abstract class BaseEntity<TKey>
    {
        [Key]
        TKey Id { get; set; }
    }
}
