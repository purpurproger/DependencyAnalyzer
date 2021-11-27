using DependencyAnalyzer.DependencyAnalyzers;
using DependencyAnalyzer.Extensions;
using DependencyAnalyzer.Models;
using Mono.Cecil;
using System.Linq;

public static class FieldAnalyzer
{
  public static TypeMemberInfo GetDependencies(FieldDefinition field)
  {
    var dependencies = GenericUtility.GetGenericArguments(field.FieldType)
      .Concat(field.FieldType)
      .Select(x=>x.GetDependencyInfo())
      .SkipNull()
      .Distinct();

    return new TypeMemberInfo(field.Name, dependencies, null);
  }
}