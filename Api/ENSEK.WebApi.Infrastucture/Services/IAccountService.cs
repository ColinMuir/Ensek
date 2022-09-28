namespace ENSEK.WebApi.Infrastucture.Services;

public interface IAccountService
{
    Task<bool> ExistsAsync(int accountId);
}