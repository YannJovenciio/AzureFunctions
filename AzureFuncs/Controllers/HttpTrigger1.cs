using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using AzureFunctions.Users;
using AzureFunctions.UserInterface;
using Newtonsoft.Json;
using AzureFunctions.UserDTOS;

namespace Company.Function;

public class HttpTrigger1
{
    private readonly IUserService _userService;

    public HttpTrigger1(IUserService userService)
    {
        _userService = userService;
    }
    [Function("GetAllUsers")]
    public async Task<IActionResult> GetAllUsers(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "users")
        ] HttpRequest req)
    {
        try
        {
            var users = await _userService.GetAllUsersAsync();
            return new OkObjectResult(users);
        }
        catch (Exception ex)
        {
            return new StatusCodeResult(500);
        }
    }


    [Function("GetUserById")]
    public async Task<IActionResult> GetUserById(
        [HttpTrigger(AuthorizationLevel.Anonymous,"get",  Route = "users/{id:int}"
    )]HttpRequest req, int id)
    {
        try
        {
            var user = await _userService.GetUserByIdAsync(id);
            if (user == null)
            {
                return new NotFoundResult();
            }
            return new OkObjectResult(user);
        }
        catch (NullReferenceException e)
        {
            return new BadRequestObjectResult("erro" + e.Message);
        }
    }


    [Function("AddUser")]
    public async Task<IActionResult> AddUser(
        [HttpTrigger (AuthorizationLevel.Anonymous,"post",Route ="user"
        )]HttpRequest req)
    {
        try
        {
            var content = await new StreamReader(req.Body).ReadToEndAsync();
            User user = JsonConvert.DeserializeObject<User>(content);
            await _userService.AddUserAsync(user);

            return new OkObjectResult(user);
        }
        catch (System.Exception ex)
        {
            return new BadRequestObjectResult($"Erro: {ex.Message}");
        }
    }

    [Function("DeleteUser")]
    public async Task<IActionResult> DeleteUser(
        [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route ="userD/{id:int}"
        )]HttpRequest req,
        int id)
    {
        try
        {
            await _userService.DeleteUserAsync(id);
            return new OkObjectResult($"Usuario com o ID {id} deletado com sucesso");
        }
        catch (System.Exception ex)
        {
            Console.WriteLine(ex.Message);
            return new NotFoundObjectResult(ex.Message);
            throw;
        }
    }

    [Function("UpdateUser")]
    public async Task<IActionResult> UpdateUser(
        [HttpTrigger(AuthorizationLevel.Anonymous, "patch",Route = "userU/{id:int}")]
        HttpRequest req,int id)
        {
        try
        {
            var content = await new StreamReader(req.Body).ReadToEndAsync();
            UserDTO userdto = new UserDTO();

            userdto =  JsonConvert.DeserializeObject<UserDTO>(content);
            
            await _userService.UpdateUserAsync(userdto,id);
            return new OkObjectResult(userdto);
            }
        catch (System.Exception ex)
        {
            return new BadRequestObjectResult($"Erro ao atualizar o usuario com o id{id}"+ex.Message);
            throw;
        }
        }
}