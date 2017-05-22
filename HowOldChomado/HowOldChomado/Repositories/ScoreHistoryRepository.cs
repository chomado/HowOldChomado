using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HowOldChomado.BusinessObjects;
using SQLite;
using HowOldChomado.Services;

namespace HowOldChomado.Repositories
{
    public class ScoreHistoryRepository : IScoreHistoryRepository
    {
        private SQLiteAsyncConnection Connection { get; }

        public ScoreHistoryRepository(IFileService fileService)
        {
            this.Connection = new SQLiteAsyncConnection(fileService.GetLocalFilePath(Consts.DatabaseFileName));
        }

        public async Task AddAsync(ScoreHistory history)
        {
            await this.Connection.InsertAsync(history);
        }

        public async Task<ScoreHistory> FindMaxScoreHistoryByPlayerIdAsync(int playerId)
        {
            var list = await this.Connection
                .QueryAsync<ScoreHistory>(@"SELECT * FROM ScoreHistory WHERE PlayerId = ? AND Age = (SELECT MIN(Age) FROM ScoreHistory WHERE PlayerId = ?)", playerId, playerId);
            return list.FirstOrDefault();
        }
    }
}
