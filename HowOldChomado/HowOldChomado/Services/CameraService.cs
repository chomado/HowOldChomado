using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Plugin.Media;
using System.IO;

namespace HowOldChomado.Services
{
    public class CameraService : ICameraService
    {
        public async Task<byte[]> TakePhotosAsync()
        {
            await CrossMedia.Current.Initialize();

            var picture = await CrossMedia.Current.TakePhotoAsync(new Plugin.Media.Abstractions.StoreCameraMediaOptions());

            if (picture == null)
            {
                return null;
            }

            var ms = new MemoryStream();

            using (var s = picture.GetStream())
            {
                await s.CopyToAsync(destination: ms);
            }

            //ms.Seek(offset: 0, loc: SeekOrigin.Begin);

            return ms.ToArray();
        }
    }
}
