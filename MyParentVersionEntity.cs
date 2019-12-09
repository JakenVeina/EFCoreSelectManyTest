using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using Microsoft.EntityFrameworkCore;

namespace EFCoreSelectManyTest
{
    [Table("ParentVersions")]
    internal class MyParentVersionEntity
    {
        public MyParentVersionEntity(
            long id,
            long parentId,
            string name,
            bool isDeleted,
            long? previousVersionId,
            long? nextVersionId)
        {
            Id = id;
            ParentId = parentId;
            Name = name;
            IsDeleted = isDeleted;
            PreviousVersionId = previousVersionId;
            NextVersionId = nextVersionId;
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public long Id { get; internal set; }

        [ForeignKey(nameof(Parent))]
        public long ParentId { get; }

        [Required]
        public string Name { get; }

        public bool IsDeleted { get; }

        public long? PreviousVersionId { get; set; }

        public long? NextVersionId { get; set; }

        public MyParentEntity Parent { get; internal set; }
            = null!;

        public MyParentVersionEntity? PreviousVersion { get; set; }

        public MyParentVersionEntity? NextVersion { get; set; }

        public static void OnModelCreating(ModelBuilder modelBuilder)
            => modelBuilder.Entity<MyParentVersionEntity>(entityBuilder =>
            {
                entityBuilder
                    .Property(x => x.Name);

                entityBuilder
                    .Property(x => x.IsDeleted);

                entityBuilder
                    .HasOne(x => x.PreviousVersion)
                    .WithOne()
                    .HasForeignKey<MyParentVersionEntity>(x => x.PreviousVersionId);

                entityBuilder
                    .HasOne(x => x.NextVersion)
                    .WithOne()
                    .HasForeignKey<MyParentVersionEntity>(x => x.NextVersionId);
            });
    }
}
