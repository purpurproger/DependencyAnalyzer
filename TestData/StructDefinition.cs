using System;
using System.Collections;

namespace TestData
{
  public struct StructDefinition<T> : IEquatable<int>
    where T : CollectionBase
  {
    public bool Equals(int other)
    {
      throw new NotImplementedException();
    }
  }
}