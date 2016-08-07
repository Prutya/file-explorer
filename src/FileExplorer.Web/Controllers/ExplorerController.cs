using System.Collections.Generic;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;

namespace FileExplorer.Web.Controllers
{
    [Route("api/[controller]")]
    public class ExplorerController : Controller
    {
        private readonly IHostingEnvironment _hostingEnvironment;

        public ExplorerController(IHostingEnvironment hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment;
        }

        [HttpGet]
        public FileExplorer.Web.Models.CurrentLocation Get()
        {
            var path = Request.Query["path"];
            if (string.IsNullOrEmpty(path))
            {
                path = _hostingEnvironment.WebRootPath;
            }
            var directoryPaths = Directory.GetDirectories(path);
            var filePaths = Directory.GetFiles(path);

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

            var result = new FileExplorer.Web.Models.CurrentLocation
            {
                Directories = directories.ToArray(),
                Files = files.ToArray(),
                Path = path,
                ParentPath = Directory.GetParent(path).FullName
            };

            return result;
        }
    }
}