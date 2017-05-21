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
        Task<ScoreHistory> FindMaxScoreHistoryByPlayerIdAsync(int playerId);
        Task AddAsync(ScoreHistory history);
    }
}
