using DependencyAnalyzer.Models;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace DependencyAnalyzer
{
  public class DependencyFilter
  {
    private readonly HashSet<string> _relevantAssemblies;

    public bool IsRelevant(DependencyInfo dependency)
    {
      return _relevantAssemblies.Contains(dependency.Assembly);
    }

    public DependencyFilter(IEnumerable<Assembly> relevantAssemblies)
    {
      _relevantAssemblies = relevantAssemblies
        .Select(x => x.GetName().Name)
        .ToHashSet();
    }
  }
}