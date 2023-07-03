using AutoRegister;

namespace AutoRegisterTest.Services
{
    [Singleton]
    public class LocalSingletonService : ILocalService
    {
        public bool IsPositive(int i)
        {
            return i > 1;
        }
    }
}