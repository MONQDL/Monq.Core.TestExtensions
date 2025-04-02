using Xunit.Sdk;

namespace Xunit;

/// <summary></summary>
/// <seealso cref="Xunit.Sdk.XunitException" />
public class FilterValidationException : XunitException
{
    readonly IEnumerable<string> _badProperties;
    readonly IEnumerable<(Type Expected, Type Actual, string propName)> _badTypes;

    /// <summary>
    /// Creates a new instance of the <see cref="FilterValidationException"/> class.
    /// </summary>
    public FilterValidationException(IEnumerable<string> badProperties, IEnumerable<(Type Expected, Type Actual, string propName)> badTypes)
        : base("Assert.FilterIsValid() Failure")
    {
        _badProperties = badProperties;
        _badTypes = badTypes;
    }

    /// <inheritdoc/>
    public override string Message
    {
        get
        {
            var message = $"{base.Message}:";
            var spaces = Environment.NewLine + "".PadRight(4);
            if (_badProperties.Any())
                message += $"{Environment.NewLine}В конечной модели отсутствуют поля: {spaces}{string.Join(spaces, _badProperties)}";
            var formattedBadTypes = _badTypes.Select(error =>
                $"{error.propName}{spaces}Filter:   {error.Expected}{spaces}Model: {error.Actual}");
            if (formattedBadTypes.Any())
                message += $"{Environment.NewLine}Несоответствие типов:{Environment.NewLine} {string.Join(Environment.NewLine, formattedBadTypes)}";
            return message;
        }
    }
}
