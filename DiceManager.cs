using System.Security.Cryptography;

namespace T3;

public static class DiceManager {
    public static int GetUserDie(List<int[]> dice) {
        Console.WriteLine("Select Die: ");
        GamePrinter.PrintTheGame(dice);

        return UserInputHandler.GetUserInput(DiceGameEnum.Dice, dice);
    }

    public static int GetPcDie(List<int[]> dice, int userChoice) {
        RandomNumberGenerator rng = RandomNumberGenerator.Create();
        int pcChoice;
        do {
            byte[] randomByte = new byte[1];
            rng.GetBytes(randomByte);
            pcChoice = randomByte[0] % dice.Count;
        } while (pcChoice == userChoice);

        Console.WriteLine($"Pc Die: {pcChoice}");
        return pcChoice;
    }

    public static List<int[]> MakeDice(string[] array) {
        List<int[]> dice = new();
        foreach (var arr in array) {
            var value = arr.Split(",").Select(int.Parse).ToArray();
            if (value.Length != 6) {
                throw new ArgumentException("Number in the dice is not 6");
            }
            dice.Add(value);
        }

        return dice;
    }
}