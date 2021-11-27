using System;
using System.Collections;
using System.Threading;

namespace TestData
{
  public class Body
  {
    public static void VariableDefinition()
    {
      ArrayList arrayList = null;
    }
    public static void GenericVariableDefinition()
    {
      GenericClass<ArgumentException> c = null;
    }

    public static void MethodCall()
    {
      GC.Collect();
    }
    public static void GenericMethodCall()
    {
      Class.GenericMethod<ArgumentNullException>();
    }

    public static void ObjectCreation()
    {
      new ArithmeticException();
    }
    public static void GenericObjectCreation()
    {
      new GenericClass<AccessViolationException>();
    }

    public static void ValueTypeCreation()
    {
      Thread.Sleep(new TimeSpan());
    }
    public static void GenericValueTypeCreation()
    {
      Console.WriteLine(new GenericStruct<AggregateException>());
    }

    public static void Combination()
    {
      var days = TimeSpan.Parse(new string('!', 10)).TotalDays;
    }
  }

  public class GenericClass<T>
  where T : Exception
  { }
  public struct GenericStruct<T>
  where T : Exception
  { }

  public class Class
  {
    public static void GenericMethod<T>()
    where T : Exception
    { }
  }
}