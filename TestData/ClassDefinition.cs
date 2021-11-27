using System;
using System.Collections;
using System.Collections.Generic;

namespace TestData
{
  public class ClassDefinition<T> : List<double>, IEquatable<int>
  where T : CollectionBase
  {
    public bool Equals(int other)
    {
      throw new NotImplementedException();
    }
  }
}