using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

using Microsoft.ProjectOxford.Face;
using HowOldChomado.BusinessObjects;

namespace HowOldChomado.Services
{
    public class FaceService : IFaceService
    {
        public async Task<IEnumerable<FaceDetectionResult>> DetectFacesAsync(ImageRequest request)
        {
            var client = new FaceServiceClient(subscriptionKey: Secrets.CongnitiveServiceFaceApiKey, apiRoot: Consts.CognitiveServiceFaceApiEndPoint);
            var results = await client.DetectAsync(imageStream: new MemoryStream(request.Image), returnFaceAttributes: new[]
            {
                FaceAttributeType.Age,
            });

            return results.Select(x => new FaceDetectionResult
            {
                FaceId = x.FaceId.ToString(),
                Age = (int)x.FaceAttributes.Age,
                Rectangle = new FaceRectangle
                {
                    Top = x.FaceRectangle.Top,
                    Left = x.FaceRectangle.Left,
                    Width = x.FaceRectangle.Width,
                    Height = x.FaceRectangle.Height,
                }
            });
        }

        public async Task RegisterFaceAsync(string faceId, ImageRequest request)
        {
            var client = new FaceServiceClient(subscriptionKey: Secrets.CongnitiveServiceFaceApiKey, apiRoot: Consts.CognitiveServiceFaceApiEndPoint);

            try
            {
                await client.GetFaceListAsync(faceId);
            }
            catch (FaceAPIException)
            {
                // not found
                await client.CreateFaceListAsync(faceListId: faceId, name: faceId);
            }

            await client.AddFaceToFaceListAsync(faceListId: faceId, imageStream: new MemoryStream(request.Image));
        }
    }
}
