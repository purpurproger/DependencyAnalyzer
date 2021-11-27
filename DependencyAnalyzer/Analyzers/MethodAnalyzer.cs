using DependencyAnalyzer.Models;
using Mono.Cecil;
using System.Linq;
using DependencyAnalyzer.Extensions;

namespace DependencyAnalyzer.DependencyAnalyzers
{
  public class MethodAnalyzer
  {
    public static TypeMemberInfo GetDependencies(MethodDefinition method) =>
      new TypeMemberInfo(
        method.Name,
        MethodDefinitionAnalyzer.GetDependencies(method).Distinct().Select(x=>x.GetDependencyInfo()).SkipNull(),
        MethodBodyAnalyzer.GetDependencies(method).Distinct());
  }
}