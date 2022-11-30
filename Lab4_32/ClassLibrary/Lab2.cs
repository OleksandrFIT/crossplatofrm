namespace ClassLibrary
{
    public class Lab2
    {
        /// <summary>
        /// Запуск алгоритму пошуку який гравець виграє згідно умов
        /// </summary>
        /// <param name="inputData"></param>
        /// <returns></returns>
        public static string RunLab(List<string> inputData)
        {
            var data = inputData.First().Split(' ', StringSplitOptions.RemoveEmptyEntries);
            int m = int.Parse(data[0]);
            int n = int.Parse(data[1]);
            if (m < 1 || n < 1 || m > 250 || n > 250 || m == 1 && n == 1)
            {
                return "Out of range exception!";

            }
            else
            {
                var numPlayerWin = CheckPlayerWin(m, n);
                return $"{numPlayerWin}";
            }
        }
        /// <summary>
        /// Алгоритм пошуку який гравець виграє
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        static int CheckPlayerWin(int x, int y)
        {
            if (x > y)
                (x, y) = (y, x);

            var listY = new HashSet<int>();
            int temp_x = 0;
            int temp_y = 0;
            for (int i = 1; i <= x; i++)
            {
                if (listY.Contains(i)) continue;
                temp_x = i;
                temp_y = temp_x + listY.Count;

                Console.WriteLine($"{temp_x} {temp_y}");
                listY.Add(temp_y);
            }
            return (temp_x == x && temp_y == y) ? 2 : 1;
        }
    }
}