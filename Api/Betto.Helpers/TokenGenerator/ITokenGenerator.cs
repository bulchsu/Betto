namespace Betto.Helpers
{
    public interface ITokenGenerator
    {
        string GenerateToken(string username, string role);
    }
}
