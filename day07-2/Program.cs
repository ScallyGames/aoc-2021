int[] positions = File.ReadAllLines("input.txt")[0].Split(',').Select(x => int.Parse(x)).ToArray();

Console.WriteLine(positions.Max());

int minFuel = int.MaxValue;

for(int targetPosition = 0; targetPosition <= positions.Max(); targetPosition++)
{
    int amountOfFuel = positions.Select(x => costFunction(Math.Abs(targetPosition - x))).Sum();
    Console.WriteLine(targetPosition + ";" + amountOfFuel);
    if(amountOfFuel < minFuel)
    {
        minFuel = amountOfFuel;
    }
}

Console.WriteLine(minFuel);

int costFunction(int distance)
{
    return distance * (distance + 1) / 2;
}