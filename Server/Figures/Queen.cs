using Svg;

namespace Server;

public class Queen: ChessPiece
{
    public Queen(int x, int y, FigureColor color): base(x, y, color)
    {
        Pos = new Position(x, y);
        if (color == FigureColor.White)
            Image = SvgDocument.Open(Application.StartupPath + @"\Assets\black_queen.svg");
        else
            Image = SvgDocument.Open(Application.StartupPath + @"\Assets\white_queen.svg");
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
        var positionUp = new Position(this.Pos.X, this.Pos.Y + 1);
        var positionLeft = new Position(this.Pos.X - 1, this.Pos.Y);
        while (positionUp.Y < 8 || positionLeft.X > -1)
        {
            foreach (var figure in GlobalVariables.WhiteChessPieces)
            {
                if (positionUp == figure.Pos)
                    positionUp.Y += 1000;
                if (positionLeft == figure.Pos)
                    positionLeft.X -= 1000;
            }
            if (positionUp.Y < 8)
                nextMoves.Add(new Position(positionUp.X, positionUp.Y));
            if (positionLeft.X > -1)
                nextMoves.Add(new Position(positionLeft.X, positionLeft.Y));
            
            foreach (var figure in GlobalVariables.BlackChessPieces)
            {
                if (positionUp == figure.Pos)
                    positionUp.Y += 1000;
                if (positionLeft == figure.Pos)
                    positionLeft.X -= 1000;
            }
            positionUp.Y += 1;
            positionLeft.X -= 1;
        }
        
        var positionDown = new Position(this.Pos.X, this.Pos.Y - 1);
        var positionRight = new Position(this.Pos.X + 1, this.Pos.Y);
        while (positionDown.Y > -1 || positionRight.X < 8)
        {
            foreach (var figure in GlobalVariables.WhiteChessPieces)
            {
                if (positionDown == figure.Pos)
                    positionDown.Y -= 1000;
                if (positionRight == figure.Pos)
                    positionRight.X += 1000;
            }

            if (positionDown.Y > -1)
                nextMoves.Add(new Position(positionDown.X, positionDown.Y));

            if (positionRight.X < 8)
                nextMoves.Add(new Position(positionRight.X, positionRight.Y));

            foreach (var figure in GlobalVariables.BlackChessPieces)
            {
                if (positionDown == figure.Pos)
                    positionDown.Y -= 1000;
                if (positionRight == figure.Pos)
                    positionRight.X += 1000;
            }
            positionDown.Y -= 1;
            positionRight.X += 1;
        }
        return nextMoves;
    }
}