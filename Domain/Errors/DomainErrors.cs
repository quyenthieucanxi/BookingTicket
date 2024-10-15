using Domain.Shared;

namespace Domain.Errors;

public static class DomainErrors
{
    public static class User
    {
        public static readonly Error EmailAlreadyInUse = new(
            "User.EmailAlreadyInUse",
            "The specified email is already in use");

        public static readonly Error PasswordInvalid = new Error(
            "User.PasswordInValid",
            "Password must have least 6 characters,one non alphanumeric character, one digit ('0'-'9'), one uppercase, one lowercase");

    }
    public static class Name
    {
        public static readonly Error TooLong =
            new Error(
                "Name.TooLong",
                "Name is too long"
            );

        public static readonly Error TooShort =
            new Error(
                "Name.TooShort",
                "Name is too short"
            );
    }
}