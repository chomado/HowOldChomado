using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using HowOldChomado.Services;

using Foundation;
using UIKit;

namespace HowOldChomado.iOS.Services
{
    public class FileService : IFileService
    {
        public string GetLocalFilePath(string fileName)
        {
            var docFolder = Environment.GetFolderPath(folder: Environment.SpecialFolder.Personal);
            var libFolder = Path.Combine(docFolder, "..", "Library", "Database");

            if (!Directory.Exists(path: libFolder))
            {
                Directory.CreateDirectory(path: libFolder);
            }
            return Path.Combine(libFolder, fileName);
        }
    }
}