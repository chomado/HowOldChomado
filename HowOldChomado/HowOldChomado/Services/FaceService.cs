using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

using Microsoft.ProjectOxford.Face;
using HowOldChomado.BusinessObjects;
using HowOldChomado.Repositories;
using Microsoft.ProjectOxford.Face.Contract;

namespace HowOldChomado.Services
{
    public class FaceService : IFaceService
    {
        private IPersonListIdRepository PersonListIdRepository { get; }

        public FaceService(IPersonListIdRepository personListIdRepository)
        {
            this.PersonListIdRepository = personListIdRepository;
        }

        public async Task<IEnumerable<FaceDetectionResult>> DetectFacesAsync(ImageRequest request)
        {
            try
            {
                var client = new FaceServiceClient(subscriptionKey: Secrets.CongnitiveServiceFaceApiKey, apiRoot: Consts.CognitiveServiceFaceApiEndPoint);
                var results = await client.DetectAsync(imageStream: new MemoryStream(request.Image), returnFaceAttributes: new[]
                {
                FaceAttributeType.Age,
            });

                var personListId = await this.PersonListIdRepository.GetIdAsync();
                var identifyResults = (await client.IdentifyAsync(personListId, results.Select(x => x.FaceId).ToArray()))
                    .ToDictionary(x => x.FaceId);

                var l = new List<FaceDetectionResult>();
                foreach (var r in results)
                {
                    IdentifyResult identifyResult = null;
                    identifyResults.TryGetValue(r.FaceId, out identifyResult);
                    var faceDetectionResult = new FaceDetectionResult
                    {
                        FaceId = identifyResult?.Candidates.FirstOrDefault()?.PersonId.ToString() ?? new Guid().ToString(),
                        Age = (int)r.FaceAttributes.Age,
                        Rectangle = new BusinessObjects.FaceRectangle
                        {
                            Top = r.FaceRectangle.Top,
                            Left = r.FaceRectangle.Left,
                            Width = r.FaceRectangle.Width,
                            Height = r.FaceRectangle.Height,
                        }
                    };
                    l.Add(faceDetectionResult);
                }
                return l;
            }
            catch (FaceAPIException)
            {
                return Enumerable.Empty<FaceDetectionResult>();
            }
        }

        public async Task<Guid> CreateFaceAsync(ImageRequest request)
        {
            var personListId = await this.PersonListIdRepository.GetIdAsync();
            var client = new FaceServiceClient(subscriptionKey: Secrets.CongnitiveServiceFaceApiKey, apiRoot: Consts.CognitiveServiceFaceApiEndPoint);
            await this.CreatePersonGroupIsNotExist(client, personListId);

            var r = await client.CreatePersonAsync(personGroupId: personListId, name: "noname");
            await client.AddPersonFaceAsync(personGroupId: personListId, personId: r.PersonId, imageStream: new MemoryStream(request.Image));
            await this.TrainPersonGroupAsync(client, personListId);
            return r.PersonId;
        }

        public async Task AddFaceAsync(string personId, ImageRequest request)
        {
            var personListId = await this.PersonListIdRepository.GetIdAsync();
            var client = new FaceServiceClient(subscriptionKey: Secrets.CongnitiveServiceFaceApiKey, apiRoot: Consts.CognitiveServiceFaceApiEndPoint);
            await this.CreatePersonGroupIsNotExist(client, personListId);

            await client.AddPersonFaceAsync(personGroupId: personListId, personId: Guid.Parse(personId), imageStream: new MemoryStream(request.Image));
            await this.TrainPersonGroupAsync(client, personListId);
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

        private async Task CreatePersonGroupIsNotExist(FaceServiceClient client, string personListId)
        {
            try
            {
                await client.GetPersonGroupAsync(personListId);
            }
            catch (FaceAPIException)
            {
                // not found
                await client.CreatePersonGroupAsync(personGroupId: personListId, name: personListId);
            }
        }

        private async Task TrainPersonGroupAsync(FaceServiceClient client, string personListId)
        {
            await client.TrainPersonGroupAsync(personListId);
            while((await client.GetPersonGroupTrainingStatusAsync(personListId)).Status == Status.Running)
            {
                await Task.Delay(1000);
            }
        }
    }
}
