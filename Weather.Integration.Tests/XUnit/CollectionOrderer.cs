using Xunit.Abstractions;

namespace Weather.Integration.Tests.XUnit;

public sealed class CollectionOrderer : ITestCollectionOrderer
{
   public IEnumerable<ITestCollection> OrderTestCollections(IEnumerable<ITestCollection> testCollections)
   {
      return testCollections.OrderBy(collection => collection.DisplayName, StringComparer.Ordinal);
   }
}
