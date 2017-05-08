using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using HowOldChomado.Services;
using Windows.Storage;

namespace HowOldChomado.UWP.Services
{
    public class FileService : IFileService
    {
        public string GetLocalFilePath(string fileName)
        {
            var localFolderPath = ApplicationData.Current.LocalFolder.Path;
            return Path.Combine(localFolderPath, fileName);
        }
    }
}
