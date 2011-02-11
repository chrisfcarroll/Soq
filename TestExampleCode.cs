using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gallio.Framework;
using MbUnit.Framework;
using MbUnit.Framework.ContractVerifiers;
using Moq;

namespace WarringtonSoftwareLtd
{
    public class SomeDependency  : Object {}
    public class OtherDependency : Object 
    {
        public static OtherDependency GetOtherDependency() { return new OtherDependency() ; }
    }

    public class MyClassThatDependsonSomething  
    {  
	    public SomeDependency  D1 { get; set; }
	    public OtherDependency D2 { get; set; }

        public MyClassThatDependsonSomething()
        {
            D1 = Soq.Quote( new SomeDependency() );  
            D2 = Soq.Quote( OtherDependency.GetOtherDependency() );  
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
}
