class ALU : IEquatable<ALU>
{
    private int[] registers = new int[4];
    private InputStream input;
    private InstructionList instructions;

    public ALU(InputStream input, InstructionList instructions)
    {
        this.input = input;
        this.instructions = instructions;
    }
    
    public void Compute()
    {
        while(instructions.Instructions.Count > 0)
        {
            int nextInput = this.input.Peek();
            if(ALUMemoization.Storage.ContainsKey((this, nextInput)))
            {
                var targetState = ALUMemoization.Storage[(this, nextInput)];
                this.registers = targetState.registers;
                this.instructions = targetState.instructions;
                this.input.Next();
            }
            else
            {
                var currentState = this.Clone();
                this.computeUntilNextInput();
                ALUMemoization.Storage.Add((currentState, nextInput), this.Clone());
            }
        }
    }

    private void computeUntilNextInput()
    {
        while(instructions.Instructions.Count > 0)
        {
            var instruction = instructions.Instructions.Dequeue();
            
            var nextIsInput = instruction(ref this.registers, ref input);

            if(nextIsInput)
            {
                return;
            }
        }
    }

    public int GetZ()
    {
        return this.registers[3];
    }

    public ALU Clone()
    {
        var clone = new ALU(this.input.Clone(), this.instructions.Clone());
        clone.registers = this.registers.ToArray();
        return clone;
    }

    public override int GetHashCode()
    {
        int hashCode = 0;
        hashCode = 53 * hashCode + this.registers[0] + 137;
        hashCode = 53 * hashCode + this.registers[1] + 137;
        hashCode = 53 * hashCode + this.registers[2] + 137;
        hashCode = 53 * hashCode + this.registers[3] + 137;
        hashCode = 53 * hashCode + this.input.CurrentIndex + 137;
        return hashCode;
    }

    public bool Equals(ALU? other)
    {
        if(other == null) return false;

        return  this.registers[0] == other.registers[0] &&
            this.registers[1] == other.registers[1] &&
            this.registers[2] == other.registers[2] &&
            this.registers[3] == other.registers[3] &&
            this.input.CurrentIndex == other.input.CurrentIndex;
    }
}


static class ALUMemoization
{
    public static Dictionary<(ALU, int), ALU> Storage = new Dictionary<(ALU, int), ALU>();
}