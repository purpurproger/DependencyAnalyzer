using System.Collections.Generic;
using DependencyAnalyzer.DependencyAnalyzers;
using DependencyAnalyzer.Models;
using Mono.Cecil;
using System.IO;
using System.Linq;

namespace DependencyAnalyzer
{
  public class DependencySearcher
  {
    public IEnumerable<ModuleInfo> GetDependency(IEnumerable<FileInfo> assemblies, ReaderParameters readerParameters)
    {
      return assemblies.Select(x => GetDependency(x, readerParameters));
    }
    public ModuleInfo GetDependency(FileInfo assembly, ReaderParameters readerParameters)
    {
      var module = ModuleDefinition.ReadModule(assembly.Name, readerParameters);
      var dependencies = module.Types.SelectMany(TypeAnalyzer.GetDependencies).Distinct();
      return new ModuleInfo(module.Name, dependencies);
    }
  }
}