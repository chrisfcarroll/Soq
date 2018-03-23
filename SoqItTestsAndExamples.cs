using System;
using System.Collections.Generic;
using NUnit.Framework;

namespace SoqIt
{
    [TestFixture]
    public class WhenADependencyIsSoqIted
    {
        [Test]
        public void SoqReturnsTheOriginalInstance()
        {
            Soq.SwapSoqs();
            
            var original = new SomeDependency();
            var soqIted= Soq.It(original);
            
            Assert.AreSame(original,soqIted);
        }
    }
    
    [TestFixture]
    public class WhenAFakeSoqIsUsed
    {
        class FakeSoq : Soq
        {
            protected override T SwapIt<T>(T dependency) => Fakes.ContainsKey(typeof(T)) ? (T)Fakes[typeof(T)] : base.SwapIt(dependency);
            public FakeSoq Setup<T>(T instance){Fakes.Add(typeof(T), instance); return this;}
            readonly Dictionary<Type,object> Fakes= new Dictionary<Type, object>();
        }
        
        [Test]
        public void SoqIt_ReturnsFakeDependencies()
        {
            SomeDependency someDependency = new FakeDependency();
            
            Soq.SwapSoqs(new FakeSoq().Setup(someDependency));
            
            var myClass = new MyClassThatDependsonSomething();
            var dependency = myClass.D1;

            Assert.AreSame( someDependency , myClass.D1);
        }
        class FakeDependency : SomeDependency{}
    }

    [TestFixture]
    public class SoqsCanBeLocked
    {
        [Test]
        public void ALockedSoqCannotBeReplaced()
        {
            try
            {
                Soq.SwapSoqs();
                Soq.Lock();
                Assert.Throws<InvalidOperationException>(() => Soq.SwapSoqs(new Soq()));
            }
            finally { Soq.IsLocked = false; }
        }
    }
   
    public class SomeDependency{}

    public class OtherDependency
    {
        public static OtherDependency GetOtherDependency() => new OtherDependency();
    }

    public class MyClassThatDependsonSomething
    {
        public MyClassThatDependsonSomething()
        {
            D1 = Soq.It(new SomeDependency());
            D2 = Soq.It(OtherDependency.GetOtherDependency());
        }

        internal SomeDependency D1 { get; }
        OtherDependency D2 { get; }
    }    
}