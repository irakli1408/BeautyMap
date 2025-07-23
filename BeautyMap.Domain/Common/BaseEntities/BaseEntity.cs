using System.ComponentModel.DataAnnotations;

namespace BeautyMap.Domain.Common.BaseEntities
{
    public abstract class BaseEntity<T>
    {
        [Key]
        public T Id { get; set; }
    }

    public abstract class BaseEntity : BaseEntity<int> { }
}
