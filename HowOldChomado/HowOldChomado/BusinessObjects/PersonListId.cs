using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HowOldChomado.BusinessObjects
{
    public class PersonListId
    {
        [PrimaryKey]
        public string Id { get; set; }
    }
}
