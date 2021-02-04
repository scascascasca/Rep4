using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

// Local'deki ekleme 2

//Git değişikliği 1
namespace MultiThreadLock
{
    public partial class Form1 : Form
    {
        BankAccount acc1;
        public Form1()
        {
            InitializeComponent();


            Thread[] threads = new Thread[15];
            acc1 = new BankAccount(10);

            Thread.CurrentThread.Name = "Main Thread";
            Console.WriteLine("---------------------------------------------------------------------");

            for (int i = 0; i < 15; i++)
            {
                //Burada  acc1.WithdrawProcess fonksiyonunun Thread tanımlaması sırasında direkt çağırılamayıp onun yerine acc1.DoWithdraw fonk. ile
                // çağırılma sebebi şu : Thread'ler defaultta void olan ve parametre almayan metorlara assign edilebiliyorlar. Eğer parametre ve return değeri olan
                //bir fonk.'a thread assign etmek istersen başka yöntem kullan.
                Thread thrd = new Thread(new ThreadStart(acc1.DoWithdraw));
                thrd.Name = "TH" + i.ToString();
                threads[i] = thrd;
            }

            for (int i = 0; i < 15; i++)
            {
                Console.WriteLine("{0} State : {1}", threads[i].Name, threads[i].IsAlive);
                threads[i].Start();
                Console.WriteLine("{0} State : {1}", threads[i].Name, threads[i].IsAlive);
            }

        }
    }

}

public class BankAccount
{
    private Object acctLock = new object();
    double Balance { get; set; }
    public BankAccount(double bal)
    {
        Balance = bal;
    }
    public double WithdrawProcess(double amt)
    {
        if (Balance == 0)
        {
            Console.WriteLine("Sorry, Balance = 0");
            return 0;
        }
        lock (acctLock)
        {
            Console.WriteLine("Amount {0} is removed from the Balance: {1} ", amt, Balance);
            Balance = Balance - amt;
            return Balance;
        }
    }

    public void DoWithdraw()
    {
        WithdrawProcess(1);
    }
}
