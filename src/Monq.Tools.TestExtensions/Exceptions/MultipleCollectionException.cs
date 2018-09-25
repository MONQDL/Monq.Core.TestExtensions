using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using Xunit.Sdk;

namespace Xunit
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
        readonly string innerException;
        readonly string innerStackTrace;

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
            this.innerException = FormatInnerException(innerException);
            innerStackTrace = innerException == null ? null : innerException.StackTrace;
            Expected = expected;
            Actual = actual;
        }

        /// <summary>
        /// The collection that failed the test.
        /// </summary>
        public object Collection { get; set; }

        public object Expected { get; set; }
        public object Actual { get; set; }

        /// <summary>
        /// The index of the position where the first comparison failure occurred, or -1 if
        /// comparisions did not occur (because the actual and expected counts differed).
        /// </summary>
        public string Position { get; set; }

        /// <inheritdoc/>
        public override string Message
        {
            get
            {
                return string.Format(CultureInfo.CurrentCulture,
                                     "{0}{4}Collection: {1}{4}Error during comparison of item with key {2}{4}Expected: {5}{4}Actual: {6}{4}Inner exception: {3}",
                                     base.Message,
                                     ArgumentFormatter.Format(Collection),
                                     Position,
                                     innerException,
                                     Environment.NewLine,
                                     ArgumentFormatter.Format(Expected),
                                     ArgumentFormatter.Format(Actual));
            }
        }

        /// <inheritdoc/>
        public override string StackTrace
        {
            get
            {
                if (innerStackTrace == null)
                    return base.StackTrace;

                return innerStackTrace + Environment.NewLine + base.StackTrace;
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