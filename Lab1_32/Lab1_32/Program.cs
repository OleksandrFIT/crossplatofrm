namespace Lab1_32
{
    public class Program
    {
        public static string InputFilePath = @"..\..\input.txt";
        public static string OutputFilePath = @"..\..\output.txt";

        static void Main(string[] args)
        {
            FileInfo outputFileInfo = new FileInfo(OutputFilePath);
            List<string> inputData = File.ReadLines(InputFilePath).ToList();
            int numberOfDays = Convert.ToInt32(inputData?[0].Trim()??"0");
            List<int> prices = inputData?[1].Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(pr => Convert.ToInt32(pr)).ToList() ?? new List<int>();
            using (StreamWriter streamWriter = outputFileInfo.CreateText())
            {
                if (numberOfDays <= 0 || numberOfDays > 100)
                {
                    streamWriter.WriteLine("Out of range exception!");
                }
                else
                {
                    streamWriter.WriteLine(GetMaxEarnings(prices));
                }
            }
        }

        public static int GetMaxEarnings(List<int> pricesPerDayList)
        {
            int maxEarnings = 0;
            int hairLength = 1;
            for (int i = 0; i < pricesPerDayList.Count; i++, hairLength++)
            {
                if (!isBiggerOrEqualPriceLater(i, pricesPerDayList))
                {
                    maxEarnings += hairLength*pricesPerDayList[i];
                    hairLength = 0;
                }
            }
            return maxEarnings;
        }

        private static bool isBiggerOrEqualPriceLater(int currentDay, List<int> pricesPerDayList)
        {
            for (int i = currentDay+1; i < pricesPerDayList.Count; i++)
            {
                if (pricesPerDayList[currentDay] <= pricesPerDayList[i])
                {
                    return true;
                }
            }
            return false;
        }
    }
}