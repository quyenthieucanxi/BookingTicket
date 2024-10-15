namespace Application.Users.Commands.CreateUser;

public record class CreateUserRequest(string Email,string UserName,string Password,string Name);
