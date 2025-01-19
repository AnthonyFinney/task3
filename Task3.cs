namespace T3;

public class Task3 {
    public void Run(string[] arrayDice) {
        if (arrayDice.Length < 3) {
            throw new ArgumentException("Number of dice have to be 3 or more");
        }

        while (true) {
            int userInput, userChoice, pcChoice, resultPc, resultUser;
            List<int[]> dice = DiceManager.MakeDice(arrayDice);
            int pc = CryptoHelper.GenerateSecureRandomBit(0, 1);
            byte[] key = CryptoHelper.Generate256BitKey();
            string hmac = CryptoHelper.ComputeHMAC(pc, key);
            int[] userDic, pcDic;

            Console.WriteLine("Let's determine who makes the first move.");
            Console.WriteLine("I selected a random value in the range 0..1");
            Console.WriteLine($"(HMAC:{hmac})");

            GamePrinter.PrintTheGame(null, DiceGameEnum.Selection);

            userInput = UserInputHandler.GetUserInput(DiceGameEnum.Selection, dice);
            if (userInput == -1) {
                break;
            }

            Console.WriteLine($"My Selection: {pc} (Key={Convert.ToHexString(key)})");

            if (userInput == pc) {
                userChoice = DiceManager.GetUserDie(dice);
                userDic = dice[userChoice];
                Console.WriteLine($"You choose the [{string.Join(", ", userDic)}] die");

                pcChoice = DiceManager.GetPcDie(dice, userChoice);
                pcDic = dice[pcChoice];
                Console.WriteLine($"I have choose the [{string.Join(", ", pcDic)}] die");
            } else {
                pcChoice = DiceManager.GetPcDie(dice, -1);
                pcDic = dice[pcChoice];
                Console.WriteLine($"I make the first move and choose the [{string.Join(", ", pcDic)}] die");

                dice.RemoveAt(pcChoice);
                List<int[]> diceCopy = new(dice);

                userChoice = DiceManager.GetUserDie(diceCopy);
                userDic = dice[userChoice];
                Console.WriteLine($"You choose the [{string.Join(", ", userDic)}] die");
            }

            Console.WriteLine("It's time for my throw.");
            Console.WriteLine("I selected a random value in the range 0..5 ");

            pc = CryptoHelper.GenerateSecureRandomBit(0, 5);
            key = CryptoHelper.Generate256BitKey();
            hmac = CryptoHelper.ComputeHMAC(pc, key);

            Console.WriteLine($"(HMAC:{hmac})");
            Console.WriteLine("Add your number modulo 6.");

            GamePrinter.PrintTheGame(null, DiceGameEnum.Main);
            Console.Write("Your selection: ");

            userInput = UserInputHandler.GetUserInput(DiceGameEnum.Main, dice);
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

            pc = CryptoHelper.GenerateSecureRandomBit(0, 5);
            key = CryptoHelper.Generate256BitKey();
            hmac = CryptoHelper.ComputeHMAC(pc, key);

            Console.WriteLine($"(HMAC:{hmac})");
            Console.WriteLine("Add your number modulo 6.");

            GamePrinter.PrintTheGame(null, DiceGameEnum.Main);
            Console.Write("Your selection: ");

            userInput = UserInputHandler.GetUserInput(DiceGameEnum.Main, dice);
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
}

public enum DiceGameEnum { Selection, Dice, Main };
