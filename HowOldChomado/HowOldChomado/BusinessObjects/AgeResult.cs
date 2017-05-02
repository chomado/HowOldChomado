using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HowOldChomado.BusinessObjects
{
    // FaceAPI から返ってきたデータのうち、必要なもの
    public class AgeResult
    {
        public int Age { get; set; }
        public FaceRectangle Rectangle { get; set; }
    }
}
