using System.Diagnostics;

namespace Server;

public partial class GetIpForm : Form
{
    public GetIpForm()
    {
        InitializeComponent();
    }

    private void GetIpFormClosing(FormClosingEventArgs e)
    {
        if (GlobalVariables.UserIp == "") Process.GetCurrentProcess().Kill();
    }

    private void StartServer()
    {
        Refresh();
        TcpCommunication.Server test = new(8888);
        test.Start();
        test.SendMessage("777");
        test.Stop();
        GlobalVariables.UserIp = "111";
        this.Close();
    }
}