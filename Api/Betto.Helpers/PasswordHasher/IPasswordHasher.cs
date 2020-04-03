namespace Betto.Helpers
{
    public interface IPasswordHasher
    {
        public bool VerifyPassword(byte[] storedPasswordHash, string passedPassword);
        public byte[] EncodePassword(string passedPassword);
    }
}
