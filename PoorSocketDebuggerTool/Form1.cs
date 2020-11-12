using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;
using System.Net.Sockets;
using System.Net;
using System.IO;
using System.Configuration;

namespace PoorSocketDebuggerTool
{
    public partial class Form1 : Form
    {
        //TCPクライアント
        System.Net.Sockets.TcpClient m_tcp;
        //TCPサーバ
        public static ManualResetEvent allDone = new ManualResetEvent(false);
        static StateObject state;
        //UDP
        System.Net.Sockets.UdpClient m_udp;

        System.Net.Sockets.NetworkStream m_ns;
        Socket localSocket;

        Thread m_thread;
        bool m_ActiveRecv = true;

        delegate void ListAddDelegate();
        delegate void ListAddDelegateUdpRecv(byte[] msg);

        public Form1()
        {
            InitializeComponent();
            buttonSend.Enabled = false;
            buttonDisconnect.Enabled = false;
            comboBoxType.SelectedIndex = 0;
            //設定ファイル読み込み
            comboBoxType.SelectedIndex = Int32.Parse(System.Configuration.ConfigurationManager.AppSettings["protocol"]);
            textBoxSrcIp.Text = System.Configuration.ConfigurationManager.AppSettings["srcip"];
            textBoxSrcPort.Text = System.Configuration.ConfigurationManager.AppSettings["srcport"];
            textBoxDstIp.Text = System.Configuration.ConfigurationManager.AppSettings["dstip"];
            textBoxDstPort.Text = System.Configuration.ConfigurationManager.AppSettings["dstport"];
            textBoxData.Text = System.Configuration.ConfigurationManager.AppSettings["senddatapath"];
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        //接続ボタン
        void ClientMode()
        {
            m_ActiveRecv = true;
            timerReconnect.Enabled = false;

            string strSrcIP = textBoxSrcIp.Text;
            int sSrcPort = Int32.Parse(textBoxSrcPort.Text);
            string strDstIP = textBoxDstIp.Text;
            int sDstPort = Int32.Parse(textBoxDstPort.Text);

            try
            {
                localSocket = new Socket(SocketType.Stream, ProtocolType.Tcp);
                localSocket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, 1);
                var local = new System.Net.IPEndPoint(IPAddress.Parse(strSrcIP), sSrcPort);
                localSocket.Bind(local);

                //TcpClientを作成し、サーバーと接続する
                m_tcp = new System.Net.Sockets.TcpClient();
                m_tcp.Client = localSocket;
                m_tcp.Connect(strDstIP, sDstPort);
                listBox1.Items.Add("接続成功");
                listBox1.SelectedIndex = listBox1.Items.Count - 1;
                listBox1.TopIndex = listBox1.SelectedIndex;

                //NetworkStreamを取得する
                m_ns = m_tcp.GetStream();
                //読み取り、書き込みのタイムアウトを10秒にする
                m_ns.ReadTimeout = 1000;
                m_ns.WriteTimeout = 1000;

                m_thread = new Thread(new ThreadStart(() =>
                {
                    System.IO.MemoryStream ms = new System.IO.MemoryStream();
                    byte[] resBytes = new byte[65536];
                    int resSize = 0;
                    do
                    {
                        try
                        {
                            //データの一部を受信する
                            resSize = m_ns.Read(resBytes, 0, resBytes.Length);
                            //Readが0を返した時はサーバーが切断したと判断
                            if (resSize != 0)
                            {
                                //受信したデータを蓄積する
                                ms.Write(resBytes, 0, resSize);
                                //まだ読み取れるデータがあるか、データの最後が\nでない時は、
                                // 受信を続ける
                                Invoke(new ListAddDelegate(OutputLogRecv));
                            }
                            else
                            {
                                //切断
                                break;
                            }

                        }
                        catch (ArgumentNullException e1)
                        {
                        }
                        catch (ArgumentOutOfRangeException e2)
                        {
                        }
                        catch (InvalidOperationException e3)
                        {
                            //切断
                            break;
                        }
                        catch (IOException e4)
                        {
                        }
                    } while (m_ActiveRecv);
                    //受信したデータを文字列に変換
                    ms.Close();
                    m_ns.Close();
                    m_ns = null;
                    m_tcp.Close();
                    m_tcp = null;
                    localSocket.Close();
                    localSocket = null;
                    try
                    {
                        Invoke(new ListAddDelegate(ErrDiscconect));
                    }
                    catch (Exception)
                    {
                    }
                }));

                m_thread.Start();

                buttonConnect.Enabled = false;
                buttonSend.Enabled = true;
                buttonDisconnect.Enabled = true;

                //連続送信する場合タイマきどう
                if (checkBoxReSend.Checked == true)
                {
                    timerResend.Interval = Int32.Parse(textBox1.Text);
                    timerResend.Enabled = true;
                }
            }
            catch (Exception)
            {
                listBox1.Items.Add("接続失敗");
                listBox1.SelectedIndex = listBox1.Items.Count - 1;
                listBox1.TopIndex = listBox1.SelectedIndex;
                if (m_ns != null)
                {
                    m_ns.Close();
                }
                if (m_tcp != null)
                {
                    m_tcp.Close();
                }
                if (localSocket != null)
                {
                    localSocket.Close();
                }
            }
            //再接続時間が設定されている場合、再接続
            if (checkBoxReconnect.Checked)
            {
                timerReconnect.Interval = Int32.Parse(textBoxReconnectTime.Text);
                timerReconnect.Enabled = true;

                buttonConnect.Enabled = false;
                buttonDisconnect.Enabled = true;
            }

        }

