using System.Diagnostics;
using System.IO;

namespace ClassLibrary
{
    public class Lab1
    {
        /// <summary>
        /// Запуск лабораторної з пошуку найкращої вартості продажу волосся
        /// </summary>
        /// <param name="inputData"></param>
        /// <returns></returns>
        public static string RunLab(List<string> inputData)
        {
            int numberOfDays = Convert.ToInt32(inputData?[0].Trim() ?? "0");

            if (numberOfDays <= 0 || numberOfDays > 100)
            {
                return "Out of range exception!";
            }
            else
            {
                List<int> prices = inputData?[1].Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(pr => Convert.ToInt32(pr)).ToList() ?? new List<int>();

                int bestPrice = FindBestHairPrice(prices);
                return bestPrice.ToString();
            }
        }

        /// <summary>
        /// Пошук найкращої вартості продажу волосся
        /// </summary>
        /// <param name="pricesByDay"></param>
        /// <returns></returns>
        static int FindBestHairPrice(List<int> pricesByDay)
        {
            int bestPrice = 0;
            if (pricesByDay.Count > 0)
            {
                int maxPrice = pricesByDay.Max();
                int day = pricesByDay.IndexOf(maxPrice) + 1;

                bestPrice = maxPrice * day + FindBestHairPrice(pricesByDay.Skip(day).ToList());
            }
            return bestPrice;
        }
    }
}