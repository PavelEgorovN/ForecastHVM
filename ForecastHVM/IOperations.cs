using ForecastHVM.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ForecastHVM
{
   public interface IOperations
    {
        BaseModel[] SortAnArray(BaseModel[] viewModel);
        ExpectedModel[] GetNewArrayAndMigrationInputData(BaseModel[] revenues);
        ExpectedModel[] GetCoefficientSeriesAndTrend(ExpectedModel[] viewModels, RelatedData relatedData);
        ExpectedModel[] GetSeasoningSmoothingFactor(RelatedData relatedData, ExpectedModel[] ViewModels);
        ExpectedModel[] GetModelEstimationForecast(RelatedData relatedData, ExpectedModel[] ViewModels);
        ExpectedModel[] GetModelError(RelatedData relatedData, ExpectedModel[] ViewModels);
        ExpectedModel[] GetErrorDeviationFromForecast(RelatedData relatedData, ExpectedModel[] ViewModels);
        double GetForecastAccuracy(ExpectedModel[] ViewModels);
        ExpectedModel[] GetForecastModel(ExpectedModel[] ViewModel, RelatedData relatedData);
        double GetForecastRevenueForTheLastYear(ExpectedModel[] ForecastviewModels);
        double GetGARPValue(ExpectedModel[] viewModels);
    }
}
