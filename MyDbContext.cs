using System.Linq;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace EFCoreSelectManyTest
{
    public class MyDbContext
        : DbContext
    {
        public MyDbContext(
                DbContextOptions<MyDbContext> options)
            : base(options) { }

        public async Task EnsureSeeded()
        {
            var parentSet = Set<MyParentEntity>();

            if (await parentSet.AnyAsync())
                return;

            await parentSet.AddRangeAsync(
                new MyParentEntity(id: 1),
                new MyParentEntity(id: 2),
                new MyParentEntity(id: 3));

            await SaveChangesAsync();


            var parentVersionSet = Set<MyParentVersionEntity>();

            var parentVersions = new[]
            {
                new MyParentVersionEntity(id: 1,    parentId: 1,    name: "Parent 1",   isDeleted: false,   previousVersionId: null,    nextVersionId: null ),
                new MyParentVersionEntity(id: 2,    parentId: 1,    name: "Parent 1a",  isDeleted: false,   previousVersionId: 1,       nextVersionId: null ),
                new MyParentVersionEntity(id: 3,    parentId: 2,    name: "Parent 2",   isDeleted: false,   previousVersionId: null,    nextVersionId: null ),
                new MyParentVersionEntity(id: 4,    parentId: 3,    name: "Parent 3",   isDeleted: false,   previousVersionId: null,    nextVersionId: null ),
                new MyParentVersionEntity(id: 5,    parentId: 2,    name: "Parent 2",   isDeleted: true,    previousVersionId: 3,       nextVersionId: null ),
                new MyParentVersionEntity(id: 6,    parentId: 3,    name: "Parent 3a",  isDeleted: false,   previousVersionId: 4,       nextVersionId: null ),
                new MyParentVersionEntity(id: 7,    parentId: 3,    name: "Parent 3a",  isDeleted: true,    previousVersionId: 6,       nextVersionId: null ),
                new MyParentVersionEntity(id: 8,    parentId: 3,    name: "Parent 3a",  isDeleted: false,   previousVersionId: 7,       nextVersionId: null ),
                new MyParentVersionEntity(id: 9,    parentId: 1,    name: "Parent 1",   isDeleted: false,   previousVersionId: 2,       nextVersionId: null )
            };
            await parentVersionSet.AddRangeAsync(parentVersions);

            await SaveChangesAsync();

            foreach (var parentVersion in parentVersions)
                parentVersion.NextVersionId = parentVersions.FirstOrDefault(x => x.PreviousVersionId == parentVersion.Id)?.Id;
            await SaveChangesAsync();


            var childSet = Set<MyChildEntity>();

            await childSet.AddAsync(new MyChildEntity(id: 1,   parentId: 1 ));
            await childSet.AddAsync(new MyChildEntity(id: 2,   parentId: 2 ));
            await childSet.AddAsync(new MyChildEntity(id: 3,   parentId: 2 ));
            await childSet.AddAsync(new MyChildEntity(id: 4,   parentId: 1 ));
            await childSet.AddAsync(new MyChildEntity(id: 5,   parentId: 3 ));
            await childSet.AddAsync(new MyChildEntity(id: 6,   parentId: 2 ));
            await childSet.AddAsync(new MyChildEntity(id: 7,   parentId: 3 ));
            await childSet.AddAsync(new MyChildEntity(id: 8,   parentId: 1 ));
            await childSet.AddAsync(new MyChildEntity(id: 9,   parentId: 3 ));

            await SaveChangesAsync();


            var childVersionSet = Set<MyChildVersionEntity>();

            var childVersions = new[]
            {
                new MyChildVersionEntity(id: 1,     childId: 1, name: "Parent 1, Child 1",  isDeleted: false,   previousVersionId: null,    nextVersionId: null ),
                new MyChildVersionEntity(id: 2,     childId: 2, name: "Parent 2, Child 1",  isDeleted: false,   previousVersionId: null,    nextVersionId: null ),
                new MyChildVersionEntity(id: 3,     childId: 3, name: "Parent 2, Child 2",  isDeleted: false,   previousVersionId: null,    nextVersionId: null ),
                new MyChildVersionEntity(id: 4,     childId: 4, name: "Parent 1, Child 2",  isDeleted: false,   previousVersionId: null,    nextVersionId: null ),
                new MyChildVersionEntity(id: 5,     childId: 4, name: "Parent 1, Child 2",  isDeleted: true,    previousVersionId: 5,       nextVersionId: null ),
                new MyChildVersionEntity(id: 6,     childId: 1, name: "Parent 1, Child 1a", isDeleted: false,   previousVersionId: 1,       nextVersionId: null ),
                new MyChildVersionEntity(id: 7,     childId: 5, name: "Parent 3, Child 1",  isDeleted: false,   previousVersionId: null,    nextVersionId: null ),
                new MyChildVersionEntity(id: 8,     childId: 5, name: "Parent 3, Child 1a", isDeleted: false,   previousVersionId: 7,       nextVersionId: null ),
                new MyChildVersionEntity(id: 9,     childId: 6, name: "Parent 2, Child 3",  isDeleted: false,   previousVersionId: null,    nextVersionId: null ),
                new MyChildVersionEntity(id: 10,    childId: 1, name: "Parent 1, Child 1a", isDeleted: true,    previousVersionId: 6,       nextVersionId: null ),
                new MyChildVersionEntity(id: 11,    childId: 7, name: "Parent 3, Child 2",  isDeleted: false,   previousVersionId: null,    nextVersionId: null ),
                new MyChildVersionEntity(id: 12,    childId: 8, name: "Parent 1, Child 3",  isDeleted: false,   previousVersionId: null,    nextVersionId: null ),
                new MyChildVersionEntity(id: 13,    childId: 9, name: "Parent 3, Child 3",  isDeleted: false,   previousVersionId: null,    nextVersionId: null ),
                new MyChildVersionEntity(id: 14,    childId: 1, name: "Parent 1, Child 1a", isDeleted: false,   previousVersionId: 10,      nextVersionId: null ),
                new MyChildVersionEntity(id: 15,    childId: 9, name: "Parent 3, Child 3a", isDeleted: false,   previousVersionId: 13,      nextVersionId: null ),
                new MyChildVersionEntity(id: 16,    childId: 9, name: "Parent 3, Child 3b", isDeleted: false,   previousVersionId: 15,      nextVersionId: null ),
                new MyChildVersionEntity(id: 17,    childId: 6, name: "Parent 2, Child 3a", isDeleted: false,   previousVersionId: 9,       nextVersionId: null ),
                new MyChildVersionEntity(id: 18,    childId: 6, name: "Parent 2, Child 3",  isDeleted: false,   previousVersionId: 17,      nextVersionId: null )
            };
            await childVersionSet.AddRangeAsync(childVersions);

            await SaveChangesAsync();

            foreach (var childVersion in childVersions)
                childVersion.NextVersionId = parentVersions.FirstOrDefault(x => x.PreviousVersionId == childVersion.Id)?.Id;
            await SaveChangesAsync();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            MyParentEntity.OnModelCreating(modelBuilder);
            MyParentVersionEntity.OnModelCreating(modelBuilder);
            MyChildEntity.OnModelCreating(modelBuilder);
            MyChildVersionEntity.OnModelCreating(modelBuilder);
        }
    }

    public static class MyDbContextServiceCollectionExtensions
    {
        public static IServiceCollection AddMyDbContext(this IServiceCollection services, IConfiguration configuration)
            => services.AddDbContext<MyDbContext>(options =>
                options.UseNpgsql(configuration.GetConnectionString("EFCoreSelectManyTest")));
    }
}
