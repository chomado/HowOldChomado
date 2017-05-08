using HowOldChomado.Services;
using System.IO;

namespace HowOldChomado.Droid.Services
{
    public class FileService : IFileService
    {
        public string GetLocalFilePath(string fileName)
        {
            var folderPath = System.Environment.GetFolderPath(folder: System.Environment.SpecialFolder.Personal);
            return Path.Combine(folderPath, fileName);
        }
    }
}
