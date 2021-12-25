string[] lines = File.ReadAllLines("input.txt");

List<int> lanternfish = lines[0].Split(',').Select(x => int.Parse(x)).ToList();

int numberOfDays = 80;

while(numberOfDays > 0)
{
    List<int> newLanternfish = new List<int>();

    for(int i = 0; i < lanternfish.Count; i++)
    {
        lanternfish[i]--;
        if(lanternfish[i] < 0)
        {
            lanternfish[i] = 6;
            newLanternfish.Add(8);
        }
    }
    lanternfish.AddRange(newLanternfish);

    numberOfDays--;
}

Console.WriteLine(lanternfish.Count);