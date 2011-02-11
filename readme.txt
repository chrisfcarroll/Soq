Soq is an extremely simple solution to the problem of dependencies when unit testing code.

It implements the object quotation pattern as described at http://www.cafe-encounter.net/p157/object-quotation and as such it competes with dependency injection, service location and the humble object pattern.

Its distinctive feature is, that it's so simple that you can write a complete implementation from scratch in less time that you can read the quick start intro to a DI container.

This is a C# implementation. There are only 8 lines of significant code so it should take you all of 2 or 3 minutes to rewrite it entirely in your own favourite language. If you do, feel free to post in the comments on the home page at http://www.cafe-encounter.net/p157/object-quotation.

When not to use object quotation: if you require real runtime configurability then object quotation is not your answer. But if you just want decoupling and testability, then here it is.

Example Usage

See https://github.com/xcarroll/Soq/blob/master/TestExampleCode.cs for the full example usage.

public class SomeDependency  : Object {}

public class MyClassThatDependsonSomething  
{  
	public SomeDependency  D1 { get; set; }

    public MyClassThatDependsonSomething()
    {
        D1 = Soq.Quote( new SomeDependency() );  
    }
}  

[TestFixture]
public class TestExampleCode
{
    [Test]
    public void Test_that_a_MoqSoq_returns_a_mock_dependency()
    {
        //Arrange  
        SomeDependency mockDependency = new Mock<SomeDependency>().Object;

        var mockSoq = new Mock<Soq>();
        mockSoq.Setup(soq => soq.QuoteImplementation<SomeDependency>(It.IsAny<SomeDependency>()))
                .Returns(mockDependency);
        Soq.ConfigureForTest(mockSoq.Object);

        // Act  
        var myClass = new MyClassThatDependsonSomething();
        var quotedDependency = myClass.D1;

        //Assert  
        Assert.AreSame<Object>(mockDependency, quotedDependency);
    }
}
