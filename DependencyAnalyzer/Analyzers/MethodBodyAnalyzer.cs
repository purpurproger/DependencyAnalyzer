using DependencyAnalyzer.DependencyAnalyzers;
using DependencyAnalyzer.Extensions;
using DependencyAnalyzer.Models;
using Mono.Cecil;
using System.Collections.Generic;
using System.Linq;

namespace DependencyAnalyzer
{
  public class MethodBodyAnalyzer
  {
    public static IEnumerable<DependencyInfo> GetDependencies(MethodDefinition method)
    {
      if (method == null || method.Body == null)
        return Enumerable.Empty<DependencyInfo>();

      var variablesTypes = method.Body.Variables.Select(x => x.VariableType).ToList();

      var types = variablesTypes.SelectMany(GenericUtility.GetGenericArguments).ToList<MemberReference>();
      types.AddRange(variablesTypes.SelectMany(x => GenericUtility.GetGenericConstraintsTypes(x.GetElementType())));
      types.AddRange(variablesTypes);
      types.AddRange(GetTypesFromMethodBodyInstructions(method));

      return types
        .Distinct()
        .Select(x => x.GetDependencyInfo())
        .SkipNull();
    }

    private static IEnumerable<MemberReference> GetTypesFromMethodBodyInstructions(MethodDefinition method)
    {
      var result = new List<MemberReference>();

      foreach (var instruction in method.Body.Instructions)
      {
        if (instruction.Operand is TypeReference typeReference)
        {
          result.Add(typeReference);
          result.AddRange(GenericUtility.GetGenericConstraintsTypes(typeReference.GetElementType()));
          result.AddRange(GenericUtility.GetGenericArguments(typeReference));
        }

        if (instruction.Operand is MethodReference methodReference)
        {
          result.Add(methodReference);
          result.AddRange(GenericUtility.GetGenericArguments(methodReference.DeclaringType));
          result.AddRange(GenericUtility.GetGenericConstraintsTypes(methodReference.DeclaringType.GetElementType()));

          result.AddRange(MethodDefinitionAnalyzer.GetDependencies(methodReference.SafeResolve()));
          result.AddRange(GenericUtility.GetGenericArguments(methodReference));
        }
      }

      return result.SkipNull();
    }
  }
}