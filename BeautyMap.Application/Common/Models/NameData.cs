using BeautyMap.Application.Base.BeautyMap.Application.Common.Base;
using BeautyMap.Domain.Common.BaseEntities;

namespace BeautyMap.Application.Common.Models
{
    public class NamedData<T> : BaseEntity<T>
    {
        public string Name { get; set; }
    }

    public class NamedData : NamedData<int>
    { }

    public class FileNamedData : NamedData<int>
    {
        public FileResponse File { get; set; }
    }
}
