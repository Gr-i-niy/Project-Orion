using System.Diagnostics;

namespace Client;

public partial class GetIPForm : Form
{
    public GetIPForm()
    {
        InitializeComponent();
    }

    private void Buttion1Click()
    {
        var ipToCheck = this.textBox1.Text;
        if (!ipToCheck.Contains('.'))
        {
            MessageBox.Show("IP адрес введён неправильно");
            return;
        }

        List<string> a = new(ipToCheck.Split("."));
        if (a.Count != 4)
        {
            MessageBox.Show("IP адрес введён неправильно");
            return;
        }
        foreach (var i in a)
        {
            try
            {
                var x = int.Parse(i);
                if (x is <= 255 and >= 0) continue;
                MessageBox.Show("IP адрес введён неправильно");
                return;
            }
            catch (Exception e)
            {
                MessageBox.Show("IP адрес введён неправильно");
                return;
            }
        }

        if (CheckIp(ipToCheck))
        {
            GlobalVariables.UserIp = ipToCheck;
            this.Close();
        }
        else
        {
            MessageBox.Show("Не удалось установить соединение");
        }
    }

    private void GetIpFormClosing(FormClosingEventArgs e)
    {
        if (GlobalVariables.UserIp == "") Process.GetCurrentProcess().Kill();
    }

    private bool CheckIp(string ip)
    {
        TcpCommunication.Client test = new(ip, 8888);
        test.Connect();
        var t = test.ReceiveMessage() == "777";
        test.Close();
        return t;
    }
    
}