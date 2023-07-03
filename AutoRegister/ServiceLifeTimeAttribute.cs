using Microsoft.Extensions.DependencyInjection;

namespace AutoRegister;

[AttributeUsage(AttributeTargets.Class)]
public class ServiceLifeAttribute:Attribute
{
    public ServiceLifetime Lifetime { get; }

    public ServiceLifeAttribute(ServiceLifetime lifetime)
    {
        Lifetime = lifetime;
    }
}

public class ScopedAttribute : ServiceLifeAttribute
{
    public ScopedAttribute() : base(ServiceLifetime.Scoped)
    {
    }
}

public class SingletonAttribute : ServiceLifeAttribute
{
    public SingletonAttribute() : base(ServiceLifetime.Singleton)
    {
    }
}

public class TransientAttribute : ServiceLifeAttribute
{
    public TransientAttribute() : base(ServiceLifetime.Transient)
    {
    }
}

[AttributeUsage(AttributeTargets.Class)]
public class DoNotAutoRegisterAttribute : Attribute
{ }