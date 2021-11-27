using Microsoft.Extensions.DependencyInjection;
using Services.Interfaces;
using Services.Services;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;

namespace SirmaConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            ServiceCollection services = new ServiceCollection();
            services.AddScoped<IFileService, FileService>();
            services.AddScoped<FileApplication>();
            ServiceProvider serviceProvider = services.BuildServiceProvider();
            var fileService = serviceProvider.GetService<FileApplication>();

            string currentFolder = Directory.GetCurrentDirectory().Substring(0, Directory.GetCurrentDirectory().LastIndexOf("\\SirmaConsole")) + "\\SirmaWebAPI\\";
            string file = Path.Combine(currentFolder, "wwwroot", "Uploud", "Emp.txt");
            
            string[] lines = System.IO.File.ReadAllLines(file);
            foreach (string line in lines)
            {
                Console.WriteLine(line);
            }

            Console.WriteLine(fileService.getMostPaired(lines));

        }

 
    }
}
