using DependencyAnalyzer.Models;
using Mono.Cecil;

namespace DependencyAnalyzer.Extensions
{
  public static class TypeDefinitionExtensions
  {
    public static DependencyInfo ToDependencyInfo(this TypeDefinition typeDefinition)
    {
      return new DependencyInfo(typeDefinition.Module.Assembly.Name.Name, typeDefinition.Namespace, typeDefinition.Name, null);
    }

    public static DependencyInfo ToDependencyInfo(this IMemberDefinition memberDefinition)
    {
      return new DependencyInfo(
        memberDefinition.DeclaringType.Module.Assembly.Name.Name,
        memberDefinition.DeclaringType.Namespace,
        memberDefinition.DeclaringType.Name,
        memberDefinition.Name);
    }
  }
}
