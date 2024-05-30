using InMemorySqliteAssignmentApp.Infrastructure_Data_;

namespace InMemorySqliteAssignmentApi
{
    public static class ServiceProviderExtensions
    {
        public static IServiceProvider UpdateDatabase(this IServiceProvider provider)
        {
            using (var scope = provider.GetRequiredService<IServiceScopeFactory>().CreateScope())
            using (var context = scope.ServiceProvider.GetService<AppDbContext>())
                context.Database.EnsureCreated();

            return provider;
        }
    }
}
