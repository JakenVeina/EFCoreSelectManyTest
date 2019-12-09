using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using Microsoft.EntityFrameworkCore;

namespace EFCoreSelectManyTest
{
    [Table("ChildVersions")]
    internal class MyChildVersionEntity
    {
        public MyChildVersionEntity(
            long id,
            long childId,
            string name,
            bool isDeleted,
            long? previousVersionId,
            long? nextVersionId)
        {
            Id = id;
            ChildId = childId;
            Name = name;
            IsDeleted = isDeleted;
            PreviousVersionId = previousVersionId;
            NextVersionId = nextVersionId;
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public long Id { get; internal set; }

        [ForeignKey(nameof(Child))]
        public long ChildId { get; }

        [Required]
        public string Name { get; }

        public bool IsDeleted { get; }

        public long? PreviousVersionId { get; set; }


        public long? NextVersionId { get; set; }

        public MyChildEntity Child { get; internal set; }
            = null!;

        public MyChildVersionEntity? PreviousVersion { get; set; }

        public MyChildVersionEntity? NextVersion { get; set; }

        public static void OnModelCreating(ModelBuilder modelBuilder)
            => modelBuilder.Entity<MyChildVersionEntity>(entityBuilder =>
            {
                entityBuilder
                    .Property(x => x.Name);

                entityBuilder
                    .Property(x => x.IsDeleted);

                entityBuilder
                    .HasOne(x => x.PreviousVersion)
                    .WithOne()
                    .HasForeignKey<MyChildVersionEntity>(x => x.PreviousVersionId);

                entityBuilder
                    .HasOne(x => x.NextVersion)
                    .WithOne()
                    .HasForeignKey<MyChildVersionEntity>(x => x.NextVersionId);
            });
    }
}
