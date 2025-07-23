namespace BeautyMap.Domain.Common.Contract
{
    internal interface IIdentityTrackedEntity : ITrackedEntity
    {
        public string? CreatedBy { get; protected set; }

        public DateTime CreateDate { get; protected set; }

        public string? LastModifiedBy { get; protected set; }

        public DateTime? LastModifiedDate { get; protected set; }

        public string? DeletedBy { get; protected set; }
    }
}
