using Client.Figures;
using Svg;

namespace Client;

public class Knight: ChessPiece
{
    public Knight(int x, int y, FigureColor color): base(x, y, color)
    {
        Pos = new Position(x, y);
        ChessPieceColor = color;
        if (color == FigureColor.White)
            Image = SvgDocument.Open(Application.StartupPath + @"\Assets\white_knight.svg");
        else
            Image = SvgDocument.Open(Application.StartupPath + @"\Assets\black_knight.svg");
    }

    public override List<Position> NextMove()
    {
        List<ChessPiece> allyFigure;
        if (this.ChessPieceColor == FigureColor.White)
            allyFigure = GlobalVariables.WhiteChessPieces;
        else
            allyFigure = GlobalVariables.BlackChessPieces;
        List<Position> nextMoves = new();
        for (int i = -2; i < 3; i++)
        {
            for (int j = -2; j < 3; j++)
            {
                if (Math.Abs(i) + Math.Abs(j) != 3)
                    continue;
                if (this.Pos.X + i > -1 && this.Pos.X + i < 8 && this.Pos.Y + j > -1 && this.Pos.Y + j < 8)
                {
                    var fl = false;
                    var pos = new Position(this.Pos.X + i, this.Pos.Y + j);
                    foreach (var figure in allyFigure)
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