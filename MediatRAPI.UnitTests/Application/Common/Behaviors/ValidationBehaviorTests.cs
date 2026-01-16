using FluentValidation;
using FluentValidation.Results;
using MediatR;
using MediatRHandlers.Application.Common.Behaviors;

namespace MediatRAPI.UnitTests.Application.Common.Behaviors;

public class ValidationBehaviorTests
{
    [Fact]
    public async Task Handle_ShouldProceedToNext_WhenNoValidatorsAreProvided()
    {
        // Arrange
        IEnumerable<IValidator<TestRequest>> validators = Enumerable.Empty<IValidator<TestRequest>>();
        ValidationBehavior<TestRequest, TestResponse> behavior = new(validators);
        TestRequest request = new();
        TestResponse expectedResponse = new();
        bool nextCalled = false;

        Task<TestResponse> next(CancellationToken ct = default)
        {
            nextCalled = true;
            return Task.FromResult(expectedResponse);
        }

        // Act
        TestResponse result = await behavior.Handle(request, next, CancellationToken.None);

        // Assert
        nextCalled.Should().BeTrue();
        result.Should().Be(expectedResponse);
    }

    [Fact]
    public async Task Handle_ShouldProceedToNext_WhenValidationPasses()
    {
        // Arrange
        Mock<IValidator<TestRequest>> mockValidator = new();
        mockValidator.Setup(v => v.Validate(It.IsAny<ValidationContext<TestRequest>>()))
                     .Returns(new ValidationResult());

        IValidator<TestRequest>[] validators = new[] { mockValidator.Object };
        ValidationBehavior<TestRequest, TestResponse> behavior = new(validators);
        TestRequest request = new();
        TestResponse expectedResponse = new();
        bool nextCalled = false;

        Task<TestResponse> next(CancellationToken ct = default)
        {
            nextCalled = true;
            return Task.FromResult(expectedResponse);
        }

        // Act
        TestResponse result = await behavior.Handle(request, next, CancellationToken.None);

        // Assert
        nextCalled.Should().BeTrue();
        result.Should().Be(expectedResponse);
        mockValidator.Verify(v => v.Validate(It.IsAny<ValidationContext<TestRequest>>()), Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldThrowValidationException_WhenValidationFails()
    {
        // Arrange
        Mock<IValidator<TestRequest>> mockValidator = new();
        List<ValidationFailure> validationFailures = new()
        {
            new("Property1", "Error message 1"),
            new("Property2", "Error message 2")
        };
        ValidationResult validationResult = new(validationFailures);

        mockValidator.Setup(v => v.Validate(It.IsAny<ValidationContext<TestRequest>>()))
                     .Returns(validationResult);

        IValidator<TestRequest>[] validators = new[] { mockValidator.Object };
        ValidationBehavior<TestRequest, TestResponse> behavior = new(validators);
        TestRequest request = new();
        bool nextCalled = false;

        Task<TestResponse> next(CancellationToken ct = default)
        {
            nextCalled = true;
            return Task.FromResult(new TestResponse());
        }

        // Act & Assert
        ValidationException exception = await Assert.ThrowsAsync<ValidationException>(
            () => behavior.Handle(request, next, CancellationToken.None));

        nextCalled.Should().BeFalse();
        exception.Errors.Should().HaveCount(2);
        mockValidator.Verify(v => v.Validate(It.IsAny<ValidationContext<TestRequest>>()), Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldCombineFailuresFromMultipleValidators()
    {
        // Arrange
        Mock<IValidator<TestRequest>> mockValidator1 = new();
        Mock<IValidator<TestRequest>> mockValidator2 = new();

        mockValidator1.Setup(v => v.Validate(It.IsAny<ValidationContext<TestRequest>>()))
                     .Returns(new ValidationResult(new[]
                     {
                         new ValidationFailure("Property1", "Error from validator 1")
                     }));

        mockValidator2.Setup(v => v.Validate(It.IsAny<ValidationContext<TestRequest>>()))
                     .Returns(new ValidationResult(new[]
                     {
                         new ValidationFailure("Property2", "Error from validator 2")
                     }));

        IValidator<TestRequest>[] validators = new[] { mockValidator1.Object, mockValidator2.Object };
        ValidationBehavior<TestRequest, TestResponse> behavior = new(validators);
        TestRequest request = new();

        static Task<TestResponse> next(CancellationToken ct = default)
        {
            return Task.FromResult(new TestResponse());
        }

        // Act & Assert
        ValidationException exception = await Assert.ThrowsAsync<ValidationException>(
            () => behavior.Handle(request, next, CancellationToken.None));

        exception.Errors.Should().HaveCount(2);
        exception.Errors.Should().Contain(e => e.PropertyName == "Property1");
        exception.Errors.Should().Contain(e => e.PropertyName == "Property2");
    }

    // Test classes for the behavior tests
    public class TestRequest : IRequest<TestResponse>
    {
    }

    public class TestResponse
    {
    }
}