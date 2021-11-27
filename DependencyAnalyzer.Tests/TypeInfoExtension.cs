using DependencyAnalyzer.Models;

namespace DependencyAnalyzer.Tests
{
  public static class TypeInfoExtension
  {
    public static DependencyInfo ToDependencyInfo(this System.Reflection.TypeInfo typeInfo)
    {
      return new DependencyInfo(typeInfo.Assembly.GetName().Name, typeInfo.Namespace, typeInfo.Name, null);
    }
    public static DependencyInfo ToDependencyInfo(this System.Reflection.TypeInfo typeInfo, string typeMemberName)
    {
      return new DependencyInfo(typeInfo.Assembly.GetName().Name, typeInfo.Namespace, typeInfo.Name, typeMemberName);
    }
  }
}