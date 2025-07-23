namespace BeautyMap.Domain.Common.Contract
{
    public interface ITrackedEntity : IDeletable
    {
        void UpdateCreateCredentials(DateTime createDate, string? createdBy);
        void UpdateLastModifiedCredentials(DateTime lastModifiedDate, string? modifiedBy);
        void UpdateDeleteCredentials(DateTime deleteDate, string? deletedBy);
    }
}
