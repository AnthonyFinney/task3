namespace T3;

public static class UserInputHandler {
    public static int GetUserInput(DiceGameEnum diceGameEnum, List<int[]> dice) {
        string userInput;
        int checkInt = 0;

        if (diceGameEnum == DiceGameEnum.Selection) {
            checkInt = 1;
        } else if (diceGameEnum == DiceGameEnum.Main) {
            checkInt = 5;
        } else if (diceGameEnum == DiceGameEnum.Dice && dice != null) {
            checkInt = dice.Count - 1;
        }

        while (true) {
            Console.Write("Your selection: ");
            userInput = Console.ReadLine()?.Trim()!;
            if (userInput == "?" && dice != null) {
                GamePrinter.PrintTable(dice);
            } else if (userInput == "?" && dice == null) {
                Console.WriteLine("No dice data is available.");
            } else if (userInput == "X") {
                return -1;
            } else if (int.TryParse(userInput, out int input) && input >= 0 && input <= checkInt) {
                return input;
            }
            Console.WriteLine("Try a new number");
        }
    }
}