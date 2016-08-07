namespace FileExplorer.Web.Models
{
    public class CurrentLocation
    {
        public string ParentPath { get; set; }
        public string Path { get; set; }
        public Directory[] Directories { get; set; }
        public File[] Files { get; set; }
    }
}