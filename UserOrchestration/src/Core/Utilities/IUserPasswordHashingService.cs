namespace UserOrchestration.Utilities;

public interface IUserPasswordHashingService
{
    string ComputeHash(string password);
}
