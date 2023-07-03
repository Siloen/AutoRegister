# A Simple DI AutoRegister For .NET

This is an extension method for `Microsoft.Extensions.DependencyInjection` .Only thing that library do is to scan an assembly and auto register all classes with the `ServiceLifeAttribute` attribute into the dependency injection provider. 

this library has learned a lot from the code of the [AutoRegisterDI](https://github.com/JonPSmith/NetCore.AutoRegisterDi)

## Example - AutoRegister

- Add a `ServiceLifeAttribute` attribute to your class

```c#
[Scoped]
public class MyService : IMyService
{} 
public interface IMyService
{}

```
-  Scan the calling assembly and register
```c#
var assembly = Assembly.GetExecutingAssembly();
var services = new ServiceCollection();
services.ScanFromEntryAssemblyForRegisterData(assembly).Inject();
```

