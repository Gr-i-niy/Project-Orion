using Svg;

namespace Server;

public class Rook: ChessPiece
{
    private bool _moved;
    public Rook(int x, int y, FigureColor color): base(x, y, color)
    {
        Pos = new Position(x, y);
        _moved = false;
        ChessPieceColor = color; //readonly, не знаю как
        if (color == FigureColor.White)
            Image = SvgDocument.Open(Application.StartupPath + @"\Assets\black_rook.svg");
        else
            Image = SvgDocument.Open(Application.StartupPath + @"\Assets\white_rook.svg");
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