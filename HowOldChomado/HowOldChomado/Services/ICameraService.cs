using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HowOldChomado.Services
{
    public interface ICameraService
    {
        Task<byte[]> TakePhotosAsync();
    }
}
