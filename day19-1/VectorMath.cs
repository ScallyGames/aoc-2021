public static class VectorMath
{
    public static int Dot(Vector3 a, Vector3 b) => a.X * b.X + a.Y * b.Y + a.Z * b.Z;

    public static Vector3 Cross(Vector3 a, Vector3 b) => new Vector3 { 
        X = a.Y * b.Z - a.Z * b.Y, // x
        Y = a.Z * b.X - a.X * b.Z, // y
        Z = a.X * b.Y - a.Y * b.X, // z
    };


    public static Vector3 Addition(Vector3 a, Vector3 b) => new Vector3 {
        X = a.X + b.X,
        Y = a.Y + b.Y,
        Z = a.Z + b.Z,
    };

    public static Vector3 Difference(Vector3 a, Vector3 b) => new Vector3 {
        X = a.X - b.X,
        Y = a.Y - b.Y,
        Z = a.Z - b.Z,
    };

    
    public static Vector3 Multiply(Vector3 source, int[,] transformationMatrix) => new Vector3 {
        X = source.X * transformationMatrix[0, 0] + source.Y * transformationMatrix[0, 1] + source.Z * transformationMatrix[0, 2],
        Y = source.X * transformationMatrix[1, 0] + source.Y * transformationMatrix[1, 1] + source.Z * transformationMatrix[1, 2],
        Z = source.X * transformationMatrix[2, 0] + source.Y * transformationMatrix[2, 1] + source.Z * transformationMatrix[2, 2],
    };
}