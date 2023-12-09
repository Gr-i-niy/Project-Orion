using Svg;

namespace Client;

public partial class MainForm : Form
{
    private readonly TcpCommunication.Client _connection;
    public MainForm()
    {
        GetIPForm getIpForm = new();
        getIpForm.ShowDialog();
        _connection =  new(GlobalVariables.UserIp, 8888);
        _connection.Connect();
        InitializeComponent();
    }
        
    private static void UpdateButtonImage(Button button, int numInTable)
    {
        button.Image = GlobalVariables.ChessBoardButtonsImages[numInTable].Draw(button.Width, button.Height);
        button.BackgroundImage = 
            GlobalVariables.ChessBoardButtonsBackgroundImages[numInTable].Draw(button.Width, button.Height);
    }
    
    private void Form_SizeChanged(object sender, EventArgs e)
    {
        UpdateChessBoardSize();
    }
    
    private void UpdateChessBoardSize()
    {
        int minSize = Math.Min(this.ClientSize.Width, this.ClientSize.Height);
        chessBoard.Width = minSize;
        chessBoard.Height = minSize;
        int x = (this.ClientSize.Width - chessBoard.Width) / 2;
        chessBoard.Location = new Point(x, 0);

        foreach (var button in GlobalVariables.ChessBoardButtons)
            button.Size = new Size(minSize / 8, minSize / 8);
    }

    private void OnClickAction(Button button, int numInTable)
    {
        if (!GlobalVariables.FlActive) return;
        bool fl = true; // if user choosed figure -> false
        Position buttonPosition = new(numInTable % 8, numInTable / 8);
        int a = buttonPosition.Y - buttonPosition.X;
        int b = buttonPosition.Y - buttonPosition.X + 8;
        for (int i = 0; i < 16; i++)
        {
            var figure = GlobalVariables.WhiteChessPieces[i];
            if (figure.Pos == buttonPosition)
            {
                fl = false;
                ClearChessBoardBackground(GlobalVariables.LastPossibleMoves);
                DrawNextMoves(figure.NextMove());
                GlobalVariables.ChoosedFigureIndex = i;
            }
        }
        
        if (fl)
            ClearChessBoardBackground(GlobalVariables.LastPossibleMoves);
        
        foreach (var possibleMove in GlobalVariables.LastPossibleMoves.ToList())
        {
            if (possibleMove != buttonPosition) continue;
            if (GlobalVariables.WhiteChessPieces[GlobalVariables.ChoosedFigureIndex].Pos == buttonPosition) continue;
            var t = (GlobalVariables.WhiteChessPieces[GlobalVariables.ChoosedFigureIndex].Pos, buttonPosition);
            t = (new Position(t.Item1.X, t.Item1.Y), new Position(t.Item2.X, t.Item2.Y));
            ChessPieceMove(GlobalVariables.WhiteChessPieces[GlobalVariables.ChoosedFigureIndex], buttonPosition);
            foreach (var figure in GlobalVariables.BlackChessPieces.ToList())
            {
                if (figure.Pos != buttonPosition) continue;
                ChessPieceCut(figure);
                break;
            }
            Console.WriteLine("Waiting for pos...");
            Refresh();
            _connection.SendPos(t.Item1, t.Item2);
            GlobalVariables.FlActive = false;
            Refresh();
            var tmp = _connection.RecievePos();
            GlobalVariables.FlActive = true;
            Console.WriteLine("Recieved pos: " + " " + tmp.Item1 + " " + tmp.Item2);
            foreach (var figure1 in GlobalVariables.BlackChessPieces)
            {
                if (figure1.Pos != tmp.Item1) continue;
                ChessPieceMove(figure1, tmp.Item2);
                foreach (var figure2 in GlobalVariables.WhiteChessPieces.ToList())
                {
                    if (figure2.Pos != figure1.Pos) continue;
                    ChessPieceCut(figure2);
                    break;
                }
                break;
            }
            break;
        }
    }

    private void DrawNextMoves(List<Position> nextMoves)
    {
        SvgDocument svgDocument = 
            SvgDocument.Open(Application.StartupPath + @"\Assets\grey_circle.svg");
        foreach (var i in GlobalVariables.BlackChessPieces)
        {
            if (i is not King) continue;
            for (var j = 0; j < nextMoves.Count; j++)
            {
                if (nextMoves[j] != i.Pos) continue;
                nextMoves.RemoveAt(j);
                break;
            }
            break;
        }
        GlobalVariables.LastPossibleMoves = nextMoves;
        foreach (var pos in nextMoves)
        {
            var button = GlobalVariables.ChessBoardButtons[pos.NumInTable()];
            button.BackgroundImage = svgDocument.Draw(button.Width, button.Height);
            GlobalVariables.ChessBoardButtonsBackgroundImages[pos.NumInTable()] = svgDocument;
        }
    }

    private void ClearChessBoard(Position posToClear)
    {
        GlobalVariables.ChessBoardButtons[posToClear.NumInTable()].Image = null;
        GlobalVariables.ChessBoardButtonsImages[posToClear.NumInTable()] = new SvgDocument();
    }
    
    private void ClearChessBoardBackground(List<Position> posToClear)
    {
        foreach (var pos in posToClear)
        {
            GlobalVariables.ChessBoardButtons[pos.NumInTable()].BackgroundImage = null;
            GlobalVariables.ChessBoardButtonsBackgroundImages[pos.NumInTable()] = new SvgDocument();
        }
    }
    
    private void ClearChessBoardBackground(Position posToClear)
    {
        GlobalVariables.ChessBoardButtons[posToClear.NumInTable()].BackgroundImage = null;
        GlobalVariables.ChessBoardButtonsBackgroundImages[posToClear.NumInTable()] = new SvgDocument();
    }
    
    private void ChessPieceMove(ChessPiece figure, Position moveTo)
    {
        ClearChessBoard(figure.Pos);
        figure.ChangePosition(moveTo);
        int height = GlobalVariables.ChessBoardButtons[figure.NumInTable()].Height;
        int width = GlobalVariables.ChessBoardButtons[figure.NumInTable()].Width;
        GlobalVariables.ChessBoardButtons[figure.NumInTable()].Image = figure.Image.Draw(width, height);
        GlobalVariables.ChessBoardButtonsImages[figure.NumInTable()] = figure.Image;
        GlobalVariables.LastPossibleMoves.Clear();
    }

    private void ChessPieceCut(ChessPiece figure)
    {
        if (figure.ChessPieceColor == FigureColor.Black)
        {
            GlobalVariables.ChoppedBlackChessPieces.Add(figure);
            GlobalVariables.BlackChessPieces.Remove(figure);
        }
        else
        {
            GlobalVariables.ChoppedWhiteChessPieces.Add(figure);
            GlobalVariables.WhiteChessPieces.Remove(figure);
        }
    } 
}