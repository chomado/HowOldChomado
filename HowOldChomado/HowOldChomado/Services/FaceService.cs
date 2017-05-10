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
        public Task<IEnumerable<AgeResult>> DetectAgeAsync(ImageRequest request)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<FaceDetectionResult>> DetectFacesAsync(ImageRequest request)
        {
            throw new NotImplementedException();
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
