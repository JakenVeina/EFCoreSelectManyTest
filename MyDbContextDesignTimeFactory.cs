using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.DependencyInjection;

namespace EFCoreSelectManyTest
{
    public class MyDbContextDesignTimeFactory
        : IDesignTimeDbContextFactory<MyDbContext>
    {
        public MyDbContext CreateDbContext(string[] args)
            => Program.BuildServiceProvider(
                    Program.BuildConfiguration())
                .GetRequiredService<MyDbContext>();
    }
}
