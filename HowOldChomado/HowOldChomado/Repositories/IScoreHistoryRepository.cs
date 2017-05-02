using HowOldChomado.BusinessObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HowOldChomado.Repositories
{
    public interface IScoreHistoryRepository
    {
        Task<ScoreHistory> FindByPlayerIdAsync(int playerId);
        Task AddAsync(ScoreHistory history);
    }
}
