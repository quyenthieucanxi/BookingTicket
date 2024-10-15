using Application.Abstractions.Messaging;
using Domain.Abstractions;
using Domain.Entities;
using Domain.Errors;
using Domain.Shared;
using Domain.ValueObjects;
using Microsoft.AspNetCore.Identity;

namespace Application.Users.Commands.CreateUser;

public sealed class CreateUserCommandHandler : ICommandHandler<CreateUserCommand,UserId>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly UserManager<User> _userManager;
    private readonly IUserRepository _userRepository;


    public CreateUserCommandHandler(
        IUnitOfWork unitOfWork, 
        UserManager<User> userManager, 
        IUserRepository userRepository)
    {
        _unitOfWork = unitOfWork;
        _userManager = userManager;
        _userRepository = userRepository;
    }

    public async Task<Result<UserId>> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        if (!await _userRepository.IsEmailUniqueAsync(request.Email,cancellationToken))
        {
            return Result.Failure<UserId>(DomainErrors.User.EmailAlreadyInUse);
        } ;
        Result<Name> nameResult = Name.Create(request.Name);
        if (nameResult.IsFailure)
        {
            return Result.Failure<UserId>(nameResult.Error);
        }
        // Validate the password using password validators
        var passwordValidator = _userManager.PasswordValidators.FirstOrDefault();
        if (passwordValidator != null)
        {
            var passwordValidationResult = await passwordValidator.ValidateAsync(_userManager, null!, request.Password);
            if (!passwordValidationResult.Succeeded)
            {
                return Result.Failure<UserId>(DomainErrors.User.PasswordInvalid);
            }
        }
        User user = User.Create(UserId.Create(), request.Email,request.UserName ,nameResult.Value);
        var userResult =  await _userManager.CreateAsync(user, request.Password);
        return  new UserId(user.Id);
    }
}