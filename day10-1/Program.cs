string[] lines = File.ReadAllLines("input.txt");


Dictionary<char, char> matchingBraces = new Dictionary<char, char>();
matchingBraces.Add('(', ')');
matchingBraces.Add('[', ']');
matchingBraces.Add('{', '}');
matchingBraces.Add('<', '>');

int totalSyntaxErrorScore = lines.Select(x => x.Trim()).Select(x => {
    if(IsCorruptedLine(x, out int position))
    {
        return GetScore(x[position]);
    }
    else
    {
        return 0;
    }
})
.Sum();

Console.WriteLine(totalSyntaxErrorScore);

int GetScore(char c) => c switch { 
    ')' => 3,
    ']' => 57,
    '}' => 1197,
    '>' => 25137,
    _ => 0,
};

bool IsCorruptedLine(string line, out int position)
{
    Stack<char> braceStack = new Stack<char>();

    for(int i = 0; i < line.Length; i++)
    {
        switch(line[i])
        {
            case '(':
            case '[':
            case '{':
            case '<':
                braceStack.Push(line[i]);
                break;
            case '>':
            case '}':
            case ']':
            case ')':
            {
                if(!braceStack.TryPop(out char prev) || matchingBraces[prev] != line[i])
                {
                    Console.WriteLine("Corruption in line " + line + " at index " + i + ". expected " + matchingBraces[prev] + ", but found " + line[i] + " instead.");
                    position = i;
                    return true;
                }
                break;
            }
        }
    }

    position = (char)0;
    return false;
}