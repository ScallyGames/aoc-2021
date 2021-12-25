using System.Collections;
using System.Numerics;
using System.Text;

string input = File.ReadAllLines("input.txt")[0];

byte[] data = Convert.FromHexString(input);

List<(ConsoleColor color, string digit)> parsed = new List<(ConsoleColor color, string digit)>();

StringBuilder parsedInstructions = new StringBuilder();

ReadPacket(data, 0, out int length);

Console.WriteLine(parsedInstructions);


BigInteger ReadPacket(byte[] data, int startIndex, out int length, string indent = "")
{
    BigInteger resultingValue = 0;

    PrintParsed();
    Console.WriteLine(indent + "{");
    Console.WriteLine(indent + "    " + "New Packet");
    Console.WriteLine(indent + "    " + "Starting at: " + startIndex);
    int currentIndex = startIndex;

    AppendDigits(data, currentIndex, 3, ConsoleColor.Red);
    uint version = ReadIntFromBits(currentIndex, 3, ref data, ref currentIndex);
    Console.WriteLine(indent + "    " + "Version: " + version);

    AppendDigits(data, currentIndex, 3, ConsoleColor.Green);
    uint type = ReadIntFromBits(currentIndex, 3, ref data, ref currentIndex);
    Console.WriteLine(indent + "    " + "Type: " + type);

    switch (type)
    {
        case 4:
            {
                ulong value = 0;
                bool shouldStop = false;
                do
                {
                    shouldStop = !GetBitFromByteArray(data, currentIndex);
                    AppendDigits(data, currentIndex, 5, ConsoleColor.Yellow);
                    currentIndex++;
                    for (int i = 0; i < 4; i++)
                    {
                        value = value << 1;
                        value += GetBitFromByteArray(data, currentIndex) ? 1u : 0u;
                        currentIndex++;
                    }
                }
                while (!shouldStop);

                resultingValue = value;
                parsedInstructions.Append(resultingValue + "UL");
                break;
            }
        default:
            {
                Queue<BigInteger> arguments = new Queue<BigInteger>();


                #region OutputOnly
                switch (type)
                {
                    case 0:
                        {
                            parsedInstructions.Append("(");
                            break;
                        }
                    case 1:
                        {
                            parsedInstructions.Append("(");
                            break;
                        }
                    case 2:
                        {
                            parsedInstructions.Append("new [] {");
                            break;
                        }
                    case 3:
                        {
                            parsedInstructions.Append("new [] {");
                            break;
                        }
                    case 5:
                        {
                            parsedInstructions.Append($"Convert.ToUInt64(");
                            break;
                        }
                    case 6:
                        {
                            parsedInstructions.Append($"Convert.ToUInt64(");
                            break;
                        }
                    case 7:
                        {
                            parsedInstructions.Append($"Convert.ToUInt64(");
                            break;
                        }
                }
                #endregion


                bool isNumberOfSubpackages = GetBitFromByteArray(data, currentIndex);
                AppendDigits(data, currentIndex, 1, ConsoleColor.Cyan);
                currentIndex++;
                if (!isNumberOfSubpackages)
                {
                    AppendDigits(data, currentIndex, 15, ConsoleColor.Yellow);
                    uint numberOfBits = ReadIntFromBits(currentIndex, 15, ref data, ref currentIndex);
                    Console.WriteLine(indent + "    " + $"Contains {numberOfBits} bits of packets");
                    int bitsConsumed = 0;
                    while (bitsConsumed < numberOfBits)
                    {
                        var value = ReadPacket(data, currentIndex, out int innerPacketLength, indent + "    ");
                        arguments.Enqueue(value);
                        bitsConsumed += innerPacketLength;
                        currentIndex += innerPacketLength;

                        #region OutputOnly
                        switch (type)
                        {
                            case 0:
                                {
                                    parsedInstructions.Append(" + ");
                                    break;
                                }
                            case 1:
                                {
                                    parsedInstructions.Append(" * ");
                                    break;
                                }
                            case 2:
                                {
                                    parsedInstructions.Append(", ");
                                    break;
                                }
                            case 3:
                                {
                                    parsedInstructions.Append(", ");
                                    break;
                                }
                            case 5:
                                {
                                    parsedInstructions.Append(" > ");
                                    break;
                                }
                            case 6:
                                {
                                    parsedInstructions.Append(" < ");
                                    break;
                                }
                            case 7:
                                {
                                    parsedInstructions.Append(" == ");
                                    break;
                                }
                        }
                        #endregion
                    }

                    #region OutputOnly
                    switch (type)
                    {
                        case 0:
                            {
                                parsedInstructions.Remove(parsedInstructions.Length - " + ".Length, " + ".Length);
                                break;
                            }
                        case 1:
                            {
                                parsedInstructions.Remove(parsedInstructions.Length - " * ".Length, " * ".Length);
                                break;
                            }
                        case 2:
                            {
                                parsedInstructions.Remove(parsedInstructions.Length - ", ".Length, ", ".Length);
                                break;
                            }
                        case 3:
                            {
                                parsedInstructions.Remove(parsedInstructions.Length - ", ".Length, ", ".Length);
                                break;
                            }
                        case 5:
                            {
                                parsedInstructions.Remove(parsedInstructions.Length - " > ".Length, " > ".Length);
                                break;
                            }
                        case 6:
                            {
                                parsedInstructions.Remove(parsedInstructions.Length - " < ".Length, " < ".Length);
                                break;
                            }
                        case 7:
                            {
                                parsedInstructions.Remove(parsedInstructions.Length - " == ".Length, " == ".Length);
                                break;
                            }
                    }
                    #endregion
                }
                else
                {
                    AppendDigits(data, currentIndex, 11, ConsoleColor.DarkYellow);
                    uint numberOfPackets = ReadIntFromBits(currentIndex, 11, ref data, ref currentIndex);
                    Console.WriteLine(indent + "    " + $"Contains {numberOfPackets} packets");
                    for (uint i = 0; i < numberOfPackets; i++)
                    {
                        var value = ReadPacket(data, currentIndex, out int innerPacketLength, indent + "    ");
                        arguments.Enqueue(value);
                        currentIndex += innerPacketLength;

                        #region OutputOnly
                        switch (type)
                        {
                            case 0:
                                {
                                    parsedInstructions.Append(" + ");
                                    break;
                                }
                            case 1:
                                {
                                    parsedInstructions.Append(" * ");
                                    break;
                                }
                            case 2:
                                {
                                    parsedInstructions.Append(", ");
                                    break;
                                }
                            case 3:
                                {
                                    parsedInstructions.Append(", ");
                                    break;
                                }
                            case 5:
                                {
                                    parsedInstructions.Append(" > ");
                                    break;
                                }
                            case 6:
                                {
                                    parsedInstructions.Append(" < ");
                                    break;
                                }
                            case 7:
                                {
                                    parsedInstructions.Append(" == ");
                                    break;
                                }
                        }
                        #endregion
                    }

                    #region OutputOnly
                    switch (type)
                    {
                        case 0:
                            {
                                parsedInstructions.Remove(parsedInstructions.Length - " + ".Length, " + ".Length);
                                break;
                            }
                        case 1:
                            {
                                parsedInstructions.Remove(parsedInstructions.Length - " * ".Length, " * ".Length);
                                break;
                            }
                        case 2:
                            {
                                parsedInstructions.Remove(parsedInstructions.Length - ", ".Length, ", ".Length);
                                break;
                            }
                        case 3:
                            {
                                parsedInstructions.Remove(parsedInstructions.Length - ", ".Length, ", ".Length);
                                break;
                            }
                        case 5:
                            {
                                parsedInstructions.Remove(parsedInstructions.Length - " > ".Length, " > ".Length);
                                break;
                            }
                        case 6:
                            {
                                parsedInstructions.Remove(parsedInstructions.Length - " < ".Length, " < ".Length);
                                break;
                            }
                        case 7:
                            {
                                parsedInstructions.Remove(parsedInstructions.Length - " == ".Length, " == ".Length);
                                break;
                            }
                    }
                    #endregion
                }

                switch (type)
                {
                    case 0:
                        {
                            resultingValue = arguments.Aggregate((a, b) => a + b);
                            break;
                        }
                    case 1:
                        {
                            resultingValue = arguments.Aggregate(new BigInteger(1), (a, b) => a * b);
                            break;
                        }
                    case 2:
                        {
                            resultingValue = arguments.Min();
                            break;
                        }
                    case 3:
                        {
                            resultingValue = arguments.Max();
                            break;
                        }
                    case 5:
                        {
                            resultingValue = arguments.Dequeue() > arguments.Dequeue() ? 1u : 0u;
                            break;
                        }
                    case 6:
                        {
                            resultingValue = arguments.Dequeue() < arguments.Dequeue() ? 1u : 0u;
                            break;
                        }
                    case 7:
                        {
                            resultingValue = arguments.Dequeue() == arguments.Dequeue() ? 1u : 0u;
                            break;
                        }
                }
                
                #region OutputOnly
                switch (type)
                {
                    case 0:
                        {
                            parsedInstructions.Append(")");
                            break;
                        }
                    case 1:
                        {
                            parsedInstructions.Append(")");
                            break;
                        }
                    case 2:
                        {
                            parsedInstructions.Append("}.Min()");
                            break;
                        }
                    case 3:
                        {
                            parsedInstructions.Append("}.Max()");
                            break;
                        }
                    case 5:
                        {
                            parsedInstructions.Append($")");
                            break;
                        }
                    case 6:
                        {
                            parsedInstructions.Append($")");
                            break;
                        }
                    case 7:
                        {
                            parsedInstructions.Append($")");
                            break;
                        }
                }
                #endregion

                break;
            }
    }

    length = currentIndex - startIndex;
    Console.WriteLine(indent + "    " + $"Value: {resultingValue}");
    Console.WriteLine(indent + "    " + "Length: " + length);

    Console.WriteLine(indent + "}");

    return resultingValue;
}

