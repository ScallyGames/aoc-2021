string[] lines = File.ReadAllLines("input.txt");

var lineSplits = lines.Select(x => x.Split('|')).ToArray();


int sum = 0;

for(int i = 0; i < lineSplits.Length; i++)
{
    var digitCombinations = lineSplits[i][0].Split(' ', StringSplitOptions.RemoveEmptyEntries).ToArray();
    var digits1 = digitCombinations.First(x => x.Length == 2);
    var digits7 = digitCombinations.First(x => x.Length == 3);
    var digits8 = digitCombinations.First(x => x.Length == 7);
    var digits4 = digitCombinations.First(x => x.Length == 4);
    var digits6 = digitCombinations.First(x => x.Length == 6 && !digits1.All(onesDigit => x.Contains(onesDigit)));
    
    var fiveDigitCandiadates = digitCombinations.Where(x => x.Length == 5);


    var digits0 = digitCombinations.First(x => 
        {
            if(x.Length != 6) return false;
            if(x == digits6) return false;
            
            var centerDigit = digits8.First(digit => !x.Contains(digit));
            return fiveDigitCandiadates.All(f => f.Contains(centerDigit));
        }
    );
    var digits9 = digitCombinations.First(x => x.Length == 6 && x != digits6 && x != digits0);

    var topDigit = digits7.Where(x => !digits1.Contains(x)).First();
    var topRightDigit = digits1.Where(x => !digits6.Contains(x)).First();
    var bottomRightDigit = digits1.Where(x => digits6.Contains(x)).First();

    var digits5 = digitCombinations.First(x => x.Length == 5 && !x.Contains(topRightDigit));
    var digits2 = digitCombinations.First(x => x.Length == 5 && !x.Contains(bottomRightDigit));
    var digits3 = digitCombinations.First(x => x.Length == 5 && x != digits5 && x != digits2);

    int number = 0;

    var outputDigits = lineSplits[i][1].Split(' ', StringSplitOptions.RemoveEmptyEntries).ToArray();
    for(int outputNumberIndex = 0; outputNumberIndex < 4; outputNumberIndex++)
    {
        int digit = 0;

        var digitCombination = outputDigits[outputNumberIndex];
        if(AreEqual(digitCombination, digits0))
        {
            digit = 0;
        }
        else if(AreEqual(digitCombination, digits1))
        {
            digit = 1;
        }
        else if(AreEqual(digitCombination, digits2))
        {
            digit = 2;
        }
        else if(AreEqual(digitCombination, digits3))
        {
            digit = 3;
        }
        else if(AreEqual(digitCombination, digits4))
        {
            digit = 4;
        }
        else if(AreEqual(digitCombination, digits5))
        {
            digit = 5;
        }
        else if(AreEqual(digitCombination, digits6))
        {
            digit = 6;
        }
        else if(AreEqual(digitCombination, digits7))
        {
            digit = 7;
        }
        else if(AreEqual(digitCombination, digits8))
        {
            digit = 8;
        }
        else if(AreEqual(digitCombination, digits9))
        {
            digit = 9;
        }
        else
        {
            Console.WriteLine("Line " + i + ": " + digitCombination + " not found");
        }

        number += digit * (int)Math.Pow(10, 3 - outputNumberIndex);
    }

    Console.WriteLine(number);

    sum += number;
}

Console.WriteLine();
Console.WriteLine(sum);

bool AreEqual(string combination1, string combination2) => combination1.All(c => combination2.Contains(c)) && combination2.All(c => combination1.Contains(c));