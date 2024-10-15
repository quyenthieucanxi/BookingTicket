using FluentAssertions;
using NetArchTest.Rules;

namespace Architecture.Tests;

public class ArchitectureTests
{
    private const string DomainNamespance = "Domain";
    private const string ApplicationNamespance = "Application";
    private const string InfrastructureNamespance = "Infrastructure";
    private const string PersistentNamespance = "Persistent";
    private const string PresentationNamespance = "Presentation";
    
    [Fact]
    public void Domain_Should_Not_HaveDependencyOnOtherProjects()
    {
        //Arrange
        var assembly = typeof(Domain.AssemblyReference).Assembly;
        var otherProjects = new[]
        {
            ApplicationNamespance,
            InfrastructureNamespance,
            PersistentNamespance,
            PresentationNamespance,
        };
        //Act
        var testResult = Types.InAssembly(assembly).ShouldNot().HaveDependencyOnAll(otherProjects).GetResult();
        
        //Assert
        testResult.IsSuccessful.Should().BeTrue();

    }
    
    
    [Fact]
    public void Application_Should_Not_HaveDependencyOnOtherProjects()
    {
        //Arrange
        var assembly = typeof(Application.AssemblyReference).Assembly;
        var otherProjects = new[]
        {
            InfrastructureNamespance,
            PersistentNamespance,
            PresentationNamespance,
        };
        //Act
        var testResult = Types.InAssembly(assembly).ShouldNot().HaveDependencyOnAll(otherProjects).GetResult();
        
        //Assert
        testResult.IsSuccessful.Should().BeTrue();

    }
    
    [Fact]
    public void Infrastructure_Should_Not_HaveDependencyOnOtherProjects()
    {
        //Arrange
        var assembly = typeof(Infrastructure.AssemblyReference).Assembly;
        var otherProjects = new[]
        {
            PresentationNamespance,
        };
        //Act
        var testResult = Types.InAssembly(assembly).ShouldNot().HaveDependencyOnAll(otherProjects).GetResult();
        
        //Assert
        testResult.IsSuccessful.Should().BeTrue();
    }
    
    
    [Fact]
    public void Persistence_Should_Not_HaveDependencyOnOtherProjects()
    {
        //Arrange
        var assembly = typeof(Persistence.AssemblyReference).Assembly;
        var otherProjects = new[]
        {
            InfrastructureNamespance,
            PresentationNamespance,
        };
        //Act
        var testResult = Types.InAssembly(assembly).ShouldNot().HaveDependencyOnAll(otherProjects).GetResult();
        
        //Assert
        testResult.IsSuccessful.Should().BeTrue();
    }
    
}
