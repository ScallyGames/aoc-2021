using System.Collections;
using System.Text;

string input = File.ReadAllLines("input.txt")[0];

byte[] data = Convert.FromHexString(input);

List<(ConsoleColor color, string digit)> parsed = new List<(ConsoleColor color, string digit)>();

uint sumOfVersions = 0;

ReadPacket(data, 0, out int length);


Console.WriteLine(sumOfVersions);

void ReadPacket(byte[] data, int startIndex, out int length, string indent = "")
{
    PrintParsed();
    Console.WriteLine(indent + "{");
    Console.WriteLine(indent + "    " + "New Packet");
    Console.WriteLine(indent + "    " + "Starting at: " + startIndex);
    int currentIndex = startIndex;

    AppendDigits(data, currentIndex, 3, ConsoleColor.Red);
    uint version = ReadIntFromBits(currentIndex, 3, ref data, ref currentIndex);
    Console.WriteLine(indent + "    " + "Version: " + version);
    sumOfVersions += version;
    
    AppendDigits(data, currentIndex, 3, ConsoleColor.Green);
    uint type = ReadIntFromBits(currentIndex, 3, ref data, ref currentIndex);
    Console.WriteLine(indent + "    " + "Type: " + type);

    switch (type)
    {
        case 4:
            {
                while(GetBitFromByteArray(data, currentIndex))
                {
                    AppendDigits(data, currentIndex, 5, ConsoleColor.Yellow);
                    currentIndex += 5;
                }
                AppendDigits(data, currentIndex, 5, ConsoleColor.Yellow);
                currentIndex += 5;
                break;
            }
        default:
            {
                bool isNumberOfSubpackages = GetBitFromByteArray(data, currentIndex);
                AppendDigits(data, currentIndex, 1, ConsoleColor.Cyan);
                currentIndex++;
                if(!isNumberOfSubpackages)
                {
                    AppendDigits(data, currentIndex, 15, ConsoleColor.Yellow);
                    uint numberOfBits = ReadIntFromBits(currentIndex, 15, ref data, ref currentIndex);
                    Console.WriteLine(indent + "    " + $"Contains {numberOfBits} bits of packets");
                    int bitsConsumed = 0;
                    while(bitsConsumed < numberOfBits)
                    {
                        ReadPacket(data, currentIndex, out int innerPacketLength, indent + "    ");
                        bitsConsumed += innerPacketLength;
                        currentIndex += innerPacketLength;
                    }
                }
                else
                {
                    AppendDigits(data, currentIndex, 11, ConsoleColor.DarkYellow);
                    uint numberOfPackets = ReadIntFromBits(currentIndex, 11, ref data, ref currentIndex);
                    Console.WriteLine(indent + "    " + $"Contains {numberOfPackets} packets");
                    for(uint i = 0; i < numberOfPackets; i++)
                    {
                        ReadPacket(data, currentIndex, out int innerPacketLength, indent + "    ");
                        currentIndex += innerPacketLength;
                    }
                }

                break;
            }
    }

    length = currentIndex - startIndex;
    Console.WriteLine(indent + "    " + "Length: " + length);
    
    Console.WriteLine(indent + "}");
}

uint ReadIntFromBits(int startIndex, int bitCount, ref byte[] data, ref int currentIndex)
{
    uint parsedData = 0;

    for(int i = 0; i < bitCount; i++)
    {
        if(GetBitFromByteArray(data, startIndex + i))
        {
            parsedData |= 1u << (bitCount - 1 - i);
        }
    }
    currentIndex += bitCount;
    return parsedData;
}

string GetBitStringFromByteArray(byte[] data)
{
    StringBuilder sb = new StringBuilder();
    for(int i = 0; i < data.Length * 8; i++)
    {
        sb.Append(((data[i / 8] & (1 << (7 - i % 8))) > 0) ? "1" : "0");
    }

    return  sb.ToString();
}

bool GetBitFromByteArray(byte[] data, int bitIndex)
{
    return (data[bitIndex / 8] & (1 << (7 - bitIndex % 8))) > 0;
}

void AppendDigits(byte[] data, int startIndex, int bitCount, ConsoleColor color)
{
    for(int i = startIndex; i < startIndex + bitCount; i++)
    {
        parsed.Add((color, ((data[i / 8] & (1 << (7 - i % 8))) > 0) ? "1" : "0"));
    }
}

void PrintParsed()
{
    return;
    foreach(var parsedDitit in parsed)
    {
        Console.ForegroundColor = parsedDitit.color;
        Console.Write(parsedDitit.digit);
    }
    Console.ResetColor();
    for(int i = parsed.Count; i < data.Length * 8; i++)
    {
        Console.Write(((data[i / 8] & (1 << (7 - i % 8))) > 0) ? "1" : "0");
    }
    Console.WriteLine();
}