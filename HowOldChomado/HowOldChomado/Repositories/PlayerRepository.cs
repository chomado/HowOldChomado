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
    public class PlayerRepository : IPlayerRepository
    {
        private SQLiteAsyncConnection Connection { get; }

        public PlayerRepository(IFileService fileService)
        {
            // databaseのファイルをどこに作るかっていうファイルパス. 指定の仕方がOSごとに異なる
            var databasePath = fileService.GetLocalFilePath(Consts.DatabaseFileName);

            this.Connection = new SQLiteAsyncConnection(databasePath);
        }

        public async Task<IEnumerable<Player>> FindAllAsync()
        {
            return await this.Connection.Table<Player>()
                             .OrderBy(x => x.Id)
                             .ToListAsync();
        }
        public async Task AddAsync(Player p)
        {
            await this.Connection.InsertAsync(item: p);
        }

        public async Task<Player> FindByDisplayNameAsync(string displayName)
        {
            return await this.Connection.Table<Player>()
                .Where(x => x.DisplayName == displayName)
                .FirstOrDefaultAsync();
        }

        public async Task UpdateAsync(Player player)
        {
            await this.Connection.UpdateAsync(player);
        }
    }
}
