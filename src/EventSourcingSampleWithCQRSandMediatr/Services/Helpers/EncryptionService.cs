
namespace EventSourcingSampleWithCQRSandMediatr.Services.Helpers
{
    public interface IEncryptionService
    {
        string HashPassword(string input);
        bool Verify(string currentPass, string originalPass);
    }

    public class EncryptionService : IEncryptionService
    {
        public string HashPassword(string input)
        {
            return BCrypt.Net.BCrypt.HashPassword(input);
        }

        public bool Verify(string currentPass, string originalPass)
        {
            return BCrypt.Net.BCrypt.Verify(currentPass, originalPass);
        }
    }
}
