using DependencyAnalyzer.Extensions;
using JetBrains.Annotations;
using System.Collections.Generic;
using System.Collections.Immutable;

namespace DependencyAnalyzer.Models
{
  public class ModuleInfo
  {
    public string Name { get; }

    public IImmutableDictionary<string, TypeInfo> Types;

    public ModuleInfo(
      string name,
      [CanBeNull] IEnumerable<TypeInfo> types)
    {
      Name = name;
      Types = types.EmptyIfNull().ToImmutableDictionary(x => x.Name, x => x);
    }
  }
}