using HowOldChomado.BusinessObjects;
using HowOldChomado.Services;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HowOldChomado.Repositories
{
    public class PersonListIdRepository : IPersonListIdRepository
    {
        private SQLiteAsyncConnection Connection { get; }

        public PersonListIdRepository(IFileService fileService)
        {
            this.Connection = new SQLiteAsyncConnection(fileService.GetLocalFilePath(Consts.DatabaseFileName));
        }

        public async Task<string> GetIdAsync()
        {
            return (await this.Connection.Table<PersonListId>().FirstAsync()).Id;
        }
    }
}
