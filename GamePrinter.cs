using ConsoleTables;

namespace T3;

public static class GamePrinter {
    public static void PrintTheGame(List<int[]> dice = null, DiceGameEnum diceGameEnum = DiceGameEnum.Dice) {
        if (diceGameEnum == DiceGameEnum.Selection) {
            int range = 2;
            Console.WriteLine("Try to guess my selection.");
            for (int i = 0; i < range; i++) {
                Console.WriteLine($"{i} : {i}");
            }
        } else if (diceGameEnum == DiceGameEnum.Main) {
            int range = 6;
            Console.WriteLine("I selected a random value in the range 0..5");
            for (int i = 0; i < range; i++) {
                Console.WriteLine($"{i} : {i}");
            }
        } else if (diceGameEnum == DiceGameEnum.Dice) {
            Console.WriteLine("Choose your dice:");
            for (int i = 0; i < dice.Count; i++) {
                Console.WriteLine($"{i} : {string.Join(", ", dice[i])}");
            }
        }
        Console.WriteLine("X - exit");
        Console.WriteLine("? - help");
    }
    public static void PrintTable(List<int[]> dice) {
        var header = new List<string> { "Dice" };
        for (int i = 0; i < dice.Count; i++) {
            header.Add($"Dice {i + 1}");
        }

        var table = new ConsoleTable(header.ToArray());
        for (int i = 0; i < dice.Count; i++) {
            var row = new List<string> { string.Join(", ", dice[i]) };
            for (int j = 0; j < dice.Count; j++) {
                if (i == j) {
                    row.Add("- (1.000)");
                } else {
                    double prob = GetWinProbability(dice[i], dice[j]);
                    row.Add($"{prob}");
                }
            }
            table.AddRow(row.ToArray());
        }

        table.Write();
        Console.WriteLine();
    }

    private static int CountWins(int[] a, int[] b) {
        return a.Select(x => ~Array.BinarySearch(b, x, Comparer<int>.Create((x, y) => Math.Sign(x - y + 0.5)))).Sum();
    }

    private static double GetWinProbability(int[] die1, int[] die2) {
        int wins = CountWins(die1, die2);
        int totalComparison = die1.Length * die2.Length;
        return (double)wins / totalComparison;
    }
}