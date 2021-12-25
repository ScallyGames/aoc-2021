using System.Diagnostics;

public class FishNumber
{
    private FishNumber? parent;
    private FishNumber? left;
    public FishNumber? Left
    { 
        get
        {
            return this.left;
        }
        set
        {
            if(this.left != value)
            {
                if(this.left != null)
                {
                    this.left.parent = null;
                }
                this.left = value;
                if(this.left != null)
                {
                    this.left.parent = this;
                }
            }
        }
    }
    private FishNumber? right;
    public FishNumber? Right
    { 
        get
        {
            return this.right;
        }
        set
        {
            if(this.right != value)
            {
                if(this.right != null)
                {
                    this.right.parent = null;
                }
                this.right = value;
                if(this.right != null)
                {
                    this.right.parent = this;
                }
            }
        }
    }
    
    private byte literalValue;

    public byte LiteralValue 
    {
        get 
        {
            Debug.Assert(this.IsLiteralValue);
            return this.literalValue;
        }
    }
    public bool IsLiteralValue => this.Left == null && this.Right == null;

    public FishNumber(FishNumber left, FishNumber right)
    {
        this.Left = left;
        this.Right = right;
    }

    public FishNumber(byte literalValue)
    {
        this.literalValue = literalValue;
    }

    public FishNumber(string input)
    {
        if(input.Length == 1)
        {
            this.literalValue = byte.Parse(input);
        }
        else
        {
            int numberOfBraces = 0;

            int startOfLeft = 0;
            int endOfLeft = 0;
            int startOfRight = 0;
            int endOfRight = 0;

            for(int i = 0; i < input.Length; i++)
            {
                switch(input[i])
                {
                    case '[':
                    {
                        numberOfBraces++;
                        if(numberOfBraces == 1)
                        {
                            startOfLeft = i + 1;
                        }
                        break;
                    }
                    case ']':
                    {
                        numberOfBraces--;
                        if(numberOfBraces == 0)
                        {
                            endOfRight = i - 1;
                        }
                        break;
                    }
                    case ',':
                    {
                        if(numberOfBraces == 1)
                        {
                            endOfLeft = i - 1;
                            startOfRight = i + 1;
                        }
                        break;
                    }
                }
            }

            string partLeft = input.Substring(startOfLeft, endOfLeft - startOfLeft + 1);
            string partRight = input.Substring(startOfRight, endOfRight - startOfRight + 1);

            this.Left = new FishNumber(partLeft);
            this.Right = new FishNumber(partRight);
        }
    }

    public override string ToString()
    {
        if(this.IsLiteralValue)
        {
            return this.literalValue.ToString();
        }
        else
        {
            return $"[{left!.ToString()},{right!.ToString()}]";
        }
    }

    public void Reduce()
    {
        bool hasChanged = true;

        while(hasChanged)
        {
            if(this.Explode()) continue;
            if(this.Split()) continue;
            hasChanged = false;
        }
    }

    public bool Explode(int depth = 0)
    {
        if(this.IsLiteralValue) return false;

        if(depth == 4)
        {
            this.AddRightmost(this.Left!.LiteralValue, this, this);
            this.AddLeftmost(this.Right!.LiteralValue, this, this);

            this.Left = null;
            this.Right = null;
            this.literalValue = 0;
            return true;
        }

        if(this.Left!.Explode(depth + 1)) return true;
        if(this.Right!.Explode(depth + 1)) return true;
        return false;
    }

    private bool AddLeftmost(byte valueToAdd, FishNumber calledFrom, FishNumber initiator)
    {
        if(initiator == this)
        {
            if(this.parent != null)
            {
                return this.parent.AddLeftmost(valueToAdd, this, initiator);
            }
            return false;
        }

        if(this.IsLiteralValue)
        {
            this.literalValue += valueToAdd;
            return true;
        }

        if(calledFrom == this.parent)
        {
            if(this.Left!.AddLeftmost(valueToAdd, this, initiator)) return true;
            if(this.Right!.AddLeftmost(valueToAdd, this, initiator)) return true;
            return false;
        }

        if(calledFrom == this.Left)
        {
            if(this.Right!.AddLeftmost(valueToAdd, this, initiator)) return true;
        }

        if(this.parent != null)
        {
            return this.parent.AddLeftmost(valueToAdd, this, initiator);
        }

        return false;
    }

    private bool AddRightmost(byte valueToAdd, FishNumber calledFrom, FishNumber initiator)
    {
        if(initiator == this)
        {
            if(this.parent != null)
            {
                return this.parent.AddRightmost(valueToAdd, this, initiator);
            }
            return false;
        }

        if(this.IsLiteralValue)
        {
            this.literalValue += valueToAdd;
            return true;
        }

        if(calledFrom == this.parent)
        {
            if(this.Right!.AddRightmost(valueToAdd, this, initiator)) return true;
            if(this.Left!.AddRightmost(valueToAdd, this, initiator)) return true;
            return false;
        }

        if(calledFrom == this.Right)
        {
            if(this.Left!.AddRightmost(valueToAdd, this, initiator)) return true;
        }

        if(this.parent != null)
        {
            return this.parent.AddRightmost(valueToAdd, this, initiator);
        }

        return false;
    }

    public bool Split()
    {
        if(this.IsLiteralValue)
        {
            if(this.literalValue > 9)
            {
                this.Left = new FishNumber((byte)Math.Floor(this.literalValue / 2f));
                this.Right = new FishNumber((byte)Math.Ceiling(this.literalValue / 2f));
                this.literalValue = 0;
                return true;
            }
            else
            {
                return false;
            }
        }
        else
        {
            if(this.Left!.Split()) return true;
            if(this.Right!.Split()) return true;
            return false;
        }
    }

    public ulong GetMagnitude()
    {
        if(this.IsLiteralValue) return (ulong)this.LiteralValue;

        return 3 * this.left!.GetMagnitude() + 2 * this.right!.GetMagnitude();
    }
}