using System.Linq;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace EFCoreSelectManyTest
{
    public static class Program
    {
        public static async Task Main()
        {
            var configuration = BuildConfiguration();
            using var serviceProvider = BuildServiceProvider(configuration);

            var context = serviceProvider.GetRequiredService<MyDbContext>();

            await context.Database.MigrateAsync();

            await context.EnsureSeeded();

            // Succeeeds
            var results1 = await context.Set<MyParentVersionEntity>()
                .Where(pv => pv.NextVersionId == null)
                .Select(pv => new
                {
                    pv.ParentId,
                    pv.Name,
                    Children = pv.Parent.Children
                        .SelectMany(c => c.Versions
                            .Where(cv => cv.NextVersionId == null)
                            .Where(cv => !cv.IsDeleted)
                            .Select(cv => new
                            {
                                cv.ChildId,
                                cv.Name
                            }))
                        .ToArray()
                })
                .ToArrayAsync();

            // Succeeeds
            var results2 = await context.Set<MyParentVersionEntity>()
                .Where(pv => pv.NextVersionId == null)
                .Select(pv => new MyParentDetailViewModel(
                    pv.ParentId,
                    pv.Name,
                    pv.Parent.Children
                        .SelectMany(c => c.Versions)
                        .Where(cv => cv.NextVersionId == null)
                        .Where(cv => !cv.IsDeleted)
                        .Select(cv => new MyChildIdentityViewModel(
                            cv.ChildId,
                            cv.Name))
                        .ToArray()))
                .ToArrayAsync();

            // Fails
            var results3 = await context.Set<MyParentVersionEntity>()
                .Where(pv => pv.NextVersionId == null)
                .Select(pv => new MyParentDetailViewModel(
                    pv.ParentId,
                    pv.Name,
                    pv.Parent.Children
                        .SelectMany(c => c.Versions
                            .Where(cv => cv.NextVersionId == null)
                            .Where(cv => !cv.IsDeleted)
                            .Select(cv => new MyChildIdentityViewModel(
                                cv.ChildId,
                                cv.Name)))
                        .ToArray()))
                .ToArrayAsync();
        }

        public static IConfiguration BuildConfiguration()
            => new ConfigurationBuilder()
                .AddUserSecrets<MyDbContext>()
                .Build();

        public static ServiceProvider BuildServiceProvider(IConfiguration configuration)
            => new ServiceCollection()
                .AddLogging(loggingBuilder => loggingBuilder
                    .AddConsole())
                .AddMyDbContext(configuration)
                .BuildServiceProvider();
    }
}
