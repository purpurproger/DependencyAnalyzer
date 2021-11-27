using DependencyAnalyzer.Extensions;
using DependencyAnalyzer.Models;
using Mono.Cecil;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DependencyAnalyzer.DependencyAnalyzers
{
  public static class TypeAnalyzer
  {
    public static IEnumerable<TypeInfo> GetDependencies(TypeDefinition type)
    {
      if (type == null)
        return Enumerable.Empty<TypeInfo>();

      Console.WriteLine("Analizing " + type.Name);

      var definitionDependencies = TypeDefinitionAnalyzer.GetDependencies(type)
        .Distinct()
        .Select(x => x.GetDependencyInfo())
        .SkipNull()
        .ToArray();

      var memberInfos = type.Properties.Select(PropertyAnalyzer.GetDependencies).ToArray();
      var enumerable = type.Fields.Select(FieldAnalyzer.GetDependencies).ToArray();

      List<TypeMemberInfo> typeMemberInfos = new List<TypeMemberInfo>();

      foreach (var method in type.Methods)
      {
        var dep = MethodAnalyzer.GetDependencies(method);
        typeMemberInfos.Add(dep);
      }

      var members = typeMemberInfos
        .Concat(memberInfos)
        .Concat(enumerable)
        .Distinct()
        .ToArray();

      var typeName = type.IsNested ? $"{type.DeclaringType.Name}.{type.Name}" : type.Name;
      var typeNamespace = type.IsNested ? type.DeclaringType.Namespace : type.Namespace; //TODO PopupDataTextComparer

      var nested = type.NestedTypes.SelectMany(GetDependencies).ToArray();
      var typeInfo = new TypeInfo(typeNamespace, typeName, definitionDependencies, members);

      return nested
        .Concat(typeInfo);
    }
  }
}