using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ForecastHVM
{
    public class ExpectedModel : BaseModel
    {
        //темп роста инвистиции в течении более одного года
        public int CompoundAnnualGrowthRate { get; set; }
        //Коэффициент сглаживания ряда
        public double CoefficientOfSmoothingTheSeries { get; set; }
        //Коэффициент сглаживания тренда
        public double CoefficientOfSmoothingTheTrend { get; set; }
        //Коэффициент сглаживания сезонности
        public double SeasoningSmoothingFactor { get; set; }
        //Прогноз оценки модели
        public double ModelEstimationForecast { get; set; }
        //Ошибка Модели
        public double ModelError { get; set; }
        //Отклонение ошибки от прогноза
        public double ErrorDeviationFromForecast { get; set; }

    }
}
