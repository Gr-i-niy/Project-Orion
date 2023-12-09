using Svg;

namespace Client;

public class Bishop: ChessPiece
{
    public Bishop(int x, int y, FigureColor color): base(x, y, color)
    {
        Pos = new Position(x, y);
        ChessPieceColor = color;
        if (color == FigureColor.White)
            Image = SvgDocument.Open(Application.StartupPath + @"\Assets\white_bishop.svg");
        else
            Image = SvgDocument.Open(Application.StartupPath + @"\Assets\black_bishop.svg");
    }

    public override List<Position> NextMove()
    {
        List<Position> nextMoves = new();
        
        var positionSide = new Position(this.Pos.X + 1, this.Pos.Y + 1);
        var positionMain = new Position(this.Pos.X - 1, this.Pos.Y + 1);
        while (positionSide is { X: < 8, Y: < 8 } || positionMain is { X: > -1, Y: < 8 })
        {
            foreach (var figure in GlobalVariables.WhiteChessPieces)
            {
                if (positionSide == figure.Pos)
                    positionSide.Y += 1000;
                if (positionMain == figure.Pos)
                    positionMain.Y += 1000;
            }

            if (positionSide is { X: < 8, Y: < 8 })
                nextMoves.Add(new Position(positionSide.X, positionSide.Y));
            if (positionMain is { X: > -1, Y: < 8 })
                nextMoves.Add(new Position(positionMain.X, positionMain.Y));
            
            foreach (var figure in GlobalVariables.BlackChessPieces)
            {
                if (positionSide == figure.Pos)
                    positionSide.Y += 1000;
                if (positionMain == figure.Pos)
                    positionMain.Y += 1000;
            }
            positionSide.X += 1;
            positionSide.Y += 1;
            positionMain.X -= 1;
            positionMain.Y += 1;
        }
        
        positionSide = new Position(this.Pos.X + 1, this.Pos.Y - 1);
        positionMain = new Position(this.Pos.X - 1, this.Pos.Y - 1);
        while (positionSide is { X: < 8, Y: > -1 } || positionMain is { X: > -1, Y: > -1 })
        {
            foreach (var figure in GlobalVariables.WhiteChessPieces)
            {
                if (positionSide == figure.Pos)
                    positionSide.Y -= 1000;
                if (positionMain == figure.Pos)
                    positionMain.Y -= 1000;
            }
            if (positionSide is { X: < 8, Y: > -1 })
                nextMoves.Add(new Position(positionSide.X, positionSide.Y));
            if (positionMain is { X: > -1, Y: > -1 })
                nextMoves.Add(new Position(positionMain.X, positionMain.Y));
            
            foreach (var figure in GlobalVariables.BlackChessPieces)
            {
                if (positionSide == figure.Pos)
                    positionSide.Y -= 1000;
                if (positionMain == figure.Pos)
                    positionMain.Y -= 1000;
            }
            positionSide.X += 1;
            positionSide.Y -= 1;
            positionMain.X -= 1;
            positionMain.Y -= 1;
        }
        
        return nextMoves;
    }
    
}