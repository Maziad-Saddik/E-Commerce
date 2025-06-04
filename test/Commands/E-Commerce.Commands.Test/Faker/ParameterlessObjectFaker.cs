using Bogus;
using System.Runtime.CompilerServices;

namespace E_Commerce.Commands.Test.Faker
{
    public class ParameterlessObjectFaker<T> : Faker<T> where T : class
    {
        public ParameterlessObjectFaker()
        {
            CustomInstantiator(_ => Initialize());
        }

        private static T Initialize() =>
            RuntimeHelpers.GetUninitializedObject(typeof(T)) as T ?? throw new TypeLoadException();
    }
}
