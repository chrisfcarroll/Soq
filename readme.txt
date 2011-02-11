Soq is intended as an extremely simple solution to the problem of dependencies when unit testing code. As such it competes with dependency injection, service location and the humble object pattern.
But it is so simple that you could write a complete implementation from scratch in less time that you can download (let alone install and read the quick start guide) a DI framework or service locator.

It implements the object quote pattern as described at http://www.cafe-encounter.net/p157/simple-object-quotation

This is a C# implementation. It should take you all of 2 or 3 minutes to rewrite it entirely in your own favourite language. If you do, feel free to post in the comments on the home page at http://www.cafe-encounter.net/p157/simple-object-quotation.

You can immediately see when not to use it: if you require real runtime configurability then object quotation is not your answer.


Example Usage
//See TestExampleCode.cs for the full code.

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
        public void Test_something_with_dependencies()
        {
            //Arrange  
            SomeDependency dummy = new Mock<SomeDependency>().Object;

            var mockSoq = new Mock<Soq>();
            mockSoq.Setup(soq => soq.QuoteImplementation<SomeDependency>(It.IsAny<SomeDependency>()))
                   .Returns(dummy);
            Soq.ConfigureForTest(mockSoq.Object, true);

            // Act  
            var myClass = new MyClassThatDependsonSomething();
            var result = myClass.D1;

            //Assert  
            Assert.AreSame<Object>(dummy, result);
        }
    }