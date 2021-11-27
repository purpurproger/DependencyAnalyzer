using DependencyAnalyzer.Extensions;
using Mono.Cecil;
using System.Collections.Generic;
using System.Linq;

namespace DependencyAnalyzer.DependencyAnalyzers
{
  public static class GenericUtility
  {
    public static IEnumerable<TypeReference> GetGenericArguments(TypeReference typeReference)
    {
      if (!(typeReference is GenericInstanceType genericType))
        return Enumerable.Empty<TypeReference>();

      return genericType.GenericArguments
        .Concat(genericType.GenericArguments.SelectMany(GetGenericArguments));
    }
    public static IEnumerable<TypeReference> GetGenericArguments(MethodReference methodReference)
    {
      if (!(methodReference is GenericInstanceMethod genericMethod))
        return Enumerable.Empty<TypeReference>();

      return genericMethod.GenericArguments
        .Concat(genericMethod.GenericArguments.SelectMany(GetGenericArguments));
    }

    public static IEnumerable<TypeReference> GetTypeOrGenericConstraintsTypes(TypeReference typeReference)
    {
      if (!(typeReference is GenericParameter genericParameter))
        return typeReference.ToEnumerable();

      return genericParameter.Constraints
        .Select(x => x.ConstraintType)
        .SelectMany(GetTypeOrGenericConstraintsTypes);
    }
    public static IEnumerable<TypeReference> GetGenericConstraintsTypes(TypeReference typeReference)
    {
      var constraints = typeReference.GenericParameters
        .SelectMany(x => x.Constraints.Select(y => y.ConstraintType))
        .ToList();

      return constraints
        .Concat(constraints.SelectMany(GetGenericConstraintsTypes));
    }
  }
}