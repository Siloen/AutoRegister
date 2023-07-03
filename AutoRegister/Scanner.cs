using System.Reflection;

namespace AutoRegister
{
    public class ClassScanner
    {
        private IEnumerable<Assembly> ScanAssemblies(Assembly entry, bool removeSystem, bool removeMicrosoft)
        {
            yield return entry;
            var references = entry
                .GetReferencedAssemblies()
                .Filter(removeSystem, t => !t.Name?.StartsWith("System") ?? false)
                .Filter(removeMicrosoft, t => !t.Name?.StartsWith("Microsoft") ?? false);
            foreach (var referenced in references)
            {
                foreach (var scanned in ScanAssemblies(Assembly.Load(referenced), removeSystem, removeMicrosoft))
                {
                    yield return scanned;
                }
            }
        }
        public List<Type> AllAccessibleClass(Assembly assembly, bool includeSystem, bool includeMicrosoft)
        {
            return ScanAssemblies(assembly, !includeSystem, !includeMicrosoft)
                .Distinct()
                .SelectMany(t => t.GetTypes())
                .Where(t => !t.IsAbstract)
                .Where(t => !t.IsNestedPrivate)
                .Where(t => !t.IsGenericType)
                .Where(t => !t.IsInterface)
                .Where(t => t.GetCustomAttribute<ServiceLifeAttribute>(true) != null)
                .Where(t => t.GetCustomAttribute<DoNotAutoRegisterAttribute>(true) == null)
                .ToList();
        }
    }

    public static class ListExtends
    {
        public static IEnumerable<T> AddWith<T>(this IEnumerable<T> input, T toAdd)
        {
            foreach (var item in input)
            {
                yield return item;
            }
            yield return toAdd;
        }

        public static IEnumerable<T> Filter<T>(this IEnumerable<T> dbSet, bool enabled, Func<T, bool> predicate) where T : class
        {
            if (enabled)
            {
                return dbSet.Where(predicate);
            }
            else
            {
                return dbSet;
            }
        }
    }
}
