using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HowOldChomado.BusinessObjects
{
    class ScoreHistory
    {
        public int Id { get; set; }
        public int PlayerId { get; set; }
        public int Age { get; set; }
        public DateTime Date { get; set; }
    }
}
