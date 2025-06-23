using AzureFunctions.DbContexts;
using AzureFunctions.UserDTOS;
using AzureFunctions.UserInterface;
using AzureFunctions.Users;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace AzureFunctions.UserService
{
    public class UserService : IUserService
    {
        private readonly UserDbContext _context;
        public UserService(UserDbContext context)
        {
            _context = context;
        }
        public async Task AddUserAsync(User user)
        {
            if (user != null)
            {
                await _context.AddAsync(user);
                await _context.SaveChangesAsync();
            }
        }
        public async Task<IEnumerable<User>> GetAllUsersAsync()
        {
            return await _context.Users.ToListAsync();
        }
        public async Task<UserDTO> GetUserByIdAsync(int id)
        {
            try
            {
                var user = await _context.Users.FindAsync(id);

                if (user != null)
                {
                    var userDto = new UserDTO { username = user.username, email = user.email };
                    return userDto;
                }
                throw new UserNotFoundException($"Usuário com ID {id} não encontrado.");
            }
            catch (UserNotFoundException ex)
            {
                throw new UserNotFoundException($"Erro ao buscar usuário com ID {id}: {ex.Message}");
            }
        }


        public async Task DeleteUserAsync(int id)
        {
            try
            {
                //buscando usuario
                var user = await _context.Users.FindAsync(id);

                if (user == null)
                {
                    throw new UserNotFoundException($"Usuario com ID {id},não encontrado");
                }

                _context.Users.Remove(user);
                await _context.SaveChangesAsync();
            }
            catch (UserNotFoundException ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }
        public async Task<UserDTO> UpdateUserAsync(UserDTO userDto, int Id)
        {
            try
            {
                var user = await _context.Users.FindAsync(Id) ?? throw new UserNotFoundException($"Usuário com ID {userDto.id} não encontrado.");

                if (user == null) throw new UserNotFoundException("User not found");

                user.username = userDto.username;
                user.email = userDto.email;

                _context.Users.Update(user);
                await _context.SaveChangesAsync();
                return userDto;
            }
            catch (UserNotFoundException ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }
    }
}
