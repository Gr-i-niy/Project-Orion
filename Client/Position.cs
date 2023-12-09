namespace Client;

public class Position
{
    public int X, Y;

    public Position(int x, int y)
    {
        X = x;
        Y = y;
    }

    public override string ToString()
    {
        // return (char) (65 + this.Pos.X) + (8 - this.Pos.Y).ToString();
        return this.X + " " + this.Y;
    }

    public Position ReversePos()
    {
        return new Position(this.X, 7 - this.Y);
    }
    
    public static bool operator ==(Position obj1, Position obj2)
    {
        if ((obj1.X == obj2.X) && (obj1.Y == obj2.Y))
            return true;
        return false;
    }
    public static bool operator !=(Position obj1, Position obj2)
    {
        if ((obj1.X != obj2.X) || (obj1.Y != obj2.Y))
            return true;
        return false;
    }

    public static Position operator +(Position obj1, Position obj2)
    {
        return new Position(obj1.X + obj2.X, obj1.Y + obj2.Y);
    }
    
    public int NumInTable()
    {
        return this.Y * 8 + this.X;
    }
}