uint ReadIntFromBits(int startIndex, int bitCount, ref byte[] data, ref int currentIndex)
{
    uint parsedData = 0;

    for (int i = 0; i < bitCount; i++)
    {
        if (GetBitFromByteArray(data, startIndex + i))
        {
            parsedData |= 1u << (bitCount - 1 - i);
        }
    }
    currentIndex += bitCount;
    return parsedData;
}


bool GetBitFromByteArray(byte[] data, int bitIndex)
{
    return (data[bitIndex / 8] & (1 << (7 - bitIndex % 8))) > 0;
}

void AppendDigits(byte[] data, int startIndex, int bitCount, ConsoleColor color)
{
    for (int i = startIndex; i < startIndex + bitCount; i++)
    {
        parsed.Add((color, ((data[i / 8] & (1 << (7 - i % 8))) > 0) ? "1" : "0"));
    }
}

void PrintParsed()
{
    return;
    foreach (var parsedDitit in parsed)
    {
        Console.ForegroundColor = parsedDitit.color;
        Console.Write(parsedDitit.digit);
    }
    Console.ResetColor();
    for (int i = parsed.Count; i < data.Length * 8; i++)
    {
        Console.Write(((data[i / 8] & (1 << (7 - i % 8))) > 0) ? "1" : "0");
    }
    Console.WriteLine();
}

