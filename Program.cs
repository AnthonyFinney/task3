using T3;

try {
    Task3 task3 = new();
    task3.Run(args);
} catch (System.Exception ex) {
    System.Console.WriteLine("Error " + ex.Message);
}