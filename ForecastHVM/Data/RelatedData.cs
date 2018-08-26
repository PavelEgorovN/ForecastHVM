using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ForecastHVM.Data
{
    public class RelatedData
    {
        public double A10 { get; set; } = 1.0;
        public double K { get; set; } = 0.70;
        public double B { get; set; } = 0.15;
        public double Q { get; set; } = 0.1;

        //Forecast
        public int First = 1;
      
    }
}
