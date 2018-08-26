using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ForecastHVM.Data;

namespace ForecastHVM
{
    public class Operations : IOperations
    {
        public BaseModel[] SortAnArray(BaseModel[] baseModel)
        {
            BaseModel[] TempBaseModel;
            if (baseModel != null)
            {
                TempBaseModel = baseModel.OrderBy(VM => VM.Month).ToArray();

            }
            else
            {
                throw new ArgumentNullException();
            }
            return TempBaseModel;
        }
        public ExpectedModel[] GetNewArrayAndMigrationInputData(BaseModel[] baseModel)
        {
            ExpectedModel[] newArray = new ExpectedModel[baseModel.Length];
            for (int i = 0; i < baseModel.Length; i++)
            {
                newArray[i] = new ExpectedModel
                {
                    Month = baseModel[i].Month,
                    Year = baseModel[i].Year,
                    Sum = baseModel[i].Sum
                };

            }
            return newArray;

        }
        public ExpectedModel[] GetCoefficientSeriesAndTrend(ExpectedModel[] expectedModel, RelatedData relatedData)
        {
            for (int i = 0; i < expectedModel.Length; i++)
            {
                if (expectedModel[i].Month == 1)
                    expectedModel[i].CoefficientOfSmoothingTheSeries = expectedModel[i].Sum;

                else
                {
                    expectedModel[i].CoefficientOfSmoothingTheSeries = relatedData.K
                        * expectedModel[i].Sum / relatedData.A10
                        + (1 - relatedData.K)
                        * (expectedModel[i - 1].CoefficientOfSmoothingTheSeries
                        + expectedModel[i - 1].CoefficientOfSmoothingTheTrend);
                    expectedModel[i].CoefficientOfSmoothingTheTrend = relatedData.B
                        * (expectedModel[i].CoefficientOfSmoothingTheSeries - expectedModel[i - 1].CoefficientOfSmoothingTheSeries)
                        + (1 - relatedData.B)
                        * expectedModel[i - 1].CoefficientOfSmoothingTheTrend;
                }

            }

            return expectedModel;
        }
        public ExpectedModel[] GetSeasoningSmoothingFactor(RelatedData relatedData, ExpectedModel[] expectedModel)
        {
            for (int i = 0; i < expectedModel.Length; i++)
            {
                if (expectedModel[i].Month < 13)
                {
                    expectedModel[i].SeasoningSmoothingFactor = relatedData.A10;

                }
                else if (expectedModel[i].Month > 12)
                {
                    expectedModel[i].SeasoningSmoothingFactor = relatedData.Q
                        * (expectedModel[i].Sum / expectedModel[i].CoefficientOfSmoothingTheSeries)
                        + (1 - relatedData.Q)
                        * expectedModel[i - 12].SeasoningSmoothingFactor;
                }

            }
            return expectedModel;


        }
        public ExpectedModel[] GetModelEstimationForecast(RelatedData relatedData, ExpectedModel[] expectedModel)
        {
            for (int i = 0; i < expectedModel.Length; i++)
            {
                if (expectedModel[i].Month == 1)
                {
                    expectedModel[i].ModelEstimationForecast = expectedModel[i].CoefficientOfSmoothingTheSeries
                        + expectedModel[i].CoefficientOfSmoothingTheTrend;

                }
                else if (expectedModel[i].Month > 1 && expectedModel[i].Month < 13)
                {
                    expectedModel[i].ModelEstimationForecast = expectedModel[i - 1].CoefficientOfSmoothingTheSeries
                        + expectedModel[i - 1].CoefficientOfSmoothingTheTrend;

                }
                else if (expectedModel[i].Month > 51)
                {
                    expectedModel[i].ModelEstimationForecast = expectedModel[i].Sum;

                }
                else
                {
                    expectedModel[i].ModelEstimationForecast = (expectedModel[i - 1].CoefficientOfSmoothingTheSeries
                        + expectedModel[i - 1].CoefficientOfSmoothingTheTrend)
                        * expectedModel[i - 12].SeasoningSmoothingFactor;

                }
            }
            return expectedModel;

        }
        public ExpectedModel[] GetModelError(RelatedData relatedData, ExpectedModel[] expectedModel)
        {
            for (int i = 0; i < expectedModel.Length; i++)
            {
                if (expectedModel[i].Month < 52)
                {
                    expectedModel[i].ModelError = expectedModel[i].Sum - expectedModel[i].ModelEstimationForecast;

                }

            }
            return expectedModel;
        }
        public ExpectedModel[] GetErrorDeviationFromForecast(RelatedData relatedData, ExpectedModel[] expectedModel)
        {
            for (int i = 0; i < expectedModel.Length; i++)
            {
                if (expectedModel[i].Month == 1)
                {
                    expectedModel[i].ErrorDeviationFromForecast = expectedModel[i].ModelError / expectedModel[i].Sum;

                }
                else if (expectedModel[i].Month < 52)
                {
                    expectedModel[i].ErrorDeviationFromForecast = Math.Pow(expectedModel[i].ModelError, 2) / Math.Pow(expectedModel[i].Sum, 2);
                }
            }
            return expectedModel;

        }
        public double GetForecastAccuracy(ExpectedModel[] expectedModel)
        {
            int CountMonthForecastAccuracy = 51;

            double ForecastAccuracy = 1 - expectedModel.Take(CountMonthForecastAccuracy)
                .Average(ErrorDeviationFromForecast => ErrorDeviationFromForecast.ErrorDeviationFromForecast);

            return ForecastAccuracy;
        }


        public ExpectedModel[] GetForecastModel(ExpectedModel[] expectedModel, RelatedData relatedData)
        {
            int TempCount = relatedData.First;
            ExpectedModel[] TempExpectedModel = expectedModel;
            Array.Resize(ref TempExpectedModel, TempExpectedModel.Length + 5);
            for (int i = 55; i < TempExpectedModel.Length; i++)
            {

                TempExpectedModel[i] = new ExpectedModel()
                {
                    Sum = (TempExpectedModel[50].CoefficientOfSmoothingTheSeries
                    + TempExpectedModel[50].CoefficientOfSmoothingTheTrend * TempCount++)
                    * TempExpectedModel[i - 12].SeasoningSmoothingFactor,

                    Month = TempExpectedModel[i - 1].Month + 1,
                    Year = TempExpectedModel[i - 1].Year

                };

            }
            TempExpectedModel = GetCoefficientSeriesAndTrend(TempExpectedModel, new RelatedData());
            TempExpectedModel = GetSeasoningSmoothingFactor(new RelatedData(), TempExpectedModel);
            TempExpectedModel = GetModelEstimationForecast(new RelatedData(), TempExpectedModel);

            return TempExpectedModel;
        }
        public double GetForecastRevenueForTheLastYear(ExpectedModel[] ForecastExpectedModel)
        {
            ExpectedModel[] TempExpectedModel = ForecastExpectedModel.Skip(48).Take(12).ToArray();
            double result = 0;
            foreach (ExpectedModel model in TempExpectedModel)
            {
                result += model.Sum;
            }
            return result;

        }

        public double GetGARPValue(ExpectedModel[] expectedModel)
        {
            double result = Math.Pow(expectedModel[59].Sum / expectedModel[0].Sum, 0.2) - 1;
            return result;

        }
    }
}
