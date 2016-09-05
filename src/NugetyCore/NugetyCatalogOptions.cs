namespace Nugety
{
    public class NugetyOptions
    {
        private readonly NugetyCatalog _catalog;

        public NugetyOptions(NugetyCatalog catalog)
        {
            _catalog = catalog;
        }

        public string FileNameFilterPattern { get; set; }

        public string ModuleNameFilterPattern { get; set; }


        public virtual NugetyCatalog SetFileNameFilterPattern(string pattern)
        {
            FileNameFilterPattern = pattern;
            return _catalog;
        }

        public virtual NugetyCatalog SetModuleNameFilterPattern(string pattern)
        {
            ModuleNameFilterPattern = pattern;
            return _catalog;
        }
    }
}