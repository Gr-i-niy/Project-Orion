using System.Runtime.CompilerServices;
using Client.Figures;
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
        var fl = true; // if user choosed figure -> false
        Position buttonPosition = new(numInTable % 8, numInTable / 8);
        for (int i = 0; i < GlobalVariables.WhiteChessPieces.Count; i++)
        {
            var figure = GlobalVariables.WhiteChessPieces[i];
            if (figure.Pos == buttonPosition)
            {
                fl = false;
                ClearChessBoardBackground(GlobalVariables.LastPossibleMoves);
                List<Position> t = new();
                Position l = new(figure.Pos.X, figure.Pos.Y);
                if (GlobalVariables.CheckWhite)
                {
                    foreach (var p in figure.NextMove())
                    {
                        figure.Pos.X = p.X;
                        figure.Pos.Y = p.Y;
                        if (p == GlobalVariables.Checker || !KingCheck(FigureColor.White))
                            t.Add(p);
                    }
                }
                else
                {
                    foreach (var p in figure.NextMove())
                    {
                        figure.Pos.X = p.X;
                        figure.Pos.Y = p.Y;
                        if (!KingCheck(FigureColor.White) || p == GlobalVariables.Checker)
                            t.Add(p);
                    }
                }
                figure.Pos.X = l.X;
                figure.Pos.Y = l.Y;
                List<Position> jhf = new();
                if (figure is King)
                {
                    foreach (var f in t)
                    {
                        var fl2 = true;
                        foreach (var j in GlobalVariables.BlackChessPieces)
                        {
                            if (f != j.Pos) continue;
                            fl2 = false;
                            ChessPieceCut(j);
                            if (!KingCheck(FigureColor.White))
                                jhf.Add(f);
                            GlobalVariables.BlackChessPieces.Add(GlobalVariables.ChoppedBlackChessPieces.Last());
                            GlobalVariables.ChoppedBlackChessPieces.RemoveAt(GlobalVariables.ChoppedBlackChessPieces.Count - 1);
                            break;
                        }
                        if (fl2)
                            jhf.Add(f);
                    }
                }
                else
                    jhf = t;
                DrawNextMoves(jhf);
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
            if (KingCheck(FigureColor.Black))
            {
                CheckMate(FigureColor.Black);
                GlobalVariables.CheckBlack = true;
                foreach (var i in GlobalVariables.BlackChessPieces)
                {
                    if (i is not King) continue;
                    GlobalVariables.ChessBoardButtons[i.NumInTable()].BackColor = Color.Red;
                    break;
                }
            }
            else
            {
                GlobalVariables.CheckBlack = false;
                for (var i = 0; i < GlobalVariables.ChessBoardButtons.Count; i++)
                {
                    if (GlobalVariables.ChessBoardButtons[i].BackColor != Color.Red) continue;
                    var btn = GlobalVariables.ChessBoardButtons[i];
                    if ((i % 8 + i / 8) % 2 == 1) btn.BackColor = Color.DarkOliveGreen;
                    else btn.BackColor = Color.LightGoldenrodYellow;
                    break;
                }
            }
            Refresh();
            _connection.SendPos(t.Item1, t.Item2);
            GlobalVariables.FlActive = false;
            Refresh();
            var tmp = _connection.RecievePos();
            if (tmp.Item1 == new Position(-1, 0))
                CheckMate(FigureColor.Black);
            else if (tmp.Item1 == new Position(0, -1))
                CheckMate(FigureColor.White);
            GlobalVariables.FlActive = true;
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
            if (KingCheck(FigureColor.White, true))
            {
                CheckMate(FigureColor.White);
                GlobalVariables.CheckWhite = true;
                foreach (var i in GlobalVariables.WhiteChessPieces)
                {
                    if (i is not King) continue;
                    GlobalVariables.ChessBoardButtons[i.NumInTable()].BackColor = Color.Red;
                    break;
                }
            }
            else
            {
                GlobalVariables.CheckWhite = false;
                for (var i = 0; i < GlobalVariables.ChessBoardButtons.Count; i++)
                {
                    if (GlobalVariables.ChessBoardButtons[i].BackColor != Color.Red) continue;
                    var btn = GlobalVariables.ChessBoardButtons[i];
                    if ((i % 8 + i / 8) % 2 == 1) btn.BackColor = Color.DarkOliveGreen;
                    else btn.BackColor = Color.LightGoldenrodYellow;
                    break;
                }
            }
            break;
        }
    }

    private void CheckMate(FigureColor clr)
    {
        if (FigureColor.White == clr)
        {
            foreach (var i in GlobalVariables.WhiteChessPieces)
            {
                Position tmp = new(i.Pos.X, i.Pos.Y);
                foreach (var j in i.NextMove())
                {
                    var fl = false;
                    foreach (var k in GlobalVariables.BlackChessPieces)
                    {
                        if (k.Pos != j) continue;
                        ChessPieceCut(k);
                        fl = true;
                        break;
                    }
                    i.Pos.X = j.X;
                    i.Pos.Y = j.Y;
                    if (!KingCheck(FigureColor.White))
                    {
                        i.Pos.X = tmp.X;
                        i.Pos.Y = tmp.Y;
                        if (fl)
                        {
                            GlobalVariables.BlackChessPieces.Add(GlobalVariables.ChoppedBlackChessPieces.Last());
                            GlobalVariables.ChoppedBlackChessPieces.RemoveAt(GlobalVariables.ChoppedBlackChessPieces.Count - 1);
                        }
                        return;
                    }

                    if (fl)
                    {
                        GlobalVariables.BlackChessPieces.Add(GlobalVariables.ChoppedBlackChessPieces.Last());
                        GlobalVariables.ChoppedBlackChessPieces.RemoveAt(GlobalVariables.ChoppedBlackChessPieces.Count - 1);
                    }
                }
                i.Pos.X = tmp.X;
                i.Pos.Y = tmp.Y;
            }
            _connection.SendPos(new Position(-1, 0), new Position(-1, -1));
            Refresh();
            MessageBox.Show("Чёрные победили");
            _connection.Close();
            this.Enabled = false;
        }
        foreach (var i in GlobalVariables.BlackChessPieces)
        {
            Position tmp = new(i.Pos.X, i.Pos.Y);
            foreach (var j in i.NextMove())
            {
                var fl = false;
                foreach (var k in GlobalVariables.WhiteChessPieces)
                {
                    if (k.Pos != j) continue;
                    ChessPieceCut(k);
                    fl = true;
                    break;
                }
                i.Pos.X = j.X;
                i.Pos.Y = j.Y;
                if (!KingCheck(FigureColor.Black))
                {
                    i.Pos.X = tmp.X;
                    i.Pos.Y = tmp.Y;
                    if (fl)
                    {
                        GlobalVariables.WhiteChessPieces.Add(GlobalVariables.ChoppedWhiteChessPieces.Last());
                        GlobalVariables.ChoppedWhiteChessPieces.RemoveAt(GlobalVariables.ChoppedWhiteChessPieces.Count -
                                                                         1);
                    }
                    return;
                }
                if (fl)
                {
                    GlobalVariables.WhiteChessPieces.Add(GlobalVariables.ChoppedWhiteChessPieces.Last());
                    GlobalVariables.ChoppedWhiteChessPieces.RemoveAt(GlobalVariables.ChoppedWhiteChessPieces.Count -
                                                                     1);
                }
            }
            i.Pos.X = tmp.X;
            i.Pos.Y = tmp.Y;
        }
        _connection.SendPos(new Position(0, -1), new Position(-1, -1));
        Refresh();
        MessageBox.Show("Белые победили");
        _connection.Close();
        this.Enabled = false;
    }
    
    private static bool KingCheck(FigureColor clr, bool fl = false)
    {
        Position kingPos = new(0, 0);
        if (clr == FigureColor.White)
        {
            foreach (var i in GlobalVariables.WhiteChessPieces)
            {
                if (i is not King) continue;
                kingPos = new(i.Pos.X, i.Pos.Y);
                break;
            }
            foreach (var i in GlobalVariables.BlackChessPieces)
                foreach (var j in i.NextMove())
                    if (j == kingPos)
                    {
                        if (fl)
                            GlobalVariables.Checker = new(i.Pos.X, i.Pos.Y);
                        return true;
                    }
            return false;
        }
        else
        {
            foreach (var i in GlobalVariables.BlackChessPieces)
            {
                if (i is not King) continue;
                kingPos = new(i.Pos.X, i.Pos.Y);
                break;
            }

            foreach (var i in GlobalVariables.WhiteChessPieces)
            foreach (var j in i.NextMove())
                if (j == kingPos)
                    return true;
            return false;
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