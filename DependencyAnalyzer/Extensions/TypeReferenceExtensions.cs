using DependencyAnalyzer.Models;
using JetBrains.Annotations;
using Mono.Cecil;
using System;

namespace DependencyAnalyzer.Extensions
{
  public static class TypeReferenceExtensions
  {
    [CanBeNull]
    public static DependencyInfo GetDependencyInfo(this MemberReference memberReference)
    {
      if (memberReference == null)
        return null;

      if (memberReference is TypeReference typeReference)
        return typeReference.SafeResolve()?.ToDependencyInfo();

      return memberReference.SafeResolve()?.ToDependencyInfo();
    }

    [CanBeNull]
    public static MethodDefinition SafeResolve(this MethodReference methodReference)
    {
      if (methodReference == null)
        return null;

      try
      {
        var methodDefinition = methodReference.Resolve();
        Console.WriteLine("Resolved " + methodDefinition);
        return methodDefinition;
      }
      catch (Exception e)
      {
        Console.WriteLine(e);
        return null;
      }
    }

    [CanBeNull]
    private static TypeDefinition SafeResolve(this TypeReference typeReference)
    {
      if (typeReference == null)
        return null;

      try
      {
        var typeDefinition = typeReference.Resolve();
        Console.WriteLine("Resolved " + typeDefinition);
        return typeDefinition;
      }
      catch (Exception e)
      {
        Console.WriteLine(e);
        return null;
      }
    }

    [CanBeNull]
    private static IMemberDefinition SafeResolve(this MemberReference memberReference)
    {
      if (memberReference == null)
        return null;

      try
      {
        var memberDefinition = memberReference.Resolve();
        Console.WriteLine("Resolved " + memberDefinition);
        return memberDefinition;
      }
      catch (Exception e)
      {

        Console.WriteLine(e);
        return null;
      }
    }
  }
}