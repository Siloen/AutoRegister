
using System.Reflection;
using System.Runtime.Serialization;
using AutoRegister;
using AutoRegisterTest.Services;
using Microsoft.Extensions.DependencyInjection;
using TestAssembly;

namespace AutoRegisterTest
{
    public class TestRegisterFromCallingAssembly
    {
        [Fact]
        public void TestAutoRegisterInjectBasicFunction()
        {
            var assembly = Assembly.GetExecutingAssembly();
            var services = new ServiceCollection();
            services.ScanFromEntryAssemblyForRegisterData(assembly).Inject();
            var provider = services.BuildServiceProvider();
            var re= provider.GetRequiredService<ILocalService>().IsPositive(2);
            Assert.True(re);
        }

        [Fact]
        public void TestAutoRegisterMultiImplementation()
        {
            var assembly = Assembly.GetExecutingAssembly();
            var services = new ServiceCollection();
            services.ScanFromEntryAssemblyForRegisterData(assembly).Inject(); 
            var reCount =services.Count(x=>x.ImplementationType != null && x.ImplementationType.FullName!.Contains("Local"));
            Assert.Equal(5,reCount);
            var re1= services.Contains(new ServiceDescriptor(typeof(ILocalService), typeof(LocalService), ServiceLifetime.Transient),
                new CheckServiceDescriptor());
            Assert.True(re1);
            var re2= services.Contains(new ServiceDescriptor(typeof(ILocalService), typeof(LocalSingletonService), ServiceLifetime.Singleton),
                new CheckServiceDescriptor());
            Assert.True(re2);
            var re3 = services.Contains(new ServiceDescriptor(typeof(ILocalService), typeof(LocalScopeService), ServiceLifetime.Scoped),
                new CheckServiceDescriptor());
            Assert.True(re3);
        }

        [Fact]
        public void TestIgnoreCondition()
        {
            var assembly = Assembly.GetExecutingAssembly();
            var services = new ServiceCollection();
            services.ScanFromEntryAssemblyForRegisterData(assembly).Ignore<ILocalService>().Inject();
            var reCount =services.Count(x=>x.ImplementationType != null && x.ImplementationType.FullName!.StartsWith("Local"));
            Assert.Equal(0,reCount);
            var re = services.Contains(new ServiceDescriptor(typeof(IAnotherInterface), typeof(LocalService), ServiceLifetime.Transient),
                new CheckServiceDescriptor());
            Assert.True(re);
            var re1 = services.Contains(new ServiceDescriptor(typeof(IDisposable), typeof(ClassWithJustIDisposable), ServiceLifetime.Scoped));
            Assert.False(re1);
            var re2 = services.Contains(new ServiceDescriptor(typeof(ISerializable), typeof(ClassWithJustISerializable), ServiceLifetime.Scoped));
            Assert.False(re2);
        }

        [Fact]
        public void TestWhereCondition()
        { 
            var assembly = Assembly.GetExecutingAssembly();
            var services = new ServiceCollection();
            services.ScanFromEntryAssemblyForRegisterData(assembly).Where(x=>x.Name.Contains("My")).Inject();
            var reCount =services.Count;
            Assert.Equal(1,reCount);
        }

        [Fact]
        public void TestDoNotRegisterInterface()
        {
            var assembly = Assembly.GetExecutingAssembly();
            var services = new ServiceCollection();
            services.ScanFromEntryAssemblyForRegisterData(assembly).Inject();
            var re = services.Contains(new ServiceDescriptor(typeof(ILocalService), typeof(LocalIgnoredService), ServiceLifetime.Transient),
                new CheckServiceDescriptor());
            Assert.False(re);
        }

        [Fact]
        public void TestNoAttributeInterface()
        {
            var assembly = Assembly.GetExecutingAssembly();
            var services = new ServiceCollection();
            services.ScanFromEntryAssemblyForRegisterData(assembly).Inject();
            var re = services.Count(x=> x.ServiceType == typeof(INoAttributeService));
            Assert.Equal(0,re);
        }

        [Fact]
        public void TestInjectFromOtherAssembly()
        {
            var assembly = Assembly.GetExecutingAssembly();
            var services = new ServiceCollection();
            services.ScanFromEntryAssemblyForRegisterData(assembly).Inject();
            var service = services.BuildServiceProvider().GetRequiredService<IMyService>();
            Assert.Equal("2",service.IntToString(2)); 
        }
    }

    public class CheckServiceDescriptor : IEqualityComparer<ServiceDescriptor>
    {
        public bool Equals(ServiceDescriptor? x, ServiceDescriptor? y)
        {
            return x?.ServiceType == y?.ServiceType
                   && x?.ImplementationType == y?.ImplementationType
                   && x?.Lifetime == y?.Lifetime;
        }

        public int GetHashCode(ServiceDescriptor obj)
        {
            throw new NotImplementedException();
        }
    }

}