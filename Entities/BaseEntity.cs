namespace AbsenceFlow.API.Entities
{
    public abstract class BaseEntity
    {
        protected BaseEntity()
        {
            CreatedAt = DateTime.UtcNow; 
            IsDeleted = false;
        }

        public int Id { get; protected set; } 
        public DateTime CreatedAt { get; private set; }
        public DateTime? UpdatedAt { get; private set; } 

        public bool IsDeleted { get; private set; }

        public void SetAsDeleted()
        {
            IsDeleted = true;
        }

        
        public void SetAsUpdated()
        {
            UpdatedAt = DateTime.UtcNow;
        }
    }
}
