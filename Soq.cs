using System;

namespace WarringtonSoftwareLtd
{
    /// <summary>
    /// A simple Object Quote class.
    /// This is intended as a C sharp implementation of the Object Quote Pattern
    /// See <see cref="http://www.cafe-encounter.net/p157/simple-object-quotation"/>
    /// </summary>
    public class Soq
    {
        private static Soq instance = new Soq();
        private static bool isUsed = false;

        public static T Quote<T>(T o)
        {
            isUsed = true;
            return instance.QuoteImplementation<T>(o);
        }

        public virtual T QuoteImplementation<T>(T o)
        {
            return o;
        }

        public static void ConfigureForTest(Soq testSoq, bool ignoreProductionEnvironmentSafetyLock)
        {
            if (isUsed && !ignoreProductionEnvironmentSafetyLock)
            {
                throw new InvalidOperationException("The Soq cannot be configured for test if it has already been called once. Which it has.");
            }
            instance = testSoq ?? new Soq();
        }
    }
}
