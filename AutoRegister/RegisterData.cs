using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using System.Runtime.Serialization;

namespace AutoRegister;

public class RegisterData
{
    public RegisterData(IEnumerable<Type> considerTypes, IServiceCollection services)
    {
        ConsiderTypes = considerTypes??throw new ArgumentNullException(nameof(considerTypes));
        Services = services ?? throw new ArgumentNullException(nameof(services));
    }

    public IEnumerable<Type> ConsiderTypes { get; set; }

    internal IEnumerable<Type> IgnoreTypes { get; set; } = new List<Type>
    {
        typeof(IDisposable),
        typeof(ISerializable)
    };

    public IServiceCollection Services { get; }

    public RegisterData Where(Func<Type, bool> filterFunc)
    {
        ConsiderTypes = ConsiderTypes.Where(filterFunc);
        return this;
    }

    public RegisterData Ignore<TInterface>()
    {
        this.IgnoreTypes =IgnoreTypes.Append(typeof(TInterface));
        return this;
    }

    public IServiceCollection Inject()
    {
        foreach (var type in ConsiderTypes)
        {
            if (type.GetCustomAttributes(typeof(ServiceLifeAttribute), true).Count() > 1)
                throw new ArgumentException($"Class {type.FullName} has multiple life time attributes");

            var lifeTime = type.GetCustomAttribute<ServiceLifeAttribute>()?.Lifetime ??
                           throw new ArgumentException($"{type.FullName} service life time must be specific");

            var interfaces = type.GetTypeInfo().ImplementedInterfaces;

            foreach (var interfaceType in interfaces.Where(i =>
                         !IgnoreTypes.Contains(i)
                         && i.IsPublic && !i.IsNested))
            {
                Services.Add(new ServiceDescriptor(interfaceType, type, lifeTime));
            }
        }
        return Services;
    }
}