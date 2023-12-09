using System.ComponentModel;

namespace Client;

partial class GetIPForm
{
    private IContainer components = null;
    protected override void Dispose(bool disposing)
    {
        if (disposing && (components != null))
        {
            components.Dispose();
        }

        base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
        label1 = new Label();
        button1 = new Button();
        textBox1 = new TextBox();
        SuspendLayout();
        // 
        // label1
        // 
        label1.AutoSize = true;
        label1.Font = new("Segoe UI", 13F, FontStyle.Regular, GraphicsUnit.Point);
        label1.Name = "label1";
        label1.AutoSize = false;
        label1.Dock = DockStyle.Fill;
        label1.TextAlign = ContentAlignment.TopCenter;
        label1.TabIndex = 0;
        label1.Text = "Пожалуйста введите ip другого игрока";
        // 
        // button1
        // 
        button1.Location = new Point(400 - this.button1.Width, 264);
        button1.Name = "button1";
        button1.Size = new Size(169, 52);
        button1.TabIndex = 1;
        button1.Text = "Начать игру";
        button1.UseVisualStyleBackColor = true;
        button1.Click += (sender, args) => Buttion1Click();
        // 
        // textBox1
        // 
        textBox1.Location = new Point(200, 112);
        textBox1.Name = "textBox1";
        textBox1.Text = "";
        textBox1.BorderStyle = BorderStyle.None;
        textBox1.Size = new Size(400, 100);
        textBox1.Font = new("Segoe UI", 13F, FontStyle.Regular, GraphicsUnit.Point);
        textBox1.TextAlign = HorizontalAlignment.Center;
        textBox1.TabIndex = 2;
        // 
        // GetIPForm
        // 
        AutoScaleDimensions = new SizeF(15F, 37F);
        AutoScaleMode = AutoScaleMode.Font;
        BackColor = Color.Aquamarine;
        ClientSize = new Size(800, 450);
        this.MinimumSize = this.Size;
        this.MaximumSize = this.Size;
        Controls.Add(textBox1);
        Controls.Add(button1);
        Controls.Add(label1);
        Name = "GetIPForm";
        Text = "GetIPForm";
        this.FormClosing += (sender, args) => GetIpFormClosing(args);
        ResumeLayout(false);
        PerformLayout();
    }

    private Label label1;
    private Button button1;
    private TextBox textBox1;
}