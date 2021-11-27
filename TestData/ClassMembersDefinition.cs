using System;
using System.Collections.Generic;

namespace TestData
{
  public class ClassMembersDefinition
  {
    public const double Pi = 3.14;

    public static IEnumerable<Nullable<int>> GenericField;

    private DateTime privateField;
    public (int first, double second) Property => default;

    public event EventHandler<EventArgs> Event;

    public static implicit operator byte(ClassMembersDefinition d) => default;

    public static explicit operator ClassMembersDefinition(byte b) => default;

    public DateTime this[int i] => default;

    private class InnerClass
    {
      public static int SomeMethod() => default;
    }

    public void MethodParameters(TimeSpan timespanParameter, int intParameter)
    { }

    public string MethodResult() => "ok";


    public void MethodGenericParameters<TEnum, TDelegate>()
      where TEnum : Enum
      where TDelegate : Delegate
    { }

    public static IntPtr[] StaticMethod(char c) => default;

    public ClassMembersDefinition(long parameter)
    { }
  }
}
