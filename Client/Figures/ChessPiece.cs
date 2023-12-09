using Svg;

namespace Client;

public abstract class ChessPiece
{
    public Position Pos;
    public SvgDocument Image; // заменить на readonly, но не знаю как
    public FigureColor ChessPieceColor; // заменить на readonly, но не знаю как

    protected ChessPiece(int x, int y, FigureColor color = FigureColor.White)
    {
        Pos = new Position(x, y);
        Image = new SvgDocument();
        ChessPieceColor = color;
    }

    public virtual void ChangePosition(int x, int y)
    {
        this.Pos.X = x;
        this.Pos.Y = y;
    }
    
    public virtual void ChangePosition(Position pos)
    {
        this.Pos.X = pos.X;
        this.Pos.Y = pos.Y;
    }
    
    public override string ToString()
    {
        // return (char) (65 + this.Pos.X) + (8 - this.Pos.Y).ToString();
        return this.Pos.X + " " + this.Pos.Y;
    }

    public virtual List<Position> NextMove()
    {
        return new List<Position>();
    }

    public int NumInTable()
    {
        return this.Pos.Y * 8 + this.Pos.X;
    }
}