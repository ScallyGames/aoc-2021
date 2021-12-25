string[] lines = File.ReadAllLines("input.txt");

var lineSplits = lines.Select(x => x.Split('|')).ToArray();


bool[][] truthTable = new bool[][] {
    //          0      1      2      3      4      5      6      7      8      9
    new []{  true, false,  true,  true, false,  true,  true,  true,  true,  true }, // a
    new []{  true, false, false, false,  true,  true,  true, false,  true,  true }, // b
    new []{  true,  true,  true,  true,  true, false, false,  true,  true,  true }, // c
    new []{ false, false,  true,  true,  true,  true,  true, false,  true,  true }, // d
    new []{  true, false,  true, false, false, false,  true, false,  true, false }, // e
    new []{  true,  true, false,  true,  true,  true,  true,  true,  true,  true }, // f
    new []{  true, false,  true,  true, false,  true,  true, false,  true,  true }, // g
};


int sum = 0;


for(int i = 0; i < lineSplits.Length; i++)
{
    string[] referenceDigits = lineSplits[i][1].Split(' ', StringSplitOptions.RemoveEmptyEntries);

    Queue<char> openDigits = new Queue<char>(new []{ 'a', 'b', 'c', 'd', 'e', 'f', 'g' });

    while(openDigits.Count > 0)
    {
        bool foundUniqueSolution = false;
        char digitToTest = openDigits.Dequeue();

        var candidates = truthTable.Where(x => )

        if(!foundUniqueSolution)
        {
            openDigits.Enqueue(digitToTest);
        }
    }


    int number = 0;

    var outputDigits = lineSplits[i][1].Split(' ', StringSplitOptions.RemoveEmptyEntries).ToArray();
    for(int outputNumberIndex = 0; outputNumberIndex < 4; outputNumberIndex++)
    {
        int digit = 0;

        
        number += digit * (int)Math.Pow(10, 3 - outputNumberIndex);
    }

    Console.WriteLine(number);

    sum += number;
}

Console.WriteLine();
Console.WriteLine(sum);

bool AreEqual(string combination1, string combination2) => combination1.All(c => combination2.Contains(c)) && combination2.All(c => combination1.Contains(c));