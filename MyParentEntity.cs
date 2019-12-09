using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using Microsoft.EntityFrameworkCore;

namespace EFCoreSelectManyTest
{
    [Table("Parents")]
    internal class MyParentEntity
    {
        public MyParentEntity(
            long id)
        {
            Id = id;
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public long Id { get; internal set; }

        public ICollection<MyChildEntity> Children { get; internal set; }
            = null!;

        public static void OnModelCreating(ModelBuilder modelBuilder)
            => modelBuilder.Entity<MyParentEntity>();
    }
}
