using System.Collections.Generic;
using System.IO;
using FileExplorer.Web.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;

namespace FileExplorer.Web.Controllers
{
    [Route("api/[controller]")]
    public class ExplorerController : Controller
    {
        private const int _smallFileSize = 10 * 1024 * 1024;
        private const int _mediumFileSize = 50 * 1024 * 1024;
        private const int _largeFileSize = 100 * 1024 * 1024;
        private readonly IHostingEnvironment _hostingEnvironment;
        private CurrentLocation _currentLocation;

        public ExplorerController(IHostingEnvironment hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment;
        }

        [HttpGet]
        public CurrentLocation Get()
        {
            _currentLocation = new CurrentLocation();

            var path = Request.Query["path"].ToString();

            if (string.IsNullOrEmpty(path))
            {
                path = _hostingEnvironment.WebRootPath;
            }

            _currentLocation.Path = path;
            var parentDir = System.IO.Directory.GetParent(path);
            _currentLocation.ParentPath = parentDir == null ? path : System.IO.Directory.GetParent(path).FullName;

            var directoryPaths = System.IO.Directory.GetDirectories(path);
            var filePaths = System.IO.Directory.GetFiles(path);

            var files = new List<FileExplorer.Web.Models.File>();
            foreach (var fp in filePaths)
            {
                var fileInfo = new FileInfo(fp);
                files.Add(new FileExplorer.Web.Models.File
                {
                    Path = fp,
                    Name = fileInfo.Name,
                    Size = fileInfo.Length
                });
            }

            var directories = new List<FileExplorer.Web.Models.Directory>();
            foreach (var dp in directoryPaths)
            {
                var name = (new DirectoryInfo(dp)).Name;
                directories.Add(new FileExplorer.Web.Models.Directory
                {
                    Path = dp,
                    Name = name
                });
            }

            _currentLocation.Directories = directories.ToArray();
            _currentLocation.Files = files.ToArray();
            _currentLocation.FilesNumber = CountFiles(new DirectoryInfo(path));

            return _currentLocation;
        }

        private FilesNumber CountFiles(DirectoryInfo directory)
        {
            var filesNumber = new FilesNumber();

            var files = directory.EnumerateFiles();

            foreach (var file in files)
            {
                if (file.Length > _largeFileSize)
                {
                    filesNumber.Large++;
                }                
                else if (file.Length >= _smallFileSize && file.Length <= _mediumFileSize)
                {
                    filesNumber.Medium++;
                }
                else if (file.Length <= _smallFileSize)
                {
                    filesNumber.Small++;
                }
            }

            var subDirs = directory.EnumerateDirectories();
            foreach (var dir in subDirs)
            {
                filesNumber += CountFiles(dir);
            }

            return filesNumber;
        }
    }
}