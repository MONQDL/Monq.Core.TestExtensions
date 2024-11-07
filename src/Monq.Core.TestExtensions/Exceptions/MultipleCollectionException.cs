using System;
using System.Globalization;
using System.Linq;

namespace Monq.Core.TestExtensions.Exceptions
{
    /// <summary>
    /// Exception thrown when Assert.Collection fails.
    /// </summary>
#if XUNIT_VISIBILITY_INTERNAL
    internal
#else

    public
#endif
    class MultipleCollectionException : XunitException
    {
        readonly string _innerException;
        readonly string _innerStackTrace;

        /// <summary>
        /// Creates a new instance of the <see cref="MultipleCollectionException" /> class.
        /// </summary>
        /// <param name="collection">The collection that failed the test.</param>
        /// <param name="expected">The expected.</param>
        /// <param name="actual">The actual.</param>
        /// <param name="position">The position.</param>
        /// <param name="innerException">The exception that was thrown during the comparison failure.</param>
        public MultipleCollectionException(object collection, object expected, object actual, string position, Exception innerException = null)
            : base("Assert.Collection() Failure")
        {
            Collection = collection;
            Position = position;
            _innerException = FormatInnerException(innerException);
            _innerStackTrace = innerException?.StackTrace;
            Expected = expected;
            Actual = actual;
        }

        /// <summary>
        /// The collection that failed the test.
        /// </summary>
        public object Collection { get; set; }

        /// <summary>
        /// Gets or sets the expected.
        /// </summary>
        public object Expected { get; set; }

        /// <summary>
        /// Gets or sets the actual.
        /// </summary>
        public object Actual { get; set; }

        /// <summary>
        /// The index of the position where the first comparison failure occurred, or -1 if
        /// comparisions did not occur (because the actual and expected counts differed).
        /// </summary>
        public string Position { get; set; }

        /// <inheritdoc/>
        public override string Message =>
            string.Format(CultureInfo.CurrentCulture,
                "{3}{0}{3}Error during comparison of item with key {1}{3}Expected: {4}{3}Actual: {5}{3}Inner exception: {2}",
                base.Message,
                Position,
                _innerException,
                Environment.NewLine,
                ArgumentFormatter.Format(Expected),
                ArgumentFormatter.Format(Actual));

        /// <inheritdoc/>
        public override string StackTrace
        {
            get
            {
                if (_innerStackTrace == null)
                    return base.StackTrace;

                return _innerStackTrace + Environment.NewLine + base.StackTrace;
            }
        }

        static string FormatInnerException(Exception innerException)
        {
            if (innerException == null)
                return null;

            var lines = innerException.Message
                                      .Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries)
                                      .Select((value, idx) => idx > 0 ? "        " + value : value);

            return string.Join(Environment.NewLine, lines);
        }
    }
}