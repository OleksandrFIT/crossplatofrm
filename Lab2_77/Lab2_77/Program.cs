namespace Lab2_77
{
    public class Program
    {
        public static string InputFilePath = @"..\..\input.txt";
        public static string OutputFilePath = @"..\..\output.txt";
        public static Dictionary<int, int> NumAndOpposite = new Dictionary<int, int>()
        {
            { 1, 6 },
            { 2, 5 },
            { 3, 4 },
            { 4, 6 },
            { 5, 2 },
            { 6, 1 }
        };
        static void Main(string[] args)
        {
            FileInfo outputFileInfo = new FileInfo(OutputFilePath);
            var inputData = File.ReadLines(InputFilePath);
            List<int> n_m = inputData.First().Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(n => Convert.ToInt32(n.Trim())).ToList();
            var matrix = inputData.Skip(1).Select(arr => arr.Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(num => Convert.ToInt32(num.Trim())).ToArray()).ToArray();
            
            using (StreamWriter streamWriter = outputFileInfo.CreateText())
            {
                if (n_m[0] * n_m[1] > 105 || n_m[0] * n_m[1] < 1 || matrix.SelectMany(n => n).Any(n => Math.Abs(n) > 103))
                {
                    streamWriter.WriteLine("Out of range exception!");
                }
                else
                {
                    streamWriter.WriteLine(GetBiggestScore(matrix));
                }
            }
        }

        public static int GetBiggestScore(int[][] matrix)
        {
            List<int> scoreList = new List<int>();
            for (int k = 1; k <= 6; k++)
            {
                if(matrix.Length != 1)
                {
                    scoreList.Add(getBiggestScore(matrix, k * matrix[0][0], k, 1, 0));
                }
                if (matrix[0].Length != 1)
                {
                    scoreList.Add(getBiggestScore(matrix, k * matrix[0][0], k, 0, 1));
                }
                if (matrix.Length == 1 && matrix[0].Length == 1)
                {
                    scoreList.Add(k * matrix[0][0]);
                }
            }
            return scoreList.Max();
        }

        private static int getBiggestScore(int[][] matrix, int prevScore, int prevEdge, int curRow, int curCol)
        {
            List<int> scoreList = new List<int>();
            bool isLast = curRow == matrix.Length - 1 && curCol == matrix[0].Length - 1;
            for (int k = 1; k <= 6; k++)
            {
                if (k == prevEdge || k == NumAndOpposite[prevEdge])
                {
                    continue;
                }
                if (matrix.Length != curRow + 1)
                {
                    scoreList.Add(isLast ? prevScore + k * matrix[curRow][curCol] : getBiggestScore(matrix, prevScore + k * matrix[curRow][curCol], k, curRow + 1, curCol));
                }
                if (matrix[0].Length != curCol + 1)
                {
                    scoreList.Add(isLast ? prevScore + k * matrix[curRow][curCol] : getBiggestScore(matrix, prevScore + k * matrix[curRow][curCol], k, curRow, curCol + 1));
                }
                if (isLast)
                {
                    scoreList.Add(prevScore + k * matrix[curRow][curCol]);
                }
            }
            return scoreList.Max();
        }
    }
}
