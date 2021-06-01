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
using ZedGraph;

namespace LookSin
{
    public partial class Form1 : Form
    {
        private static byte[] ipAdd = {192,168,0,25};
        private static UDHHandler UDHHandler = new UDHHandler(1500, new System.Net.IPAddress(ipAdd));
        private bool FlagStart_Or_Stop = false;
        private static DrawGraph drawGraph;
        Thread thread = new Thread(dr);

        public Form1()
        {
            UDHHandler.DataRecive += UDHHandler_DataRecive;
            UDHHandler.Exep += UDHHandler_Exep;
            
            InitializeComponent();
            drawGraph = new DrawGraph(zedGraphControl1, 100);
            thread.Start();
        }

        public static void dr()
        {
            while (true) 
            {
                drawGraph.Do_Dinamik_DrawGraph();
                Thread.Sleep(200);
            }
        }

        private void UDHHandler_Exep(string str)
        {
            MessageBox.Show($"{str}");
        }

        private void UDHHandler_DataRecive(byte[] vs)
        {
            MessageBox.Show(Convert.ToString(vs));
        }

        private void StartStop_Click(object sender, EventArgs e)
        {
            if (FlagStart_Or_Stop == false)
            {
                FlagStart_Or_Stop = true;
                StartStop.Text = "Стоп";
                try 
                {
                    UDHHandler.ThreadReaderStart();
                    MessageBox.Show("Прием данных запущен");
                }
                catch(ThreadInterruptedException ex)
                {
                    MessageBox.Show($"Произошло исключение типа: {ex.ToString()}");
                }
            }
            else if (FlagStart_Or_Stop == true)
            {
                FlagStart_Or_Stop = false;
                StartStop.Text = "Старт";
                try
                {
                    UDHHandler.ThreadReaderStop();
                    MessageBox.Show("Прием данных остановлен");
                }
                catch (Exception ex) { };
            }
        }




    }
}
