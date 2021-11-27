using DependencyAnalyzer.Extensions;
using JetBrains.Annotations;
using System.Collections.Generic;
using System.Collections.Immutable;

namespace DependencyAnalyzer.Models
{
  public class TypeMemberInfo
  {
    public string Name { get; }

    public IImmutableList<DependencyInfo> DefinitionDependencies { get; }

    public IImmutableList<DependencyInfo> BodyDependencies { get; }

    public TypeMemberInfo(
      string name,
      [CanBeNull] IEnumerable<DependencyInfo> definitionDependencies,
      [CanBeNull] IEnumerable<DependencyInfo> bodyDependencies)
    {
      Name = name;
      DefinitionDependencies = definitionDependencies.EmptyIfNull().ToImmutableList();
      BodyDependencies = bodyDependencies.EmptyIfNull().ToImmutableList();
    }
  }
}