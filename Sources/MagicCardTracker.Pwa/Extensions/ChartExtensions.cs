#nullable enable

using ChartJs.Blazor.Common;
using ChartJs.Blazor.PieChart;
using ChartJs.Blazor.BarChart;
using MagicCardTracker.Pwa.Models;
using System.Collections.Generic;
using MagicCardTracker.Contracts;
using System.Linq;
using System;

namespace MagicCardTracker.Pwa.Extensions
{
    internal static class ChartExtensions
    {
        public static BarConfig AddCollectionValueBySet(
            this BarConfig chartConfig,
            IEnumerable<CollectedCard> cards,
            Currency dominatingCurrency)
        {
            chartConfig.Data.Labels.Clear();
            chartConfig.Data.Datasets.Clear();
            chartConfig.Options = new BarOptions
            {
                Responsive = true,
                MaintainAspectRatio = false,
                Legend = new Legend { Display = false }
            };

            var cardsBySet = cards.GroupBy(c => c.SetCode);
            
            var dataset = new BarDataset<decimal>(
                cardsBySet.Select(c => Math.Round(c.Sum(c => c.GetCollectionValue(dominatingCurrency)), 2)))
            {
                Label = dominatingCurrency.ToCurrencySymbol(),
                BackgroundColor = "#C14219"
            };

            foreach (var set in cardsBySet)
            {
                chartConfig.Data.Labels.Add(set.Key.ToUpper());
            }
            chartConfig.Data.Datasets.Add(dataset);

            return chartConfig;
        }

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
