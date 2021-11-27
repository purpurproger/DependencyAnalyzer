using DependencyAnalyzer.Extensions;
using DependencyAnalyzer.Models;
using Mono.Cecil;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using TypeInfo = DependencyAnalyzer.Models.TypeInfo;


namespace DependencyAnalyzer
{
  class Program
  {
    static void Main(string[] args)
    {

      var output = new FileInfo(@"D:\result.txt");

      var assembliesToCheck = new[]
      {
        new FileInfo(@"D:\Assemblie1.dll"),
        new FileInfo(@"D:\Assemblie2.dll")
      };


      var assembliesToLookFor = new HashSet<string>
      {
        "System.ValueTuple",
        "Npgsql"
      };

      var assembliesToLookForLocation = new DirectoryInfo(@"D:\<some directory>");
      var resolver = new DefaultAssemblyResolver();
      resolver.AddSearchDirectory(assembliesToLookForLocation.FullName);

      var assemblies = assembliesToLookForLocation.EnumerateFiles("*.dll", SearchOption.AllDirectories)
        .Select(AssemblySafeLoad)
        .SkipNull()
        .Where(x => assembliesToLookFor.Contains(x.GetName().Name))
        .Distinct();

       var filter = new DependencyFilter(assemblies);
      var modules = new DependencySearcher().GetDependency(assembliesToCheck, new ReaderParameters { AssemblyResolver = resolver });

      Console.WriteLine("Selecting relevant dependencies...");

      var relevantDependencies = modules
        .SelectMany(GetDependenciesData)
        .Where(x => filter.IsRelevant(x.UsedItem));

      Console.WriteLine("Writing result to file...");

      WriteToFile(output, relevantDependencies);
    }

    private static Assembly AssemblySafeLoad(FileInfo file)
    {
      try
      {
        return Assembly.LoadFile(file.FullName);
      }
      catch (Exception)
      {
        return null;
      }
    }

    private static IEnumerable<Dependency> GetDependenciesData(ModuleInfo module)
    {
      var dependenciesData = module.Types.Values.SelectMany(typeInfo => GetTypeDefinitionDependencies(module, typeInfo))
        .Concat(module.Types.Values.SelectMany(typeInfo => typeInfo.Members.Values.SelectMany(typeMemberInfo => GetTypeMemberDependencies(module, typeInfo, typeMemberInfo))))
        .Distinct()
        .ToList();

      return dependenciesData;
    }

    private static IEnumerable<Dependency> GetTypeMemberDependencies(ModuleInfo module, TypeInfo typeInfo, TypeMemberInfo typeMemberInfo)
    {
      return typeMemberInfo.DefinitionDependencies
        .Concat(typeMemberInfo.BodyDependencies)
        .Select(x => new Dependency { UsageContext = new DependencyInfo(module.Name, typeInfo.NameSpace, typeInfo.Name, typeMemberInfo.Name), UsedItem = x });
    }

    private static IEnumerable<Dependency> GetTypeDefinitionDependencies(ModuleInfo module, TypeInfo typeInfo)
    {
      return typeInfo.DefinitionDependencies
        .Select(x => new Dependency { UsageContext = new DependencyInfo(module.Name, typeInfo.NameSpace, typeInfo.Name, string.Empty), UsedItem = x });
    }

    private static void WriteToFile(FileInfo file, IEnumerable<Dependency> dependencies)
    {
      File.Delete(file.FullName);
      File.AppendAllLines(file.FullName,
        new[] { "Used in namespace\tUsed in type\tUsed in type member\tUsage assembly\tUsed namespace\tUsed type\tUsed type member" });

      File.AppendAllLines(file.FullName, dependencies.Select(x =>
        $"{x.UsageContext.NameSpace}\t{x.UsageContext.Type}\t{x.UsageContext.TypeMember}\t{x.UsedItem.Assembly}\t{x.UsedItem.NameSpace}\t{x.UsedItem.Type}\t{x.UsedItem.TypeMember}"));
    }

    private class Dependency : IEquatable<Dependency>
    {
      public DependencyInfo UsageContext { get; set; }
      public DependencyInfo UsedItem { get; set; }

      #region IEquitable

      public bool Equals(Dependency other)
      {
        if (ReferenceEquals(null, other))
          return false;
        if (ReferenceEquals(this, other))
          return true;
        return Equals(UsageContext, other.UsageContext) &&
               Equals(UsedItem, other.UsedItem);
      }

      public override bool Equals(object obj)
      {
        if (ReferenceEquals(null, obj))
          return false;
        if (ReferenceEquals(this, obj))
          return true;
        if (obj.GetType() != this.GetType())
          return false;
        return Equals((Dependency)obj);
      }

      public override int GetHashCode()
      {
        unchecked
        {
          return ((UsageContext != null ? UsageContext.GetHashCode() : 0) * 397)
                 ^ (UsedItem != null ? UsedItem.GetHashCode() : 0);
        }
      }

      #endregion
    }
  }
}
