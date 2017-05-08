using HowOldChomado.BusinessObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HowOldChomado.Services
{
    public interface IFaceService
    {
        // 画像を渡したら、その画像に誰が写っているかを返してくれるメソッド
        Task<IEnumerable<FaceDetectionResult>> DetectFacesAsync(ImageRequest request);
        // 画像を渡したら、その画像に写っている人が何歳かを返してくれるメソッド
        Task<IEnumerable<AgeResult>> DetectAgeAsync(ImageRequest request);
        // 顔を登録する
        Task RegisterFaceAsync(string faceId, ImageRequest request);
    }
}
