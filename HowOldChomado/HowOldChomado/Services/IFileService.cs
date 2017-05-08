namespace HowOldChomado.Services
{
    // ファイル名から path を返すっていうインターフェイス
    public interface IFileService
    {
        // OSごとに異なるdatabaseのファイルをどこに作るかっていうファイルパス
        string GetLocalFilePath(string fileName);
    }
}