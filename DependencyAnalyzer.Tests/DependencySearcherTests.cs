using DependencyAnalyzer.Models;
using FluentAssertions;
using Mono.Cecil;
using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using TestData;

namespace DependencyAnalyzer.Tests
{
  public class DependencySearcherTests
  {
    private ModuleInfo _module;

    [SetUp]
    public void Setup()
    {
      var path = Path.Combine(Directory.GetCurrentDirectory(), "..", "..", "..", "..", "TestData", "bin", "Debug", "TestData.dll");
      var assembly = new FileInfo(path);
      _module = new DependencySearcher().GetDependency(assembly, new ReaderParameters { AssemblyResolver = new DefaultAssemblyResolver() });
    }

    [TestCase(typeof(ClassDefinition<>), typeof(List<>), typeof(double), typeof(IEquatable<>), typeof(int), typeof(CollectionBase))]
    [TestCase(typeof(StructDefinition<>), typeof(IEquatable<>), typeof(int), typeof(CollectionBase), typeof(ValueType))]
    public void TypeDefinitionTest(System.Reflection.TypeInfo classInfo, params System.Reflection.TypeInfo[] expectedUsedTypes)
    {
      var usedTypes = _module.Types[classInfo.Name].DefinitionDependencies;
      usedTypes.Should().BeEquivalentTo(expectedUsedTypes.Select(x => x.ToDependencyInfo()));
    }

    [TestCase(nameof(ClassMembersDefinition), nameof(ClassMembersDefinition.Pi), typeof(double))]
    [TestCase(nameof(ClassMembersDefinition), "privateField", typeof(DateTime))]
    [TestCase(nameof(ClassMembersDefinition), nameof(ClassMembersDefinition.GenericField), typeof(IEnumerable<>), typeof(Nullable<>), typeof(int))]
    [TestCase(nameof(ClassMembersDefinition), nameof(ClassMembersDefinition.Property), typeof(ValueTuple<,>), typeof(int), typeof(double))]
    [TestCase(nameof(ClassMembersDefinition), nameof(ClassMembersDefinition.Event), typeof(EventHandler<>), typeof(EventArgs))]
    [TestCase(nameof(ClassMembersDefinition), "op_Implicit", typeof(byte), typeof(ClassMembersDefinition))]
    [TestCase(nameof(ClassMembersDefinition), "op_Explicit", typeof(byte), typeof(ClassMembersDefinition))]
    [TestCase(nameof(ClassMembersDefinition), "get_Item", typeof(int), typeof(DateTime), typeof(ClassMembersDefinition))]
    [TestCase(nameof(ClassMembersDefinition) + ".InnerClass", "SomeMethod", typeof(int), "InnerClass")]
    [TestCase(nameof(ClassMembersDefinition), nameof(ClassMembersDefinition.MethodParameters), typeof(TimeSpan), typeof(int), typeof(void), typeof(ClassMembersDefinition))]
    [TestCase(nameof(ClassMembersDefinition), nameof(ClassMembersDefinition.MethodResult), typeof(string), typeof(ClassMembersDefinition))]
    [TestCase(nameof(ClassMembersDefinition), nameof(ClassMembersDefinition.MethodGenericParameters), typeof(Enum), typeof(Delegate), typeof(void), typeof(ClassMembersDefinition))]
    [TestCase(nameof(ClassMembersDefinition), nameof(ClassMembersDefinition.StaticMethod), typeof(IntPtr), typeof(char), typeof(ClassMembersDefinition))]
    [TestCase(nameof(ClassMembersDefinition), ".ctor", typeof(long), typeof(void), typeof(ClassMembersDefinition))]
    public void MethodParametersTest(string typeName, string methodName, params System.Reflection.TypeInfo[] expectedUsedTypes)
    {
      var usedTypes = _module.Types[typeName].Members[methodName].DefinitionDependencies;
      usedTypes.Should().BeEquivalentTo(expectedUsedTypes.Select(x => x.ToDependencyInfo()));
    }

    [TestCase(nameof(Body), nameof(Body.VariableDefinition), typeof(ArrayList))]
    [TestCase(nameof(Body), nameof(Body.GenericVariableDefinition), typeof(GenericClass<>), typeof(Exception), typeof(ArgumentException))]
    [TestCase(nameof(Body), nameof(Body.MethodCall), typeof(GC), typeof(void))]
    [TestCase(nameof(Body), nameof(Body.GenericMethodCall), typeof(Class), typeof(Exception), typeof(ArgumentNullException), typeof(void))]
    [TestCase(nameof(Body), nameof(Body.ObjectCreation), typeof(ArithmeticException), typeof(void))]
    [TestCase(nameof(Body), nameof(Body.GenericObjectCreation), typeof(GenericClass<>), typeof(Exception), typeof(AccessViolationException), typeof(void))]
    [TestCase(nameof(Body), nameof(Body.ValueTypeCreation), typeof(Thread), typeof(TimeSpan), typeof(void))]
    [TestCase(nameof(Body), nameof(Body.GenericValueTypeCreation), typeof(GenericStruct<>), typeof(Exception), typeof(AggregateException), typeof(Console), typeof(void), typeof(object))]
    [TestCase(nameof(Body), nameof(Body.Combination), typeof(double), typeof(TimeSpan), typeof(string), typeof(char), typeof(int), typeof(void))]
    public void ClassMembersBodyTest(string typeName, string methodName, params System.Reflection.TypeInfo[] expectedUsedTypes)
    {
      var usedTypes = _module
        .Types[typeName]
        .Members[methodName]
        .BodyDependencies
        .Select(x => new DependencyInfo(x.Assembly, x.NameSpace, x.Type, null)) //Чтобы сравнивать буз учета TypeMember
        .Distinct();

      usedTypes.Should().BeEquivalentTo(expectedUsedTypes.Select(x => x.ToDependencyInfo()));
    }

    [TestCase(nameof(Body), nameof(Body.MethodCall), typeof(GC), nameof(GC.Collect))]
    [TestCase(nameof(Body), nameof(Body.GenericMethodCall), typeof(Class), nameof(Class.GenericMethod))]
    [TestCase(nameof(Body), nameof(Body.GenericMethodCall), typeof(ArgumentNullException), null)]
    [TestCase(nameof(Body), nameof(Body.ObjectCreation), typeof(ArithmeticException), ".ctor")]
    public void TypeMemberTest(string typeName, string methodName, System.Reflection.TypeInfo type, string typeMember)
    {
      var dependencyInfo = _module
        .Types[typeName]
        .Members[methodName]
        .BodyDependencies
        .FirstOrDefault(x => x.Type == type.Name);

      dependencyInfo.Should().BeEquivalentTo(type.ToDependencyInfo(typeMember));
    }
  }
}