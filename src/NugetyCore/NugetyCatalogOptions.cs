namespace Nugety
{
    public class NugetyOptions
    {
        private readonly NugetyCatalog _catalog;

        public NugetyOptions(NugetyCatalog catalog)
        {
            _catalog = catalog;
            this.IgnoreLoaderExceptions = true;
        }

        public string ModuleFileNameFilterPattern { get; set; }

        public string ModuleNameFilterPattern { get; set; }

        public bool IgnoreLoaderExceptions { get; set; }

        public virtual NugetyCatalog SetModuleFileNameFilter(string pattern)
        {
            this.ModuleFileNameFilterPattern = pattern;
            return _catalog;
        }

        public virtual NugetyCatalog SetModuleNameFilter(string pattern)
        {
            this.ModuleNameFilterPattern = pattern;
            return _catalog;
        }

        public virtual NugetyCatalog SetIgnoreLoaderExceptions(bool ignoreLoaderExceptions)
        {
            this.IgnoreLoaderExceptions = ignoreLoaderExceptions;
            return _catalog;
        }
    }
}