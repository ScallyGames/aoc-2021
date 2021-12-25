using System.Numerics;

string[] lines = File.ReadAllLines("input.txt");

var lanternfish = lines[0].Split(',').Select(x => int.Parse(x)).GroupBy(x => x).ToDictionary(x => x.Key, x => new BigInteger(x.Count()));

for(int i = 0; i <= 8; i++)
{
    if(!lanternfish.ContainsKey(i))
    {
        lanternfish.Add(i, new BigInteger(0));
    }
}

int numberOfDays = 256;

while(numberOfDays > 0)
{
    Console.WriteLine("New day");
    for(int i = 0; i <= 8; i++)
    {
        Console.WriteLine(i + " " + lanternfish[i]);
    }
    Console.WriteLine("");
    BigInteger fishesGivingBirth = lanternfish[0];
    
    for(int i = 0; i < 8; i++)
    {
        lanternfish[i] = lanternfish[i+1];
    }

    lanternfish[6] += fishesGivingBirth;
    lanternfish[8] = fishesGivingBirth;

    numberOfDays--;
}

Console.WriteLine(lanternfish.Aggregate(new BigInteger(0), (a, b) => a + b.Value));