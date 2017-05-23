using HowOldChomado.BusinessObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HowOldChomado.Repositories
{
    public interface IPlayerRepository
    {
        Task<IEnumerable<Player>> FindAllAsync();
        Task AddAsync(Player p);
        Task<Player> FindByDisplayNameAsync(string displayName);
        Task<Player> FindByFaceIdAsync(string faceId); 
        Task UpdateAsync(Player player);
        Task DeleteAsync(Player player);
    }
}
