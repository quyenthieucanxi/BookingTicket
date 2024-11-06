using Application.Abstractions.Messaging;
using Domain.Primitives;
using FluentAssertions;
using NetArchTest.Rules;

namespace Architecture.Tests;

public class ApplicationTests
{
    [Fact]
    public void Command_Should_Be_Sealed()
    {
        //Arrange
        var assembly = typeof(Application.AssemblyReference).Assembly ;
        //Act
        var result = assembly.GetTypes()
            .Where(t => (typeof(ICommand).IsAssignableFrom(t) || typeof(ICommand<>).IsAssignableFrom(t)) 
                        && !t.IsInterface);
        
        //Assert
        result.Should().OnlyContain(t => t.IsSealed, 
            "All command classes should be sealed to prevent inheritance");;
    }
}