using DependencyAnalyzer.Extensions;
using DependencyAnalyzer.Models;
using Mono.Cecil;
using System.Linq;

namespace DependencyAnalyzer.DependencyAnalyzers
{
  public class PropertyAnalyzer
  {
    public static TypeMemberInfo GetDependencies(PropertyDefinition property)
    {
      var definitionDependencies = PropertyDefinitionAnalyzer.GetDependencies(property).Distinct();

      var bodyDependencies = MethodBodyAnalyzer.GetDependencies(property.GetMethod)
        .Concat(MethodBodyAnalyzer.GetDependencies(property.SetMethod))
        .Distinct();

      return new TypeMemberInfo(property.Name, definitionDependencies, bodyDependencies);
    }
  }
}