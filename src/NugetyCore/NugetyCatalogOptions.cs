namespace Nugety
{
    public class NugetyOptions
    {
        private readonly NugetyCatalog _catalog;

        public NugetyOptions(NugetyCatalog catalog)
        {
            _catalog = catalog;
        }

        public string ModuleFileNameFilterPattern { get; set; }

        public string ModuleNameFilterPattern { get; set; }


        public virtual NugetyCatalog SetModuleFileNameFilter(string pattern)
        {
            this.ModuleFileNameFilterPattern = pattern;
            return _catalog;
        }

        public virtual NugetyCatalog SetModuleNameFilterPattern(string pattern)
        {
            this.ModuleNameFilterPattern = pattern;
            return _catalog;
        }
    }
}