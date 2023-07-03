using AutoRegister;

namespace AutoRegisterTest.Services
{
    [Scoped]
    public class ClassWithJustIDisposable : IDisposable
    {
        public void Dispose()
        {
        }
    }
}