namespace EFCoreSelectManyTest
{
    public class MyChildIdentityViewModel
    {
        public MyChildIdentityViewModel(
            long id,
            string name)
        {
            Id = id;
            Name = name;
        }

        public long Id { get; }

        public string Name { get; }
    }
}
