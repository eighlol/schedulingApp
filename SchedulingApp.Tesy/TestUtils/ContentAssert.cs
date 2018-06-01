using System.Collections.Generic;
using System.Linq;
using KellermanSoftware.CompareNetObjects;
using NUnit.Framework;

namespace SchedulingApp.Tesy.TestUtils
{
    public static class ContentAssert
    {
        public static void AreEqual(object expected, object actual, string errorMessage = null)
        {
            var compareObjects = new CompareLogic(new ComparisonConfig
            {
                MaxDifferences = 100
            });

            ComparisonResult comparisonResult = compareObjects.Compare(expected, actual);

            if (comparisonResult.AreEqual)
            {
                return;
            }

            Assert.Fail($"{comparisonResult.DifferencesString}.\n {errorMessage}");
        }

        public static void AreCollectionsEquivalent<T>(IEnumerable<T> expected, IEnumerable<T> actual, string errorMessage = null)
        {
            IOrderedEnumerable<T> expectedOrderedEnumerable = expected.OrderBy(arg => arg.GetHashCode());

            IOrderedEnumerable<T> orderedEnumerable = actual.OrderBy(arg => arg.GetHashCode());

            AreEqual(expectedOrderedEnumerable, orderedEnumerable);
        }
    }
}
