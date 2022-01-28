using System.Diagnostics;

string[] instructions = File.ReadAllLines("input.txt");

Dictionary<(int, int), string> correctDigits = new Dictionary<(int, int), string>();
HashSet<(int layer, int z)> checkedStates = new HashSet<(int layer, int z)>();

var parseStack = new []
{
    Parse0,
    Parse1,
    Parse2,
    Parse3,
    Parse4,
    Parse5,
    Parse6,
    Parse7,
    Parse8,
    Parse9,
    Parse10,
    Parse11,
    Parse12,
    Parse13,
};


PriorityQueue<(string inputDigits, int z), long> openList = new PriorityQueue<(string inputDigits, int z), long>(Comparer<long>.Create((a, b) => b > a ? 1 : -1));

openList.Enqueue(("", 0), 0);

bool hasFoundSolution = false;

while(openList.Count > 0 && !hasFoundSolution)
{
    var currentElement = openList.Dequeue();

    Console.WriteLine(currentElement.inputDigits);

    for(int digit = 9; digit >= 1; digit--)
    {
        string nextInputDigits = currentElement.inputDigits + digit.ToString();
        int newZ = parseStack[currentElement.inputDigits.Length](digit, currentElement.z);


        if(nextInputDigits.Length == parseStack.Length)
        {
            if(newZ == 0)
            {
                Debug.Assert(IsValid(nextInputDigits));

                Console.WriteLine(nextInputDigits);
                hasFoundSolution = true;
            }
        }
        else
        {
            if(!checkedStates.Contains((nextInputDigits.Length, newZ)))
            {
                openList.Enqueue((nextInputDigits, newZ), long.Parse(nextInputDigits));
                checkedStates.Add((nextInputDigits.Length, newZ));
                
                // Console.WriteLine($"Adding {nextInputDigits:15} with a z of {newZ}");
            }
            else
            {
                Console.WriteLine($"Already had something instead of {nextInputDigits:15} (Length: {nextInputDigits.Length})");
            }
        }
    }
}

bool IsValid(string d)
{
    int z = 0;
    
    for(int i = 0; i < parseStack.Length; i++)
    {
        z = parseStack[i](int.Parse(d[i] + ""), z);
    }

    return z == 0;
}

static int Parse0(int d0, int z)
{
    return (d0 + 2) * 26;
}

static int Parse1(int d1, int z)
{
    return z + (d1 + 4);
}

static int Parse2(int d2, int z)
{
    int x = (z % 26) + 14 == d2 ? 0 : 1;
    return z * ((25 * x) + 1) + ((d2 + 8) * x);
}

static int Parse3(int d3, int z)
{
    int x = (z % 26) + 11 == d3 ? 0 : 1;
    return z * ((25 * x) + 1) + ((d3 + 7) * x);
}

static int Parse4(int d4, int z)
{
    int x = (z % 26) + 14 == d4 ? 0 : 1;
    return z * ((25 * x) + 1) + ((d4 + 12) * x);
}

static int Parse5(int d5, int z)
{
    int x = (z % 26) - 14 == d5 ? 0 : 1;
    z = z / 26;
    return z + ((25 * x) + 1) + ((d5 + 7) * x);
}

static int Parse6(int d6, int z)
{
    int x = (z % 26) == d6 ? 0 : 1;
    z = z / 26;
    return z * ((25 * x) + 1) + ((d6 + 10) * x);
}

static int Parse7(int d7, int z)
{
    int x = (z % 26) + 10 == d7 ? 0 : 1;
    return z * ((25 * x) + 1) + ((d7 + 14) * x);
}

static int Parse8(int d8, int z)
{
    int x = (z % 26) - 10 == d8 ? 0 : 1;
    z = z / 26;
    return z * ((25 * x) + 1) + ((d8 + 2) * x);
}

static int Parse9(int d9, int z)
{
    int x = (z % 26) + 13 == d9 ? 0 : 1;
    return z * ((25 * x) + 1) + ((d9 + 6) * x);
}

static int Parse10(int d10, int z)
{
    int x = (z % 26) - 12 == d10 ? 0 : 1;
    z = z / 26;
    return z * ((25 * x) + 1) + ((d10 + 8) * x);
}

static int Parse11(int d11, int z)
{
    int x = (z % 26) - 3 == d11 ? 0 : 1;
    z = z / 26;
    return z * ((25 * x) + 1) + ((d11 + 11) * x);
}

static int Parse12(int d12, int z)
{
    int x = (z % 26) - 11 == d12 ? 0 : 1;
    z = z / 26;
    return z * ((25 * x) + 1) + ((d12 + 5) * x);
}

static int Parse13(int d13, int z)
{
    int x = (z % 26) - 2 == d13 ? 0 : 1;
    z = z / 26;
    return z * ((25 * x) + 1) + ((d13 + 11) * x);
}
