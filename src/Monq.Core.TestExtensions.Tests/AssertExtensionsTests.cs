using Monq.Core.TestExtensions;
using Monq.Core.TestExtensions.Tests.TestModels;
using System.Linq;
using Xunit;

namespace Monq.Core.TestExtensions.Tests;

public class AssertExtensionsTests
{
    [Fact(DisplayName = "Проверить соответствие модели фильтру.")]
    public void ShouldProperlyValidFilter()
    {
        Assert.FilterIsValid<TestFilterViewModel, ValueViewModel>();
        //Assert.FilterIsValid<BadFilterModel, ValueViewModel>();
    }

    [Fact(DisplayName = "Проверка коллекции.")]
    public void ShouldProperlyAssertCollection()
    {
        var expectedCollection = Enumerable.Range(0, 10).Select(x => new ValueViewModel { Id = x, Name = $"TestName{x}" });
        var actualCollection = Enumerable.Range(0, 10).Select(x => new ValueViewModel { Id = x, Name = $"TestName{x}" });
        Assert.Collection(expectedCollection, actualCollection,
            x => x.Id, x => x.Id,
            (expected, actual) =>
            {
                Assert.NotNull(actual);
                Assert.Equal(expected.Name, actual.Name);
            });
    }
}