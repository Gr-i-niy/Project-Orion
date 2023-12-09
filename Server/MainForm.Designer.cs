using Svg;

namespace Server;

partial class MainForm
{
    TableLayoutPanel chessBoard = new();
    private System.ComponentModel.IContainer components = null;
    private void InitializeComponent()
    {
        for (int i = 0; i < 64; i++)
        {
            GlobalVariables.ChessBoardButtonsImages.Add(new SvgDocument());
            GlobalVariables.ChessBoardButtonsBackgroundImages.Add(new SvgDocument());
        }
        
        this.Controls.Add(chessBoard);
        chessBoard.BorderStyle = BorderStyle.FixedSingle;
        for (int row = 0; row < 8; row++)
        {
            chessBoard.RowStyles.Add(new RowStyle(SizeType.Percent, 1));
            chessBoard.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 1));
            for (int col = 0; col < 8; col++)
            {
                Button button = new Button();
                button.FlatStyle = FlatStyle.Flat;
                button.Dock = DockStyle.Fill;
                button.Name = (row * 8 + col).ToString();
                switch (row)
                {
                    case 0:
                        GlobalVariables.BlackChessPieces[col].ChangePosition(col, row);
                        button.Image = GlobalVariables.BlackChessPieces[col].Image.Draw(button.Width, button.Height);
                        GlobalVariables.ChessBoardButtonsImages[row * 8 + col] = 
                            GlobalVariables.BlackChessPieces[col].Image;
                        break;
                    case 1:
                        GlobalVariables.BlackChessPieces.Add(new Pawn(col, row, FigureColor.Black));
                        button.Image = GlobalVariables.BlackChessPieces.Last().Image.Draw(button.Width, button.Height);
                        GlobalVariables.ChessBoardButtonsImages[row * 8 + col] = 
                            GlobalVariables.BlackChessPieces.Last().Image;
                        break;
                    case 6:
                        GlobalVariables.WhiteChessPieces.Add(new Pawn(col, row, FigureColor.White));
                        button.Image = GlobalVariables.WhiteChessPieces.Last().Image.Draw(button.Width, button.Height);
                        GlobalVariables.ChessBoardButtonsImages[row * 8 + col] = 
                            GlobalVariables.WhiteChessPieces.Last().Image;
                        break;
                    case 7:
                        GlobalVariables.WhiteChessPieces[col].ChangePosition(col, row);
                        button.Image = GlobalVariables.WhiteChessPieces[col].Image.Draw(button.Width, button.Height);
                        GlobalVariables.ChessBoardButtonsImages[row * 8 + col] = 
                            GlobalVariables.WhiteChessPieces[col].Image;
                        break;
                }
                button.SizeChanged += (sender, args) =>
                    UpdateButtonImage((Button)button, Int32.Parse(button.Name));
                button.Click += (sender, e) =>
                    OnClickAction((Button)sender, Int32.Parse(button.Name));
                button.Margin = Padding.Empty;
                button.FlatAppearance.BorderSize = 0;
                if ((col + row) % 2 == 1) button.BackColor = Color.DarkOliveGreen;
                else button.BackColor = Color.LightGoldenrodYellow;
                chessBoard.Controls.Add(button, col, row);
                GlobalVariables.ChessBoardButtons.Add(button);
            }
        }

        this.Shown += (sender, args) => StartListening();
        this.components = new System.ComponentModel.Container();
        this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
        this.WindowState = FormWindowState.Maximized;
        this.SizeChanged += Form_SizeChanged;
        this.Text = "Chess";
    }

}