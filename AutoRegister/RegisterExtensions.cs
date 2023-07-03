using System.Reflection;
using System.Reflection.Metadata;
using Microsoft.Extensions.DependencyInjection;

namespace AutoRegister;

public static class RegisterExtensions
{

    public static RegisterData ScanFromEntryAssemblyForRegisterData(this IServiceCollection serviceCollection, Assembly assembly)
    {
        var executingTypes = new ClassScanner().AllAccessibleClass(assembly,false, false);
        var registerData = new RegisterData(executingTypes, serviceCollection);
        return registerData;
    }
}