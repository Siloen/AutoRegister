using AutoRegister;

namespace AutoRegisterTest.Services
{
    [Scoped]
    public class LocalScopeService : ILocalService
    {
        public bool IsPositive(int i)
        {
            return i > 1;
        }
    }
}