        void ServerMode()
        {
            m_ActiveRecv = true;
            timerReconnect.Enabled = false;

            string strSrcIP = textBoxSrcIp.Text;
            int sSrcPort = Int32.Parse(textBoxSrcPort.Text);
            string strDstIP = textBoxDstIp.Text;
            int sDstPort = Int32.Parse(textBoxDstPort.Text);

            try
            {
                localSocket = new Socket(SocketType.Stream, ProtocolType.Tcp);
                localSocket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, 1);
                var local = new System.Net.IPEndPoint(IPAddress.Parse(strSrcIP), sSrcPort);
                localSocket.Bind(local);
                localSocket.Listen(100);

                m_thread = new Thread(new ThreadStart(() =>
                {
                    while (m_ActiveRecv == true)
                    {
                        // Set the event to nonsignaled state.  
                        allDone.Reset();

                        localSocket.BeginAccept(
                            new AsyncCallback(AcceptCallback),
                            localSocket);

                        try
                        {
                            Invoke(new ListAddDelegate(SendStart));
                        }
                        catch (Exception)
                        {
                        }

                        // Wait until a connection is made before continuing.  
                        allDone.WaitOne(1000);
                    }
                    if (m_ns != null)
                    {
                        m_ns.Close();
                    }
                    if (localSocket != null)
                    {
                        localSocket.Close();
                    }
                    if (state != null)
                    {
                        if (state.workSocket != null)
                        {
                            state.workSocket.Close();
                        }
                    }
                }));

                m_thread.Start();
                listBox1.Items.Add("Accept開始");

                buttonConnect.Enabled = false;
                buttonDisconnect.Enabled = true;
            }
            catch (Exception)
            {
                listBox1.Items.Add("接続失敗");
                listBox1.SelectedIndex = listBox1.Items.Count - 1;
                listBox1.TopIndex = listBox1.SelectedIndex;
                if (m_ns != null)
                {
                    m_ns.Close();
                }
                if (localSocket != null)
                {
                    localSocket.Close();
                }
                if(state != null)
                {
                    if (state.workSocket != null)
                    {
                        state.workSocket.Shutdown(SocketShutdown.Both);
                        state.workSocket.Close();
                    }
                }
            }
        }
        // State object for reading client data asynchronously  
        public class StateObject
        {
            // Client  socket.  
            public Socket workSocket = null;
            // Size of receive buffer.  
            public const int BufferSize = 1024;
            // Receive buffer.  
            public byte[] buffer = new byte[BufferSize];
            // Received data string.  
            public StringBuilder sb = new StringBuilder();
        }
        public static void AcceptCallback(IAsyncResult ar)
        {
            try
            {
                // Signal the main thread to continue.  
                allDone.Set();

                // Get the socket that handles the client request.  
                Socket listener = (Socket)ar.AsyncState;
                Socket handler = listener.EndAccept(ar);
                handler.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, 1);

                // Create the state object.  
                state = new StateObject();
                state.workSocket = handler;
                handler.BeginReceive(state.buffer, 0, StateObject.BufferSize, 0,
                    new AsyncCallback(ReadCallback), state);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }
        public static void ReadCallback(IAsyncResult ar)
        {
            try
            {

                String content = String.Empty;

                // Retrieve the state object and the handler socket  
                // from the asynchronous state object.  
                StateObject state = (StateObject)ar.AsyncState;
                Socket handler = state.workSocket;

                // Read data from the client socket.
                int bytesRead = handler.EndReceive(ar);

                if (bytesRead > 0)
                {
                    // There  might be more data, so store the data received so far.  
                    state.sb.Append(Encoding.ASCII.GetString(
                        state.buffer, 0, bytesRead));

                    // Check for end-of-file tag. If it is not there, read
                    // more data.  
                    content = state.sb.ToString();
                    if (content.IndexOf("<EOF>") > -1)
                    {
                        // All the data has been read from the
                        // client. Display it on the console.  
                        Console.WriteLine("Read {0} bytes from socket. \n Data : {1}",
                            content.Length, content);
                        // Echo the data back to the client.  
                    }
                    else
                    {
                        // Not all data received. Get more.  
                        handler.BeginReceive(state.buffer, 0, StateObject.BufferSize, 0,
                        new AsyncCallback(ReadCallback), state);
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }
        private static void TcpServerSend(Socket handler, byte[] byteData)
        {
            // Begin sending the data to the remote device.  
            handler.BeginSend(byteData, 0, byteData.Length, 0,
                new AsyncCallback(SendCallback), handler);
        }

        private static void SendCallback(IAsyncResult ar)
        {
            try
            {
                // Retrieve the socket from the state object.  
                Socket handler = (Socket)ar.AsyncState;

                // Complete sending the data to the remote device.  
                int bytesSent = handler.EndSend(ar);

            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }
        void UdpMode()
        {
            m_ActiveRecv = true;
            timerReconnect.Enabled = false;

            string strSrcIP = textBoxSrcIp.Text;
            int sSrcPort = Int32.Parse(textBoxSrcPort.Text);
            string strDstIP = textBoxDstIp.Text;
            int sDstPort = Int32.Parse(textBoxDstPort.Text);

            try
            {
                localSocket = new Socket(SocketType.Dgram, ProtocolType.Udp);
                localSocket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, 1);
                var local = new System.Net.IPEndPoint(IPAddress.Parse(strSrcIP), sSrcPort);
                localSocket.Bind(local);

                //TcpClientを作成し、サーバーと接続する
                m_udp = new System.Net.Sockets.UdpClient();
                m_udp.Client = localSocket;
                listBox1.Items.Add("接続成功");
                listBox1.SelectedIndex = listBox1.Items.Count - 1;
                listBox1.TopIndex = listBox1.SelectedIndex;

                m_thread = new Thread(new ThreadStart(() =>
                {
                    do
                    {
                        try
                        {
                            //データを受信する
                            System.Net.IPEndPoint remoteEP = null;
                            byte[] rcvBytes = m_udp.Receive(ref remoteEP);
                            Invoke(new ListAddDelegateUdpRecv(OutputLogRecvUdp),rcvBytes);
                        }
                        catch (Exception)
                        {
                        }
                    } while (m_ActiveRecv);
                    //受信したデータを文字列に変換
                    m_udp.Close();
                    m_udp = null;
                    localSocket.Close();
                    localSocket = null;
                    try
                    {
                        Invoke(new ListAddDelegate(ErrDiscconect));
                    }
                    catch (Exception)
                    {
                    }
                }));

                m_thread.Start();

                buttonConnect.Enabled = false;
                buttonSend.Enabled = true;
                buttonDisconnect.Enabled = true;

                //連続送信する場合タイマきどう
                if (checkBoxReSend.Checked == true)
                {
                    timerResend.Interval = Int32.Parse(textBox1.Text);
                    timerResend.Enabled = true;
                }
            }
            catch (Exception)
            {
                listBox1.Items.Add("接続失敗");
                listBox1.SelectedIndex = listBox1.Items.Count - 1;
                listBox1.TopIndex = listBox1.SelectedIndex;
                if (m_udp != null)
                {
                    m_udp.Close();
                }
                if (localSocket != null)
                {
                    localSocket.Close();
                }
            }
            //再接続時間が設定されている場合、再接続
            if (checkBoxReconnect.Checked)
            {
                timerReconnect.Interval = Int32.Parse(textBoxReconnectTime.Text);
                timerReconnect.Enabled = true;

                buttonConnect.Enabled = false;
                buttonDisconnect.Enabled = true;
            }

        }
        private void button1_Click(object sender, EventArgs e)
        {
            if(comboBoxType.SelectedIndex == 0)
            {
                ClientMode();
            }
            else if(comboBoxType.SelectedIndex == 1)
            {
                ServerMode();
            }
            else if(comboBoxType.SelectedIndex == 2)
            {
                UdpMode();
            }
        }

        private void OutputLogRecv()
        {
//            listBox1.Items.Add("受信");
        }

        private void OutputLogRecvUdp(byte[] msg)
        {
            string text = System.Text.Encoding.UTF8.GetString(msg);
            listBox1.Items.Add(text);
        }

        private void SendStart()
        {
            buttonSend.Enabled = true;
            //連続送信する場合タイマきどう
            if (checkBoxReSend.Checked == true)
            {
                timerResend.Interval = Int32.Parse(textBox1.Text);
                timerResend.Enabled = true;
            }
        }

        private void ErrDiscconect()
        {
            listBox1.Items.Add("切断");
            listBox1.SelectedIndex = listBox1.Items.Count - 1;
            listBox1.TopIndex = listBox1.SelectedIndex;
            if (checkBoxReconnect.Checked)
            {
                //再接続時間が設定されている場合、再接続するので、接続ボタン押下維持
                m_ActiveRecv = false;
                buttonSend.Enabled = false;
                timerResend.Enabled = false;
            }
            else
            {
                Disconnect();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                string[] sendfiles = textBoxData.Text.ToString().Split(',');

                foreach(string filename in sendfiles)
                {
                    FileStream fs = new FileStream(filename, FileMode.Open, FileAccess.Read);

                    int fileSize = (int)fs.Length; // ファイルのサイズ
                    byte[] buf = new byte[fileSize]; // データ格納用配列

                    int readSize; // Readメソッドで読み込んだバイト数
                    int remain = fileSize; // 読み込むべき残りのバイト数
                    int bufPos = 0; // データ格納用配列内の追加位置

                    while (remain > 0)
                    {
                        // 1024Bytesずつ読み込む
                        readSize = fs.Read(buf, bufPos, Math.Min(1024, remain));

                        bufPos += readSize;
                        remain -= readSize;
                    }
                    fs.Dispose();
                    if (comboBoxType.SelectedIndex == 0)
                    {
                        m_ns.Write(buf, 0, fileSize);
                    }
                    else if (comboBoxType.SelectedIndex == 1)
                    {
                        if (state != null)
                        {
                            if (state.workSocket != null)
                            {

                                TcpServerSend(state.workSocket, buf);
                            }
                        }
                    }
                    else if (comboBoxType.SelectedIndex == 2)
                    {
                        if (m_udp != null)
                        {
                            UdpSend(buf);
                        }
                    }
                }

                    //                listBox1.Items.Add("送信成功");
            }
            catch (Exception)
            {
//                listBox1.Items.Add("送信失敗");
            }
        }
        private void UdpSend(byte[] byteData)
        {
            string strDstIP = textBoxDstIp.Text;
            int sDstPort = Int32.Parse(textBoxDstPort.Text);

            //リモートホストを指定してデータを送信する
            m_udp.Send(byteData, byteData.Length, strDstIP, sDstPort);
        }

        void Disconnect()
        {
            m_ActiveRecv = false;
            if (m_udp != null)
            {
                m_udp.Close();
            }

            if (state != null)
            {
                if(state.workSocket != null)
                {
                    state.workSocket.Close();
                }
            }
            buttonConnect.Enabled = true;
            buttonSend.Enabled = false;
            buttonDisconnect.Enabled = false;

            timerResend.Enabled = false;
        }
        private void button3_Click(object sender, EventArgs e)
        {
            timerReconnect.Enabled = false;
            Disconnect();
            listBox1.Items.Add("切断");
            listBox1.SelectedIndex = listBox1.Items.Count - 1;
            listBox1.TopIndex = listBox1.SelectedIndex;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            //OpenFileDialogクラスのインスタンスを作成
            OpenFileDialog ofd = new OpenFileDialog();

            //タイトルを設定する
            ofd.Title = "開くファイルを選択してください";
            ofd.Multiselect = true;

            //ダイアログを表示する
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                //OKボタンがクリックされたとき、選択されたファイル名を表示する。
                //複数選択の場合はコンマ区切り
                textBoxData.Text = "";
                int cnt = 0;
                foreach (string element in ofd.FileNames)
                {
                    if(cnt == 0)
                    {
                        textBoxData.Text = element;
                    }
                    else
                    {
                        textBoxData.Text += ",";
                        textBoxData.Text += element;
                    }
                    cnt++;
                }

            }
        }

        private void timerReconnect_Tick(object sender, EventArgs e)
        {
            if (m_thread == null)
            {
                //接続
                button1_Click(sender, e);
            }
            else
            {
                if (m_thread.IsAlive == false)
                {
                    //接続
                    button1_Click(sender, e);
                }
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (m_thread != null)
            {
                if (m_thread.IsAlive == true)
                {
                    //送信
                    button2_Click(sender, e);
                }
            }
        }

        private void textBoxReSendTime_TextChanged(object sender, EventArgs e)
        {

        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            button3_Click(sender, e);
        }
    }
}
