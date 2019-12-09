using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using Microsoft.EntityFrameworkCore;

namespace EFCoreSelectManyTest
{
    [Table("Children")]
    internal class MyChildEntity
    {
        public MyChildEntity(
            long id,
            long parentId)
        {
            Id = id;
            ParentId = parentId;
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public long Id { get; internal set; }

        [ForeignKey(nameof(Parent))]
        public long ParentId { get; }

        public MyParentEntity Parent { get; internal set; }
            = null!;

        public ICollection<MyChildVersionEntity> Versions { get; internal set; }
            = null!;

        public static void OnModelCreating(ModelBuilder modelBuilder)
            => modelBuilder.Entity<MyChildEntity>();
    }
}
