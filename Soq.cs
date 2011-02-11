using System;

namespace WarringtonSoftwareLtd
{
    /// <summary>
    /// A simple Object Quote class.
    /// This is intended as a C sharp implementation of the Object Quote Pattern
    /// See <see cref="http://www.cafe-encounter.net/p157/object-quotation"/>
    /// </summary>
    public class Soq
    {
        private static Soq instance = new Soq();

        /// <summary>
        /// Surround or 'Quote' calls that construct or build your dependencies with this method.
        /// Example usage: D1 = Soq.Quote( new SomeDependency() );  
        /// </summary>
        /// <typeparam name="T">The type of the dependancy interface or class</typeparam>
        /// <param name="instantiatedDependency">The instantiated instance of a T that should be used in the production runtime environment</param>
        /// <returns>In a production environment - that is, when you have not called ConfigureForTest - this method is a simple passthrough and will always return <see cref="dependency"/>
        /// If the Soq has been configured with a MoqSoq, then this method will return the result of calling MoqSoq.QuoteImplementation&lt;T&gt;(dependency)
        /// </returns>
        public static T Quote<T>(T instantiatedDependency)
        {
            return instance.QuoteImplementation<T>(instantiatedDependency);
        }

        /// <summary>
        /// Setup your Moq Soq to return your mock dependencies when this method is called.
        /// Example implementation with Moq as your mock framework : 
        /// //Arrange  
        /// SomeDependency dummy = new Mock&lt;SomeDependency&gt;().Object;
        /// var mockSoq = new Mock<Soq>();
        /// mockSoq.Setup(soq => soq.QuoteImplementation&lt;SomeDependency&gt;(It.IsAny&lt;SomeDependency&gt;()))
        ///        .Returns(dummy);
        /// Soq.ConfigureForTest(mockSoq.Object);
        /// </summary>
        /// <typeparam name="T">The type of the dependancy interface or class</typeparam>
        /// <param name="dependency">The instantiated instance of a T. Typically your mock will simpley discard this, and return a mock object instead.</param>
        /// <returns>Whatever you have configured your mock object to return</returns>
        public virtual T QuoteImplementation<T>(T dependency)
        {
            return dependency;
        }

        /// <summary>
        /// Allows you to inject a Moq Soq which you can configure to return mock objects when QuoteImplementation is called.
        /// </summary>
        /// <param name="testSoq">Your Moq Soq</param>
        public static void ConfigureForTest(Soq testSoq)
        {
            if (!ConfigurationIsEnabled)
            {
                throw new InvalidOperationException("The Soq may not bet configured with ConfigurationIsEnabled is false.");
            }
            instance = testSoq ?? new Soq();
        }
        /// <summary>
        /// If you want to lock down your Soqs under certain conditions then implement your lockdown here. If you don't, you can delete this member.
        /// </summary>
        public static bool ConfigurationIsEnabled { get { return true; } }
    }
}
