using StructureMap;

namespace Wiki.Dependencies
{
    public static class IoC
    {
        private static IContainer _container;

        public static IContainer Container => _container ?? (_container = Initialize());

        public static IContainer Initialize()
        {
            _container = new Container(c => c.AddRegistry<IocRegistry>());
            return _container;
        }
    }
}
