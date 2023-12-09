using System.ComponentModel;

namespace Server;

partial class GetIpForm
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
        label1.Text = "Ожидание игрока";
        // 
        // GetIPForm
        // 
        AutoScaleDimensions = new SizeF(15F, 37F);
        AutoScaleMode = AutoScaleMode.Font;
        BackColor = Color.Aquamarine;
        ClientSize = new Size(800, 450);
        this.MinimumSize = this.Size;
        this.MaximumSize = this.Size;
        Controls.Add(label1);
        this.Shown += (sender, args) => StartServer();
        Name = "GetIpForm";
        Text = "GetIPForm";
        this.FormClosing += (sender, args) => GetIpFormClosing(args);
        ResumeLayout(false);
        PerformLayout();
    }

    private Label label1;
}