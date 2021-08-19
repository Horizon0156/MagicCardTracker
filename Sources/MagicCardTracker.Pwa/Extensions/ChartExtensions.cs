#nullable enable
using ChartJs.Blazor.Common;
using ChartJs.Blazor.PieChart;
using MagicCardTracker.Pwa.Models;

namespace MagicCardTracker.Pwa.Extensions
{
    internal static class ChartExtensions
    {
        public static PieConfig AddManaDistribution(
            this PieConfig chartConfig,
            CollectionStatistic statistic)
        {
            chartConfig.Options = new PieOptions
            {
                Responsive = true,
                Legend = new Legend { Display = false }
            };
            chartConfig.Data.Labels.Clear();
            chartConfig.Data.Datasets.Clear();

            foreach (string color in new[] { "White", "Blue", "Black", "Red", "Green" })
            {
                chartConfig.Data.Labels.Add(color);
            }

            var dataset = new PieDataset<int>(
            new[] 
            { 
                statistic.NumberOfWhiteCards,
                statistic.NumberOfBlueCards,
                statistic.NumberOfBlackCards,
                statistic.NumberOfRedCards,
                statistic.NumberOfGreenCards 
            })
            {
                BorderColor = "#25262c",
                BackgroundColor = new[]
                {
                    "#DDCA98", // White
                    "#4594B8", // Blue
                    "#3C352B", // Black
                    "#C14219", // Red
                    "#649349", // Green
                }
            };
            chartConfig.Data.Datasets.Add(dataset);
            
            return chartConfig;
        }
    }
}
