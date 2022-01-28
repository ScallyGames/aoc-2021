class InstructionList
{
    public Queue<Instruction> Instructions { get; private set; }

    public InstructionList(IEnumerable<string> instructions)
    {
        this.Instructions = new Queue<Instruction>();

        foreach(var instruction in instructions)
        {
            string[] parts = instruction.Split(' ');
            
            Instruction newInstruction = parts[0] switch
            {
                "inp" => (ref int[] registers, ref InputStream inputStream) =>
                {
                    registers[GetRegisterIndexFromName(parts[1])] = inputStream.Next();
                    return false;
                },
                "add" => (ref int[] registers, ref InputStream inputStream) =>
                {
                    int registerA = GetRegisterIndexFromName(parts[1]);
                    int valueA = registers[registerA];
                    int valueB = int.TryParse(parts[2], out int result) ? result : registers[GetRegisterIndexFromName(parts[2])];
                    registers[registerA] = valueA + valueB;
                    return false;
                },
                "mul" => (ref int[] registers, ref InputStream inputStream) =>
                {
                    int registerA = GetRegisterIndexFromName(parts[1]);
                    int valueA = registers[registerA];
                    int valueB = int.TryParse(parts[2], out int result) ? result : registers[GetRegisterIndexFromName(parts[2])];
                    registers[registerA] = valueA * valueB;
                    return false;
                },
                "div" => (ref int[] registers, ref InputStream inputStream) =>
                {
                    int registerA = GetRegisterIndexFromName(parts[1]);
                    int valueA = registers[registerA];
                    int valueB = int.TryParse(parts[2], out int result) ? result : registers[GetRegisterIndexFromName(parts[2])];
                    registers[registerA] = (int)Math.Truncate(valueA / (float)valueB);
                    return false;
                },
                "mod" => (ref int[] registers, ref InputStream inputStream) =>
                {
                    int registerA = GetRegisterIndexFromName(parts[1]);
                    int valueA = registers[registerA];
                    int valueB = int.TryParse(parts[2], out int result) ? result : registers[GetRegisterIndexFromName(parts[2])];
                    registers[registerA] = valueA % valueB;
                    return false;
                },
                "eql" => (ref int[] registers, ref InputStream inputStream) =>
                {
                    int registerA = GetRegisterIndexFromName(parts[1]);
                    int valueA = registers[registerA];
                    int valueB = int.TryParse(parts[2], out int result) ? result : registers[GetRegisterIndexFromName(parts[2])];
                    registers[registerA] = valueA == valueB ? 0 : 1;
                    return false;
                },
                _ => (ref int[] registers, ref InputStream inputStream) => false,
            };

            if(this.Instructions.Count > 0 && parts[0] == "inp")
            {
                this.Instructions.Enqueue((ref int[] registers, ref InputStream inputStream) => true);
            }

            this.Instructions.Enqueue(newInstruction);
        }
    }

    private InstructionList()
    {
        this.Instructions = new Queue<Instruction>();
    }

    public InstructionList Clone()
    {
        var clone = new InstructionList();
        clone.Instructions = new Queue<Instruction>(this.Instructions);
        return clone;
    }

    private static int GetRegisterIndexFromName(string registerName)
    {
        return registerName switch
        {
            "w" => 0,
            "x" => 1,
            "y" => 2,
            "z" => 3,
            _ => throw new ArgumentException(),
        };
    }
}