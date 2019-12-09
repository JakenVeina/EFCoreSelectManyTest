using System.Collections.Generic;

namespace EFCoreSelectManyTest
{
    public class MyParentDetailViewModel
    {
        public MyParentDetailViewModel(
            long id,
            string name,
            IReadOnlyCollection<MyChildIdentityViewModel> children)
        {
            Id = id;
            Name = name;
            Children = children;
        }

        public long Id { get; }

        public string Name { get; }

        public IReadOnlyCollection<MyChildIdentityViewModel> Children { get; }
    }
}
