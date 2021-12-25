int[] positions = File.ReadAllLines("input-crafted.txt")[0].Split(',').Select(x => int.Parse(x)).ToArray();

Console.WriteLine(positions.Max());

int minFuel = int.MaxValue;
int minPosition = -1;

for(int targetPosition = 0; targetPosition <= positions.Max(); targetPosition++)
{
    int amountOfFuel = positions.Select(x => Math.Abs(targetPosition - x)).Sum();
    Console.WriteLine(targetPosition + ";" + amountOfFuel);
    if(amountOfFuel < minFuel)
    {
        minFuel = amountOfFuel;
        minPosition = targetPosition;
    }
}

Console.WriteLine(minFuel);
Console.WriteLine(minPosition);