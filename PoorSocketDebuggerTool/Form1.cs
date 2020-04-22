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

namespace PoorSocketDebuggerTool
{
    public partial class Form1 : Form
    {
        System.Net.Sockets.TcpClient m_tcp;
        System.Net.Sockets.NetworkStream m_ns;
        Socket localSocket;

        Thread m_thread;
        bool m_ActiveRecv = true;

        delegate void ListAddDelegate();

        public Form1()
        {
            InitializeComponent();
            buttonSend.Enabled = false;
            buttonDisconnect.Enabled = false;
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
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
                if(checkBoxReSend.Checked == true)
                {
                    timerResend.Interval = Int32.Parse(textBox1.Text);
                    timerResend.Enabled = true;
                }
            }
            catch (Exception)
            {
                listBox1.Items.Add("接続失敗");
                if(m_ns != null)
                {
                    m_ns.Close();
                }
                if (m_tcp != null)
                {
                    m_tcp.Close();
                }
                if(localSocket != null)
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

        private void OutputLogRecv()
        {
            listBox1.Items.Add("受信");
        }
        private void ErrDiscconect()
        {
            listBox1.Items.Add("切断");
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

                FileStream fs = new FileStream(textBoxData.Text, FileMode.Open, FileAccess.Read);

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
                m_ns.Write(buf, 0, fileSize);
                listBox1.Items.Add("送信成功");
            }
            catch (Exception)
            {
                listBox1.Items.Add("送信失敗");
            }
        }

        void Disconnect()
        {
            m_ActiveRecv = false;

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
        }

        private void button4_Click(object sender, EventArgs e)
        {
            //OpenFileDialogクラスのインスタンスを作成
            OpenFileDialog ofd = new OpenFileDialog();

            //タイトルを設定する
            ofd.Title = "開くファイルを選択してください";
            //ダイアログを表示する
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                //OKボタンがクリックされたとき、選択されたファイル名を表示する
                textBoxData.Text = ofd.FileName;
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
            button2_Click(sender, e);
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
