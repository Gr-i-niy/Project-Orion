using Svg;

namespace Server;

public static class GlobalVariables
{
    public static List<ChessPiece> WhiteChessPieces = new(16)
        {new Rook(0, 0, FigureColor.White), 
        new Knight(0, 0, FigureColor.White),
        new Bishop(0, 0, FigureColor.White), 
        new Queen(0, 0, FigureColor.White),
        new King(0, 0, FigureColor.White),
        new Bishop(0, 0, FigureColor.White),
        new Knight(0, 0, FigureColor.White),
        new Rook(0, 0, FigureColor.White)};

    public static List<ChessPiece> BlackChessPieces = new(16)
        {new Rook(0, 0, FigureColor.Black), 
        new Knight(0, 0, FigureColor.Black),
        new Bishop(0, 0, FigureColor.Black), 
        new Queen(0, 0, FigureColor.Black),
        new King(0, 0, FigureColor.Black),
        new Bishop(0, 0, FigureColor.Black),
        new Knight(0, 0, FigureColor.Black),
        new Rook(0, 0, FigureColor.Black)};

    public static List<ChessPiece> ChoppedWhiteChessPieces = new(15);
    public static List<ChessPiece> ChoppedBlackChessPieces = new(15);

    public static List<Button> ChessBoardButtons = new(64);
    public static List<SvgDocument> ChessBoardButtonsImages = new(64);
    public static List<SvgDocument> ChessBoardButtonsBackgroundImages = new(64);
    
    public static List<Position> LastPossibleMoves = new(); // Cсылка?
    public static int ChoosedFigureIndex;

    public static string UserIp = "";
    public static bool FlActive = true;
    public static bool FlCheck = false;
}