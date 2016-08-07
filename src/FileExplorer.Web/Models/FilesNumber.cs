namespace FileExplorer.Web.Models
{
    public class FilesNumber
    {
        public int Small { get; set; }
        public int Medium { get; set; }
        public int Large { get; set; }

        public static FilesNumber operator +(FilesNumber first, FilesNumber second)
        {
            return new FilesNumber
            {
                Small = first.Small + second.Small,
                Medium = first.Medium + second.Medium,
                Large = first.Large + second.Large
            };
        }
    }
}