using System.Collections.Generic;
using System.IO;
using Microsoft.AspNetCore.Mvc;

namespace FileExplorer.Web.Controllers
{
    [Route("api/[controller]")]
    public class ExplorerController : Controller
    {
        [HttpGet]
        public FileExplorer.Web.Models.DirectoryInfo Get()
        {
            var path = Request.Query["path"];
            var directories = Directory.GetDirectories(path);
            var filePaths = Directory.GetFiles(path);

            var files = new List<FileExplorer.Web.Models.File>();
            foreach (var fp in filePaths)
            {
                var size = (new FileInfo(fp)).Length;
                files.Add(new FileExplorer.Web.Models.File{Path = fp, Size = size});
            }

            var result = new FileExplorer.Web.Models.DirectoryInfo
            {
                Directories = directories,
                Files = files.ToArray()
            };

            return result;
        }
    }
}