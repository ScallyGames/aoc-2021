string[] lines = File.ReadAllLines("input.txt");

string[] outputDigits = lines.Select(x => x.Split('|')[1]).ToArray();

int numberOfDiscernibleDigits = outputDigits
    .Select(x => x.Split(' '))
    .Select(x => x.Count(digit => digit.Length == 2 || digit.Length == 4 || digit.Length == 3 || digit.Length == 7))
    .Sum();

Console.WriteLine(numberOfDiscernibleDigits);