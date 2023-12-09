using Svg;

namespace Client;

public class King: ChessPiece
{
    private bool _moved;

    public King(int x, int y, FigureColor color): base(x, y, color)
    {
        Pos = new Position(x, y);
        _moved = false;
        if (color == FigureColor.Black)
            Image = SvgDocument.Open(Application.StartupPath + @"\Assets\black_king.svg");
        else
            Image = SvgDocument.Open(Application.StartupPath + @"\Assets\white_king.svg");
    }
    
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
        List<Position> nextMoves = new();
        for (int i = -1; i < 2; i++)
        {
            for (int j = -1; j < 2; j++)
            {
                if (this.Pos.X + i > -1 && this.Pos.X + i < 8 && this.Pos.Y + j > -1 && this.Pos.Y + j < 8)
                {
                    var fl = false;
                    var pos = new Position(this.Pos.X + i, this.Pos.Y + j);
                    foreach (var figure in GlobalVariables.WhiteChessPieces)
                    {
                        if (figure.Pos == pos)
                        {
                            fl = true;
                            break;
                        }
                    }
                    if (fl)
                        continue;
                    nextMoves.Add(pos);
                }
            }
        }
        return nextMoves;
    }
}