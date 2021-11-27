using Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SirmaConsole
{
    public class FileApplication
    {
        private readonly IFileService _fileService;
        public FileApplication(IFileService fileService)
        {
            _fileService = fileService;
        }

        public string getMostPaired(string[] lines)
        {
            return _fileService.getMostPaired(lines);
        }
    }
}