ulong result = ((Convert.ToUInt64(1196UL < 1196UL) * 30UL) + new [] {1577851UL, 8664UL}.Max() + new [] {196UL, 3580224UL, 2301UL, 63287UL}.Min() + new [] {2669UL}.Min() + (225UL * 59UL * 183UL * 129UL) + (Convert.ToUInt64(1946967272UL > 99UL) * 4578544UL) + (1947692UL * Convert.ToUInt64((14UL + 4UL + 2UL) < (8UL + 4UL + 6UL))) + new [] {707013UL}.Max() + (9504UL + 14UL) + (181UL * 182UL * 182UL * 217UL * 8UL) + new [] {7UL, 144UL, 15UL, 227537134UL}.Max() + new [] {new [] {(new [] {(new [] {((new [] {new [] {(new [] 
{(new [] {new [] {new [] {(new [] {(new [] {27956UL}.Max())}.Max())}.Max()}.Max()}.Max())}.Max())}.Min()}.Max()))}.Min())}.Max())}.Min()}.Max() + (Convert.ToUInt64((15UL + 8UL + 6UL) == (9UL + 6UL + 11UL)) * 16057443UL) + ((2UL * 11UL * 15UL) + (6UL * 3UL * 12UL) + (5UL * 15UL * 15UL)) + new [] {27084UL, 2522UL, 56UL, 403601UL, 221UL}.Min() + (Convert.ToUInt64((2UL + 13UL + 5UL) < (8UL + 14UL + 5UL)) * 405705UL) + (1000390UL * Convert.ToUInt64(206UL == 181537UL)) + 2UL + (60965UL) + (11UL) + (778UL * Convert.ToUInt64(54288UL < 35UL)) + (Convert.ToUInt64(97UL < 131134112UL) * 4UL) + (Convert.ToUInt64(167UL > 167UL) * 390367UL) + (3353UL * Convert.ToUInt64(155UL < 2350UL)) + (1324491436UL * Convert.ToUInt64(19UL < 19UL)) + (145UL * 196UL * 135UL) + (215UL * Convert.ToUInt64(3347UL == 156UL)) + (8092UL + 3UL + 234UL + 3498848726UL) + (107UL * 171UL) + (118840197UL + 680UL + 80UL + 599UL + 402128UL) + new [] {1UL, 176UL, 856UL, 32271UL, 12970UL}.Max() + (Convert.ToUInt64(6513237UL > 6513237UL) * 175037938UL) + (Convert.ToUInt64(4069UL > 10UL) * 544UL) + 884095UL + (159UL * Convert.ToUInt64(898UL > 5325350060278UL)) + 63UL + (46UL * Convert.ToUInt64((11UL + 14UL + 15UL) > (2UL + 5UL + 5UL))) + new [] {3186UL, 11UL}.Min() + 37362UL + 691221UL + new [] {9UL, 2068UL, 8UL}.Min() + (1441UL * Convert.ToUInt64(44977264440UL < 13UL)) + (12UL * Convert.ToUInt64(4609UL == 4609UL)) + new [] {6UL, 2274787348UL, 2402524000838UL}.Max() + (273226UL * Convert.ToUInt64(101UL > 1042UL)) + 17306UL + 177UL + 991521628476UL + (43234UL * Convert.ToUInt64((12UL + 11UL + 10UL) > (8UL + 2UL + 9UL))) + ((4UL + 11UL + 6UL) * (13UL + 12UL + 11UL) * (12UL + 11UL + 6UL)) + (5UL + 170UL + 25222069UL) + 11513108UL + 399112UL);

Console.WriteLine(result);