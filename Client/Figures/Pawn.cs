using Svg;

namespace Client;

public class Pawn: ChessPiece
{
    private bool _moved;

    public Pawn(int x, int y, FigureColor color): base(x, y, color)
    {
        Pos = new Position(x, y);
        _moved = false;
        ChessPieceColor = color; //readonly, не знаю как
        if (color == FigureColor.White)
            Image = SvgDocument.Open(Application.StartupPath + @"\Assets\white_pawn.svg");
        else
            Image = SvgDocument.Open(Application.StartupPath + @"\Assets\black_pawn.svg");
    }
    //???|
    public override void ChangePosition(int x, int y)
    {
        this.Pos.X = x;
        this.Pos.Y = y;
        _moved = true;
    }
    public override void ChangePosition(Position pos)
    {
        this.Pos.X = pos.X;
        this.Pos.Y = pos.Y;
        _moved = true;
    }

    public override List<Position> NextMove()
    {
        List<Position> possibleMoves = new();
        List<Position> nextMoves = new();
        if (this.Pos.Y == 0)
            return nextMoves;
        if (!_moved)
        {
            var fl1 = true;
            var fl2 = true;
            foreach (var figure in GlobalVariables.WhiteChessPieces)
            {
                if (figure.Pos == new Position(this.Pos.X, this.Pos.Y - 2))
                    fl1 = false;
                if (figure.Pos == new Position(this.Pos.X, this.Pos.Y - 1))
                {
                    fl2 = false;
                    break;
                }
            }
            if (fl2)
                possibleMoves.Add(new Position(this.Pos.X, this.Pos.Y - 1));
            if (fl1 && fl2)
                possibleMoves.Add(new Position(this.Pos.X, this.Pos.Y - 2));
        }
        else
        {
            if (GlobalVariables.WhiteChessPieces.All(figure => figure.Pos != new Position(this.Pos.X, this.Pos.Y - 1)))
                possibleMoves.Add(new Position(this.Pos.X, this.Pos.Y - 1));
        }
        foreach (var figure in GlobalVariables.BlackChessPieces)
        {
            if (figure.Pos == new Position(this.Pos.X + 1, this.Pos.Y - 1))
                nextMoves.Add(new Position(this.Pos.X + 1, this.Pos.Y - 1));
            if (figure.Pos == new Position(this.Pos.X - 1, this.Pos.Y - 1))
                nextMoves.Add(new Position(this.Pos.X - 1, this.Pos.Y - 1));
            if (figure.Pos == new Position(this.Pos.X, this.Pos.Y - 1))
                possibleMoves.Clear();
            if (figure.Pos == new Position(this.Pos.X, this.Pos.Y - 2) && possibleMoves.Count == 2)
                possibleMoves.RemoveAt(1);
        }
        nextMoves.AddRange(possibleMoves);
        return nextMoves;
    }
}
