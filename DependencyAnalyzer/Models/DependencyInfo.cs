using JetBrains.Annotations;
using System;

namespace DependencyAnalyzer.Models
{
  public class DependencyInfo : IEquatable<DependencyInfo>
  {
    public string Assembly { get; }

    public string NameSpace { get; }

    public string Type { get; }

    [CanBeNull]
    public string TypeMember { get; }

    public DependencyInfo(string assembly, string nameSpace, string type, [CanBeNull] string typeMember)
    {
      Assembly = assembly;
      NameSpace = nameSpace;
      Type = type;
      TypeMember = typeMember;
    }

    #region IEquitable

    public bool Equals(DependencyInfo other)
    {
      if (ReferenceEquals(null, other))
        return false;

      if (ReferenceEquals(this, other))
        return true;

      return string.Equals(Assembly, other.Assembly, StringComparison.OrdinalIgnoreCase)
             && string.Equals(NameSpace, other.NameSpace, StringComparison.OrdinalIgnoreCase)
             && string.Equals(Type, other.Type, StringComparison.OrdinalIgnoreCase)
             && string.Equals(TypeMember, other.TypeMember, StringComparison.OrdinalIgnoreCase);
    }

    public override bool Equals(object obj)
    {
      if (ReferenceEquals(null, obj))
        return false;

      if (ReferenceEquals(this, obj))
        return true;

      if (obj.GetType() != this.GetType())
        return false;

      return Equals((DependencyInfo)obj);
    }

    public override int GetHashCode()
    {
      unchecked
      {
        var hashCode = (Assembly != null ? Assembly.GetHashCode() : 0);
        hashCode = (hashCode * 397) ^ (NameSpace != null ? NameSpace.GetHashCode() : 0);
        hashCode = (hashCode * 397) ^ (Type != null ? Type.GetHashCode() : 0);
        hashCode = (hashCode * 397) ^ (TypeMember != null ? TypeMember.GetHashCode() : 0);
        return hashCode;
      }
    }

    #endregion
  }
}