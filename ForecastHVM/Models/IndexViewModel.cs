using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ForecastHVM.Models
{
    public class IndexViewModel
    {
        public double FocasRevenueForTheLastYear { get; set; }
        public double GARP5 { get; set; }
        public double ForecastAccuracy { get; set; }
        public List<ExpectedModel> ExpectedModelData { get; set; }
    }
}
