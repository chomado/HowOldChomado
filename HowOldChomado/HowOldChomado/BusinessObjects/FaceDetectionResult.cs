using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HowOldChomado.BusinessObjects
{
    // 「こいつは誰だ」判定の結果
    public class FaceDetectionResult
    {
        public string FaceId { get; set; }
        public int Age { get; set; }
        public FaceRectangle Rectangle { get; set; }
    }
}
