using System;
using System.Collections.Generic;
using System.Text;
using Gallio.Framework;
using MbUnit.Framework;
using MbUnit.Framework.ContractVerifiers;
using Moq;

namespace WarringtonSoftwareLtd
{
    [TestFixture]
    public class When_a_soq_is_configured_for_test
    {
        [Test]
        [ExpectedException(typeof(InvalidOperationException))]
        public void A_used_soq_should_throw_if_ignore_not_set()
        {
            Soq.Quote<Object>(new Object());
            Soq.ConfigureForTest(new Soq(), false);
        }

        [Test]
        public void A_used_soq_should_not_throw_if_ignore_is_set()
        {
            Soq.Quote<Object>(new Object());
            Soq.ConfigureForTest(new Soq(), true);
        }
    
    }

    [TestFixture]
    public class When_SoqQuote_is_called
    {
        [Test]
        public void An_unconfigured_Soq_should_return_the_passed_object()
        {
            //A
            Soq.ConfigureForTest(null, true);
            Object o = new DateTime();
            //A
            Object result = Soq.Quote<Object>(o);
            //A
            Assert.AreSame<Object>(o,result);
        }

        [Test]
        public void A_Soq_configured_with_a_moq_should_return_what_the_moq_was_configured_to_return()
        {
            //Arrange
            Object o = "quoted object";
            Object dummy = DateTime.Now;

            var mockSoq = new Mock<Soq>();
            mockSoq.Setup(soq => soq.QuoteImplementation<Object>(o)).Returns(()=>dummy);
            Soq.ConfigureForTest(mockSoq.Object, true);

            // Act
            Object result = Soq.Quote<Object>(o);

            //Assert
            Assert.AreSame<Object>(dummy, result);
        }
    }
}
