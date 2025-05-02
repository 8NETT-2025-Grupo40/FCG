namespace FCG.Domain.Modules.BaseEntities
{
    public class BaseEntity
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string UserCreated { get; set; }
        public DateTime DateCreated { get; set; }
        public string userUpdated { get; set; }
        public DateTime? DateUpdated { get; set; }
        public string Status { get; set; }
    }
}