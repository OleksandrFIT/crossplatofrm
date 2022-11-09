using System.Collections.Generic;
using System.Dynamic;

namespace Lab3_32
{
    public class Program
    {
        public static string InputFilePath = @"..\..\input.txt";
        public static string OutputFilePath = @"..\..\output.txt";

        static void Main(string[] args)
        {
            FileInfo outputFileInfo = new FileInfo(OutputFilePath);
            var inputData = File.ReadLines(InputFilePath);
            var (n, game) = (Convert.ToInt32(inputData.First().Trim()), inputData.Skip(1).Select(row => row.ToCharArray()).ToArray());

            using (StreamWriter streamWriter = outputFileInfo.CreateText())
            {
                if (n < 6 || n > 19)
                {
                    streamWriter.WriteLine("Out of range exception!");
                }
                else
                {
                    streamWriter.WriteLine(GetCountBlackGroupInAtari(game));
                }
            }
        }
        
        /// <summary>
        /// Отримати кількість груп чорного кольору, які знаходяться в атарі
        /// </summary>
        /// <param name="game">Ігрове поле</param>
        private static int GetCountBlackGroupInAtari(char[][] game)
        {
            int result = 0;
            bool isBlackGroup = game.SelectMany(row => row).Any(ch => ch == 'B');
            while (isBlackGroup)
            {
                (int row, int col) groupPos = (0, 0);
                for (int i = 0; i < game.Length; i++)
                {
                    for(int j = 0; j < game.Length; j++)
                    {
                        if (game[i][j] == 'B')
                        {
                            groupPos = (i, j);
                            break;
                        }
                    }
                    if(groupPos != (0, 0))
                    {
                        break;
                    }
                }

                if (groupIsInAtari(groupPos, game))
                {
                    result++;
                }
                isBlackGroup = game.SelectMany(row => row).Any(ch => ch == 'B');
            }
            return result;
        }

        /// <summary>
        /// Перевірка, чи група в атарі
        /// </summary>
        /// <param name="groupPos">Позиція одного з елементів групи</param>
        /// <param name="game">Ігрове поле</param>
        private static bool groupIsInAtari((int row, int col) groupPos, char[][] game)
        {
            //Перелік позицій усіх елементів групи
            List<(int row, int col)> groupPosList = getGroupPositions(groupPos, game);
            //Перелік позицій усіх даме групи
            List<(int row, int col)> damePosList = new List<(int row, int col)>();
            foreach(var pos in groupPosList)
            {
                damePosList.AddRange(getDamePosListAroundPos(pos, game));
            }
            damePosList = damePosList.Distinct().ToList();
            return damePosList.Count == 1;
        }

        /// <summary>
        /// Отримати усі даме, які знаходяться навколо елемента pos
        /// </summary>
        /// <param name="pos">Позиція, навколо якої шукати даме</param>
        /// <param name="game">Ігрове поле</param>
        private static List<(int row, int col)> getDamePosListAroundPos((int row, int col) pos, char[][] game)
        {
            List<(int row, int col)> result = new List<(int row, int col)>();
            if (pos.row != 0 && game[pos.row - 1][pos.col] == '.')
            {
                result.Add((pos.row - 1, pos.col));
            }
            if (pos.col != 0 && game[pos.row][pos.col - 1] == '.')
            {
                result.Add((pos.row, pos.col - 1));
            }
            if (pos.row != game.Length - 1 && game[pos.row + 1][pos.col] == '.')
            {
                result.Add((pos.row + 1, pos.col));
            }
            if (pos.col != game.Length - 1 && game[pos.row][pos.col + 1] == '.')
            {
                result.Add((pos.row, pos.col + 1));
            }
            return result;
        }

        /// <summary>
        /// Отримати перелік позицій усіх елементів групи
        /// </summary>
        /// <param name="groupPos">Позиція одного з елементів групи</param>
        /// <param name="game">Ігрове поле</param>
        private static List<(int row, int col)> getGroupPositions((int row, int col) groupPos, char[][] game)
        {
            fillGroupInGame(groupPos, game);
            List<(int row, int col)> result = new List<(int row, int col)>();
            for (int i = 0; i < game.Length; i++)
            {
                for (int j = 0; j < game.Length; j++)
                {
                    if (game[i][j] == 'G')
                    {
                        result.Add((i, j));
                        game[i][j] = 'D';
                    }
                }
            }
            return result;
        }

        /// <summary>
        /// Заповнити усі елементи групи іншим значенням (було значення 'B' - стане 'G')
        /// </summary>
        /// <param name="pos">Поточна позиція в групі</param>
        /// <param name="game">Ігрове поле</param>
        private static void fillGroupInGame((int row, int col) pos, char[][] game)
        {
            game[pos.row][pos.col] = 'G';
            if (pos.row != 0 && game[pos.row - 1][pos.col] == 'B')
            {
                fillGroupInGame((pos.row - 1, pos.col), game);
            }
            if (pos.col != 0 && game[pos.row][pos.col - 1] == 'B')
            {
                fillGroupInGame((pos.row, pos.col - 1), game);
            }
            if (pos.row != game.Length - 1 && game[pos.row + 1][pos.col] == 'B')
            {
                fillGroupInGame((pos.row + 1, pos.col), game);
            }
            if (pos.col != game.Length - 1 && game[pos.row][pos.col + 1] == 'B')
            {
                fillGroupInGame((pos.row, pos.col + 1), game);
            }
        }

    }
}