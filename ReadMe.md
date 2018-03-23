SoqIt
---

SoqIt — Simply Object Quote It — is a 3-lines-of-code solution to the problem of dependencies vs. unit-testable code. 
It is an alternative to dependency injection, service location and the humble object pattern for making old code more 
testable, but its real competitor is probably poor-man's dependency injection.

Use it by wrapping constructor calls with `Soq.It( ... )`;

Simple Object Quotation is described at http://www.cafe-encounter.net/p157/object-quotation

A distinctive feature is, that it's so simple that you can write a complete implementation from scratch in less time 
that you can read the quick start intro to a DI container. 
This C# implementation has 3 lines of significant code so it should take you all of 2 or 3 minutes to rewrite it 
entirely in your own favourite language.

When not to use object quotation: when you really want DI. But if you just want decoupling for testability, then here it is.

Example Usage
-------------

See https://github.com/xcarroll/Soq/blob/master/TestExampleCode.cs for the full example usage.

```
public class MyClassThatDependsonSomething
{
    public MyClassThatDependsonSomething()
    {
        D1 = Soq.It(new SomeDependency());
        D2 = Soq.It(OtherDependency.GetOtherDependency());
    }
    internal SomeDependency D1 { get; }
    internal OtherDependency D2 { get; }
}    
```

```
[TestFixture]
public class WhenIUseAFakeSoqInATest
{
    [Test]
    public void ThenICanTestMyClassUsingFakeDependencies()
    {
        SomeDependency someDependency = new FakeDependency();
        
        Soq.SwapSoqs(new FakeSoq().Setup(someDependency));
        
        var myClass = new MyClassThatDependsonSomething();
        var dependency = myClass.D1;

        Assert.AreSame( someDependency , myClass.D1);
    }
    
    class FakeDependency : SomeDependency{}
    
    class FakeSoq : Soq
    {
        protected override T SwapIt<T>(T dependency) => Fakes.ContainsKey(typeof(T)) ? (T)Fakes[typeof(T)] : base.SwapIt(dependency);
        public FakeSoq Setup<T>(T instance){Fakes.Add(typeof(T), instance); return this;}
        readonly Dictionary<Type,object> Fakes= new Dictionary<Type, object>();
    }
}
```
