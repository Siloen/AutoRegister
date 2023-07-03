using AutoRegister;

namespace AutoRegisterTest.Services
{
    [Transient]
    public class LocalTransientService : ILocalService
    {
        public bool IsPositive(int i)
        {
            return i > 1;
        }
    }
}