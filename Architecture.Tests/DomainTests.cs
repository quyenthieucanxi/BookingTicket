using Domain.Primitives;
using FluentAssertions;
using NetArchTest.Rules;

namespace Architecture.Tests;

public class DomainTests
{
    [Fact]
    public void DomainEvents_Should_ResideInDomainEventsNamespace()
    {
        //Arrange
        var assembly = typeof(Domain.AssemblyReference).Assembly ;
        
        //Act
        var result = Types.InAssembly(assembly)
            .That()
            .ImplementInterface(typeof(IDomainEvent))
            .Should()
            .ResideInNamespace(nameof(Domain.Entities))
            .GetResult();
        
        //Assert
        result.IsSuccessful.Should().BeTrue();
    }
}