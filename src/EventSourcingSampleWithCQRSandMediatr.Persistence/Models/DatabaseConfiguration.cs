namespace EventSourcingSampleWithCQRSandMediatr.Persistence.Models
{
    public class DatabaseConfiguration
    {
        public string ConnectionString { get; set; }
        public bool UseMemoryDb { get; set; }
        public string ApplicationName { get; set; }
        public bool SeedData { get; set; }
    }
}
