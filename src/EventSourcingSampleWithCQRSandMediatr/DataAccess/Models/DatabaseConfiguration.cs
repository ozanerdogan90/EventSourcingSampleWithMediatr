
namespace EventSourcingSampleWithCQRSandMediatr.DataAccess.Models
{
    public class DatabaseConfiguration
    {
        public string ConnectionString { get; set; }
        public bool UseMemoryDb { get; set; }
        public string ApplicationName { get; set; }
    }
}
