using DependencyAnalyzer.DependencyAnalyzers;
using DependencyAnalyzer.Extensions;
using DependencyAnalyzer.Models;
using Mono.Cecil;
using System.Collections.Generic;
using System.Linq;

public static class PropertyDefinitionAnalyzer
{
  public static IEnumerable<DependencyInfo> GetDependencies(PropertyDefinition property)
  {
    return GenericUtility.GetGenericArguments(property.PropertyType)
      .Concat(property.PropertyType)
      .Select(x=>x.GetDependencyInfo())
      .SkipNull();
  }
}