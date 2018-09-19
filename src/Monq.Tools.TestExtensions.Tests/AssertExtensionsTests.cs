using Monq.Tools.TestExtensions.Extensions;
using Monq.Tools.TestExtensions.Tests.TestModels;
using Xunit;

namespace Monq.Tools.TestExtensions.Tests
{
    public class AssertExtensionsTests
    {
        [Fact(DisplayName = "Проверить соответствие модели фильтру.")]
        public void ShouldProperlyValidFilter()
        {
            AssertExtensions.AssertFilterIsValid<TestFilterViewModel, ValueViewModel>();
            //AssertExtensions.AssertFilterIsValid<BadFilterModel, ValueViewModel>();
        }
    }
}