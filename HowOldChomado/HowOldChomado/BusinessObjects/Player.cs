using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HowOldChomado.BusinessObjects
{
    public class Player
    {
        public int Id { get; set; }
        public string FaceId { get; set; }
        public byte[] Picture { get; set; }
        public string DisplayName { get; set; }
        public int Age { get; set; }
    }
}
