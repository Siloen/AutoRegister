using AutoRegister;

namespace AutoRegisterTest.Services
{
    [DoNotAutoRegister]
    public class LocalIgnoredService : ILocalService
    {
        public bool IsPositive(int i)
        {
            return i > 1;
        }
    }
}