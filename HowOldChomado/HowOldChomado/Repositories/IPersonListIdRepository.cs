using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HowOldChomado.Repositories
{
    public interface IPersonListIdRepository
    {
        Task<string> GetIdAsync();
    }
}
