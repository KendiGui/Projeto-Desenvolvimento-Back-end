namespace Domain.Entities
{
    public class BaseEntity
    {
        public long Id { get; set; }

        public Guid guid { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime? LastUpdatedAt { get; set; }

        public BaseEntity()
        {
            this.guid = guid == null ? guid : Guid.NewGuid();
            CreatedAt = CreatedAt == null ? CreatedAt : DateTime.Now;
            LastUpdatedAt = DateTime.UtcNow;
        }
    }
}
