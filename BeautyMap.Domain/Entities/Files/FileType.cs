using BeautyMap.Domain.Common.BaseEntities;

namespace BeautyMap.Domain.Entities.Files
{
    public partial class FileType : BaseEntity
    {
        public FileType()
            => Files = [];
        public int Code { get; set; }
        /// <summary>
        /// A name of the format.
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// A description of the format.
        /// </summary>
        public string Description { get; set; }

        public virtual ICollection<File> Files { get; set; }
    }
}
