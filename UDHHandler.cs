using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace LookSin
{
    class UDHHandler
    {

        /// <summary>
        /// Делегат для события возвращаюего полученные данные из UDP
        /// </summary>
        /// <param name="vs"></param>
        public delegate void UDHHandDelegat(byte[] vs);

        /// <summary>
        /// Делегат для создания события, которое возвращает исключения
        /// </summary>
        public event UDHHandDelegat DataRecive;


        /// <summary>
        /// Событие возвращающее исключение
        /// </summary>
        /// <param name="str"></param>
        public delegate void UDPHandlerExeption(string str);

        /// <summary>
        /// Событие возвращающее принятые данные
        /// </summary>
        public event UDPHandlerExeption Exep;

        /// <summary>
        /// Поток прослушивания порта
        /// </summary>
        public static Thread thread_reader;
       
        
        /// <summary>
        /// Номер порта прослушивания
        /// </summary>
        public UInt16 NumPort { get; private set; }
        

        /// <summary>
        /// ip адрес отправителя
        /// </summary>
        public IPAddress iPAddres { get; private set; }


        private UdpClient udpClient;


        private IPEndPoint RemoteIpEndPoint = null;

        /// <summary>
        /// Конструктор класса
        /// </summary>
        /// <param name="NumPort">Принимает прослушиваемый порт</param>
        /// <param name="iPAddres">ip адрес отправителя</param>
        public UDHHandler(UInt16 NumPort, IPAddress iPAddres) 
        {
            this.iPAddres = iPAddres;
            this.NumPort = NumPort;
            udpClient = new UdpClient(NumPort);
            RemoteIpEndPoint = new IPEndPoint(iPAddres, NumPort);
            thread_reader = new Thread(new ThreadStart(UDPRecive));
        }

        public void UDPRecive() 
        {
            try
            {
                while (true)
                {
                    byte[] receiveBytes = udpClient.Receive(ref RemoteIpEndPoint);
                    DataRecive?.Invoke(receiveBytes);
                }
            }
            catch(Exception e)
            {
                Exep?.Invoke($"Произошло исключение в блоке UDPRecive, вида {e} \r\n" +
                    $"----> Метод приема закрыт, для возобновления необходимо вызвать заново метод UDPRecive();");
                return;
            }
        }



        public void ThreadReaderStart() 
        {
            //TODO: Запуск потока + отлов исключений
            try
            {
                thread_reader.Start();
            }
            catch(Exception e)
            {
                Exep?.Invoke($"Произошло исключение в блоке ThreadReaderStart, вида : {e}");
            }
        }

        public static void ThreadReaderStop() 
        {

            //TODO: Остановка потока.

        }


        public void UDPClose() 
        {
            udpClient.Close();
        }
    }
}
