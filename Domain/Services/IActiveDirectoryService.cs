using kairosApp.Models.Support;
using System.DirectoryServices;

namespace kairosApp.Domain.Services
{
    public interface IActiveDirectoryService
    {
        string GetCurrentDomainPath();
        string GetAUser(string userName);
        DirectorySearcher BuildUserSearcher(DirectoryEntry de);
        void GetAdditionalUserInfo();

        ADToDBUser Login(string userName, string password);
        
        bool ResetPassword(string userName, string password);
        ADToDBUser FindUserByIdentification(string identificacion);
        bool CreateUser(ADCreateUser newUser);

    }
}
