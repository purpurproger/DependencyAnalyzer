using DependencyAnalyzer.DependencyAnalyzers;
using DependencyAnalyzer.Extensions;
using Mono.Cecil;
using System.Collections.Generic;
using System.Linq;

public static class MethodDefinitionAnalyzer
{
  public static IEnumerable<TypeReference> GetDependencies(MethodDefinition method)
  {
    if (method == null)
      return Enumerable.Empty<TypeReference>();

    var declaringType = method.DeclaringType;
    var returnType = method.ReturnType;
    var parameters = method.Parameters.Select(x => x.ParameterType);
    var genericParameters = method.GenericParameters;

    //TODO var attributes = method.Attributes;

    return declaringType.ToEnumerable()
      .Concat(returnType)
      .Concat(parameters)
      .Concat(genericParameters)
      .SelectMany(GenericUtility.GetTypeOrGenericConstraintsTypes)
      .SkipNull();
  }
}