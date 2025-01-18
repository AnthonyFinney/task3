using System.Security.Cryptography;
using System.Text;
using ConsoleTables;

namespace T3;

public class Task3 {
    public void Run(string[] arrayDice) {
        if (arrayDice.Length < 3) {
            throw new ArgumentException("Number of dice have to be 3 or more");
        }

        int userInput, userChoice, pcChoice, resultPc, resultUser;
        List<int[]> dice = MakeDice(arrayDice);
        int pc = GenerateSecureRandomBit(0, 1);
        byte[] key = Generate256BitKey();
        string hmac = ComputeHMAC(pc, key);
        int[] userDic, pcDic;


        while (true) {
            Console.WriteLine("Let's determine who makes the first move.");
            Console.WriteLine("I selected a random value in the range 0..1");
            Console.WriteLine($"(HMAC:{hmac})");

            PrintTheGame(null, DiceGameEnum.Selection);

            userInput = GetUserInput(DiceGameEnum.Selection, 0);
            if (userInput == -1) {
                break;
            }

            Console.WriteLine($"My Selection: {pc} (Key={Convert.ToHexString(key)})");

            if (userInput == pc) {
                userChoice = GetUserDie(dice);
                userDic = dice[userChoice];
                Console.WriteLine($"You choose the [{string.Join(", ", userDic)}] die");

                pcChoice = GetPcDie(dice, userChoice);
                pcDic = dice[pcChoice];
                Console.WriteLine($"I have choose the [{string.Join(", ", pcDic)}] die");
            } else {
                pcChoice = GetPcDie(dice, -1);
                pcDic = dice[pcChoice];
                Console.WriteLine($"I make the first move and choose the [{string.Join(", ", pcDic)}] die");

                dice.RemoveAt(pcChoice);
                List<int[]> diceCopy = new(dice);

                userChoice = GetUserDie(diceCopy);
                userDic = dice[userChoice];
                Console.WriteLine($"You choose the [{string.Join(", ", userDic)}] die");
            }

            Console.WriteLine("It's time for my throw.");
            Console.WriteLine("I selected a random value in the range 0..5 ");

            pc = GenerateSecureRandomBit(0, 5);
            key = Generate256BitKey();
            hmac = ComputeHMAC(pc, key);

            Console.WriteLine($"(HMAC:{hmac})");
            Console.WriteLine("Add your number modulo 6.");

            PrintTheGame(null, DiceGameEnum.Main);
            Console.Write("Your selection: ");

            userInput = GetUserInput(DiceGameEnum.Main, 0);
            if (userInput == -1) {
                break;
            }

            Console.WriteLine($"My number is {pc} (Key:{Convert.ToHexString(key)})");
            resultPc = (pc + userInput) % 6;
            Console.WriteLine($"The result is {pc} + {userInput} = {resultPc} (mod 6).");
            Console.WriteLine($"My throw is {pcDic[resultPc]}");
            resultPc = pcDic[resultPc];

            Console.WriteLine("It's time for your throw.");
            Console.WriteLine("I selected a random value in the range 0..5 ");

            pc = GenerateSecureRandomBit(0, 5);
            key = Generate256BitKey();
            hmac = ComputeHMAC(pc, key);

            Console.WriteLine($"(HMAC:{hmac})");
            Console.WriteLine("Add your number modulo 6.");

            PrintTheGame(null, DiceGameEnum.Main);
            Console.Write("Your selection: ");

            userInput = GetUserInput(DiceGameEnum.Main, 0);
            if (userInput == -1) {
                break;
            }

            Console.WriteLine($"My number is {pc} (Key:{Convert.ToHexString(key)})");
            resultUser = (pc + userInput) % 6;
            Console.WriteLine($"The result is {pc} + {userInput} = {resultUser} (mod 6).");
            Console.WriteLine($"Your throw is {userDic[resultUser]}");
            resultUser = userDic[resultUser];

            if (resultUser > resultPc) {
                Console.WriteLine($"You win ({resultUser} > {resultPc})");
            } else if (resultUser < resultPc) {
                Console.WriteLine($"You loss ({resultUser} < {resultPc})");
            } else {
                Console.WriteLine($"It draw ({resultUser} = {resultPc})");
            }
        }
    }

    private int GetUserInput(DiceGameEnum diceGameEnum, int diceCount) {
        string userInput;
        int checkInt = 0;

        if (diceGameEnum == DiceGameEnum.Selection) {
            checkInt = 1;
        } else if (diceGameEnum == DiceGameEnum.Main) {
            checkInt = 5;
        } else if (diceGameEnum == DiceGameEnum.Dice) {
            checkInt = diceCount - 1;
        }

        while (true) {
            Console.Write("Your selection: ");
            userInput = Console.ReadLine()!;
            if (userInput == "?") {
                PrintTable();
            } else if (userInput == "X") {
                return -1;
            } else if (int.TryParse(userInput, out int input) && input >= 0 && input <= checkInt) {
                return input;
            }
            Console.WriteLine("Try a new number");
        }
    }

    private void PrintTable() {
        var table = new ConsoleTable(" User dice v ", " 2,2,4,4,9,9 ", " 1,1,6,6,8,8 ", " 3,3,5,5,7,7 ");
        table.AddRow(" 2,2,4,4,9,9 ", " - (0.3333)  ", " 0.5556 ", " 0.4444 ")
            .AddRow(" 1,1,6,6,8,8 ", " 0.4444 ", " - (0.3333) ", " 0.5556 ")
            .AddRow(" 3,3,5,5,7,7 ", " 0.5556 ", " 0.4444 ", " - (0.3333) ");

        table.Write();
        Console.WriteLine();
    }

    private int GetUserDie(List<int[]> dice) {
        Console.WriteLine("Select Die: ");
        PrintTheGame(dice);

        return GetUserInput(DiceGameEnum.Dice, dice.Count);
    }

    private int GetPcDie(List<int[]> dice, int userChoice) {
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

    private void PrintTheGame(List<int[]> dice = null, DiceGameEnum diceGameEnum = DiceGameEnum.Dice) {
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

    private List<int[]> MakeDice(string[] array) {
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

    private int GenerateSecureRandomBit(int min, int max) {
        byte[] randomByte = new byte[1];
        RandomNumberGenerator.Fill(randomByte);
        int range = max - min + 1;
        return (randomByte[0] % range) + min;
    }

    private byte[] Generate256BitKey() {
        byte[] key = new byte[32];
        RandomNumberGenerator.Fill(key);
        return key;
    }

    private string ComputeHMAC(int randomByte, byte[] key) {
        HMACSHA256 hmac = new HMACSHA256(key);
        byte[] bitBytes = Encoding.UTF8.GetBytes(randomByte.ToString());
        byte[] hash = hmac.ComputeHash(bitBytes);
        return Convert.ToHexString(hash);
    }
}

enum DiceGameEnum { Selection, Dice, Main };
