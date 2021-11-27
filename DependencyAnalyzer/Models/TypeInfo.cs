using DependencyAnalyzer.Extensions;
using JetBrains.Annotations;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace DependencyAnalyzer.Models
{
  public class TypeInfo
  {
    public string NameSpace { get; }
    public string Name { get; }

    public IImmutableList<DependencyInfo> DefinitionDependencies { get; }

    public IImmutableDictionary<string, TypeMemberInfo> Members { get; }

    public TypeInfo(string nameSpace,
      string name,
      [CanBeNull]IEnumerable<DependencyInfo> definitionDependencies, [CanBeNull]IEnumerable<TypeMemberInfo> typeMembers)
    {
      NameSpace = nameSpace;
      Name = name;
      DefinitionDependencies = definitionDependencies.EmptyIfNull().ToImmutableList();

      Members = typeMembers.EmptyIfNull()
        .GroupBy(x => x.Name)
        .ToImmutableDictionary(x => x.Key, x =>
        {
          //обработка перегрузок методов

          var items = x.ToArray();

          return new TypeMemberInfo(
            x.Key,
            items.SelectMany(y => y.DefinitionDependencies),
            items.SelectMany(y => y.BodyDependencies));
        });
    }
  }
}