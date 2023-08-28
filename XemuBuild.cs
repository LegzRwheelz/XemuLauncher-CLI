namespace XemuLauncher_CLI
{
    public class XemuBuild
    {
        public XemuBuild(string Name, string URL, string FolderName, string ExecutableName, string ZipName)
        {
            this.Name = Name;
            this.URL = URL;
            this.FolderName = FolderName;
            this.ZipName = ZipName;
            this.ExecutableName = ExecutableName;
        }
        public string Name { get; set; }
        public string URL { get; set; }
        public string FolderName { get; set; }
        public string ZipName { get; set; }
        public string ExecutableName { get; set; }

        public override string ToString()
        {
            return $"Xemu {Name}";
        }
    }
}
