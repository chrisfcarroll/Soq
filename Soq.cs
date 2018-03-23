using System;

namespace SoqIt
{
    /// <summary>
    /// Simple Object Quotation : See <see cref="http://www.cafe-encounter.net/p157/object-quotation" />
    /// </summary>
    public class Soq
    {
        static Soq instance = new Soq();

        /// <summary>Put a 'SoqIt()' round calls that construct your dependencies to make them swappable at Test-time.</summary>
        /// <typeparam name="T">The type of the dependancy interface or class</typeparam>
        /// <param name="instance">The instantiated T to use in production</param>
        /// <returns>
        /// In a production environment - that is, when you have not called <seealso cref="SwapSoqs"/> - this method is a simple
        /// passthrough and returns <paramref name="instance" />
        /// If the Soq has been configured with some fake or mock Soq, then this method will return the result of calling
        /// the virtual method Soq.SwapIt&lt;T&gt;(instance)
        /// </returns>
        /// <example><code>var someDep = Soq.It( new SomeDependency() );</example>
        public static T It<T>(T instance) => Soq.instance.SwapIt(instance);

        /// <summary>
        /// Setup your fake or mock Soq to return your mock dependencies when this method is called.
        /// Example implementation with Moq as your mock framework :
        /// <code>
        /// //Arrange
        /// SomeDependency dummy = new Mock&lt;SomeDependency&gt;().Object;
        /// var mockSoq = new Mock&lt;Soq&gt;();
        /// mockSoq.Setup(soq => soq.SwapIt&lt;SomeDependency&gt;(It.IsAny&lt;SomeDependency&gt;()))
        ///         .Returns(dummy);
        ///         Soq.ConfigureForTest(mockSoq.Object);
        /// </code>
        /// </summary>
        /// <typeparam name="T">The type of the dependency interface or class</typeparam>
        /// <param name="originalInstance">
        /// The real instance of a T. Typically your mock will simply discard this, and return a mock object instead.
        /// </param>
        /// <returns>Whatever you configure your fake Soq to return</returns>
        protected virtual T SwapIt<T>(T originalInstance) => originalInstance;

        /// <summary>Allows you to inject a fake or mock or alternative Soq which you can configure to return mock objects when <seealso cref="Soq.It{T}"/> is called.</summary>
        /// <param name="replacementSoq">Your fake or mocking Soq</param>
        public static void SwapSoqs(Soq replacementSoq=null)
        {
            if (IsLocked) throw new InvalidOperationException("The Soq may not be configured when ConfigurationIsEnabled is false.");
            instance = replacementSoq ?? new Soq();
        }

        /// <summary>Lock your Soqs to avoid surprises in large and sprawling codebases</summary>
        public static void Lock() => IsLocked = true;
        
        protected internal static bool IsLocked =false;
    }
}