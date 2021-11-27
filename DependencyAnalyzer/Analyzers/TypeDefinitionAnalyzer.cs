using DependencyAnalyzer.Extensions;
using Mono.Cecil;
using System.Collections.Generic;
using System.Linq;

namespace DependencyAnalyzer.DependencyAnalyzers
{
  public static class TypeDefinitionAnalyzer
  {
    public static IEnumerable<TypeReference> GetDependencies(TypeDefinition type)
    {
      if (type == null)
        return Enumerable.Empty<TypeDefinition>();

      var types = type.BaseType.ToEnumerable()
        .Concat(type.Interfaces.Select(x => x.InterfaceType))
        .ToList();

      types.AddRange(GenericUtility.GetGenericConstraintsTypes(type));
      types.AddRange(GenericUtility.GetGenericArguments(type.BaseType));
      types.AddRange(type.Interfaces.SelectMany(x => GenericUtility.GetGenericArguments(x.InterfaceType)));

      return types;
    }
  }
}