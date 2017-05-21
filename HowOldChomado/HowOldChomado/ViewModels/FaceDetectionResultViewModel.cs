using HowOldChomado.BusinessObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HowOldChomado.ViewModels
{
    public class FaceDetectionResultViewModel
    {
        public FaceDetectionResult FaceDetectionResult { get; set; }
        public Player Player { get; set; }
    }
}
