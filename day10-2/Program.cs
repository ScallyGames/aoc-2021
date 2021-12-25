string[] lines = File.ReadAllLines("input.txt");


Dictionary<char, char> matchingBraces = new Dictionary<char, char>();
matchingBraces.Add('(', ')');
matchingBraces.Add('[', ']');
matchingBraces.Add('{', '}');
matchingBraces.Add('<', '>');

var correctLines = lines.Select(x => x.Trim()).Where(x => !IsCorruptedLine(x, out int position));

long middleScore = correctLines.Select(x => {
    GetCompletedLine(x, out long score);
    return score;
})
.OrderBy(x => x)
.Skip(correctLines.Count() / 2)
.First();

Console.WriteLine(middleScore);


int GetScore(char c) => c switch { 
    ')' => 1,
    ']' => 2,
    '}' => 3,
    '>' => 4,
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


string GetCompletedLine(string line, out long score)
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
                if(!braceStack.TryPop(out char prev)) throw new Exception("Nothing to pop");
                break;
            }
        }
    }

    score = 0;

    while(braceStack.Any())
    {
        char openingBrace = braceStack.Pop();
        char neededClosingBrace = matchingBraces[openingBrace];

        line += neededClosingBrace;
        score *= 5;
        score += GetScore(neededClosingBrace);
    }

    Console.WriteLine(score);

    return line;
}