

namespace NugetyCore
{
    public class NugetyOptions
    {
        private NugetyCatalog _catalog;
        public NugetyOptions(NugetyCatalog catalog)
        {
            _catalog = catalog;
        }

        public string FileNameFilterPattern { get; set; }

        public string ModuleNameFilterPattern { get; set; }


        public virtual NugetyCatalog SetFileNameFilterPattern(string pattern)
        {
            this.FileNameFilterPattern = pattern;
            return _catalog;
        }

        public virtual NugetyCatalog SetModuleNameFilterPattern(string pattern)
        {
            this.ModuleNameFilterPattern = pattern;
            return _catalog;
        }
    }
}
