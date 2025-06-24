using AzureFunctions.Users;
using AzureFunctions.UserDTOS;

namespace AzureFunctions.UserInterface
{
    public interface IUserService
    {   
        Task<IEnumerable<User>> GetAllUsersAsync();
        Task<UserDTO> GetUserByIdAsync(int id);
        Task AddUserAsync(User user);
        Task DeleteUserAsync(int id);
        Task<UserDTO> UpdateUserAsync(UserDTO userdto,int id);
    }
}