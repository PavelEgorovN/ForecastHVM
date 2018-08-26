using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ForecastHVM.Models;
using ForecastHVM.Data;

namespace ForecastHVM.Controllers
{
    public class HomeController : Controller
    {
        IOperations Operations;
        BaseModel[] baseModel;
        RelatedData RelateData;

        public HomeController(IOperations operations)
        {
            Operations = operations;
            baseModel = SampleData.Initialize();
            RelateData = new RelatedData();

        }
        public IActionResult Index()
        {
            BaseModel[] SortInputData = Operations.SortAnArray(baseModel);
            ExpectedModel[] BaseModel = Operations.GetNewArrayAndMigrationInputData(SortInputData);
            BaseModel = Operations.GetCoefficientSeriesAndTrend(BaseModel, RelateData);
            BaseModel = Operations.GetSeasoningSmoothingFactor(RelateData, BaseModel);
            BaseModel = Operations.GetModelEstimationForecast(RelateData, BaseModel);
            BaseModel = Operations.GetModelError(RelateData, BaseModel);
            BaseModel = Operations.GetErrorDeviationFromForecast(RelateData, BaseModel);
            double ForecastAccuracy = Operations.GetForecastAccuracy(BaseModel);
            ExpectedModel[] ForecastModel = Operations.GetForecastModel(BaseModel, RelateData);
            double ForecasRevenueForTheLastYear = Operations.GetForecastRevenueForTheLastYear(ForecastModel);
            double GARP5 = Operations.GetGARPValue(ForecastModel);

            IndexViewModel indexViewModel = new IndexViewModel
            {
                ForecastAccuracy = ForecastAccuracy,
                FocasRevenueForTheLastYear = ForecasRevenueForTheLastYear,
                GARP5 = GARP5,
                ExpectedModelData = ForecastModel.ToList()
            };

            return View(indexViewModel);
        }
    }
}
