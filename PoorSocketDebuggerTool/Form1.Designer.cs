namespace PoorSocketDebuggerTool
{
    partial class Form1
    {
        /// <summary>
        /// 必要なデザイナー変数です。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 使用中のリソースをすべてクリーンアップします。
        /// </summary>
        /// <param name="disposing">マネージド リソースを破棄する場合は true を指定し、その他の場合は false を指定します。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows フォーム デザイナーで生成されたコード

        /// <summary>
        /// デザイナー サポートに必要なメソッドです。このメソッドの内容を
        /// コード エディターで変更しないでください。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.buttonConnect = new System.Windows.Forms.Button();
            this.buttonSend = new System.Windows.Forms.Button();
            this.buttonDisconnect = new System.Windows.Forms.Button();
            this.textBoxSrcIp = new System.Windows.Forms.TextBox();
            this.textBoxSrcPort = new System.Windows.Forms.TextBox();
            this.textBoxDstIp = new System.Windows.Forms.TextBox();
            this.textBoxDstPort = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.comboBoxType = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.textBoxData = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.button4 = new System.Windows.Forms.Button();
            this.listBox1 = new System.Windows.Forms.ListBox();
            this.checkBoxReconnect = new System.Windows.Forms.CheckBox();
            this.label7 = new System.Windows.Forms.Label();
            this.textBoxReconnectTime = new System.Windows.Forms.TextBox();
            this.checkBoxReSend = new System.Windows.Forms.CheckBox();
            this.label8 = new System.Windows.Forms.Label();
            this.timerReconnect = new System.Windows.Forms.Timer(this.components);
            this.timerResend = new System.Windows.Forms.Timer(this.components);
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // buttonConnect
            // 
            this.buttonConnect.Location = new System.Drawing.Point(292, 12);
            this.buttonConnect.Name = "buttonConnect";
            this.buttonConnect.Size = new System.Drawing.Size(143, 49);
            this.buttonConnect.TabIndex = 0;
            this.buttonConnect.Text = "接続";
            this.buttonConnect.UseVisualStyleBackColor = true;
            this.buttonConnect.Click += new System.EventHandler(this.button1_Click);
            // 
            // buttonSend
            // 
            this.buttonSend.Location = new System.Drawing.Point(726, 53);
            this.buttonSend.Name = "buttonSend";
            this.buttonSend.Size = new System.Drawing.Size(141, 49);
            this.buttonSend.TabIndex = 1;
            this.buttonSend.Text = "送信";
            this.buttonSend.UseVisualStyleBackColor = true;
            this.buttonSend.Click += new System.EventHandler(this.button2_Click);
            // 
            // buttonDisconnect
            // 
            this.buttonDisconnect.Location = new System.Drawing.Point(292, 72);
            this.buttonDisconnect.Name = "buttonDisconnect";
            this.buttonDisconnect.Size = new System.Drawing.Size(143, 48);
            this.buttonDisconnect.TabIndex = 2;
            this.buttonDisconnect.Text = "切断";
            this.buttonDisconnect.UseVisualStyleBackColor = true;
            this.buttonDisconnect.Click += new System.EventHandler(this.button3_Click);
            // 
            // textBoxSrcIp
            // 
            this.textBoxSrcIp.Location = new System.Drawing.Point(124, 41);
            this.textBoxSrcIp.Name = "textBoxSrcIp";
            this.textBoxSrcIp.Size = new System.Drawing.Size(141, 22);
            this.textBoxSrcIp.TabIndex = 3;
            this.textBoxSrcIp.Text = "192.168.1.1";
            // 
            // textBoxSrcPort
            // 
            this.textBoxSrcPort.Location = new System.Drawing.Point(124, 69);
            this.textBoxSrcPort.Name = "textBoxSrcPort";
            this.textBoxSrcPort.Size = new System.Drawing.Size(141, 22);
            this.textBoxSrcPort.TabIndex = 4;
            this.textBoxSrcPort.Text = "50000";
            // 
            // textBoxDstIp
            // 
            this.textBoxDstIp.Location = new System.Drawing.Point(124, 97);
            this.textBoxDstIp.Name = "textBoxDstIp";
            this.textBoxDstIp.Size = new System.Drawing.Size(141, 22);
            this.textBoxDstIp.TabIndex = 5;
            this.textBoxDstIp.Text = "192.168.1.2";
            // 
            // textBoxDstPort
            // 
            this.textBoxDstPort.Location = new System.Drawing.Point(124, 125);
            this.textBoxDstPort.Name = "textBoxDstPort";
            this.textBoxDstPort.Size = new System.Drawing.Size(141, 22);
            this.textBoxDstPort.TabIndex = 6;
            this.textBoxDstPort.Text = "60000";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 43);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(65, 15);
            this.label1.TabIndex = 7;
            this.label1.Text = "送信元IP";
            this.label1.Click += new System.EventHandler(this.label1_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 72);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(79, 15);
            this.label2.TabIndex = 8;
            this.label2.Text = "送信元Port";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 132);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(79, 15);
            this.label3.TabIndex = 9;
            this.label3.Text = "送信先Port";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(12, 102);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(65, 15);
            this.label4.TabIndex = 10;
            this.label4.Text = "送信先IP";
            // 
            // comboBoxType
            // 
            this.comboBoxType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxType.FormattingEnabled = true;
            this.comboBoxType.Items.AddRange(new object[] {
            "TCPクライアント",
            "TCPサーバ",
            "UDP"});
            this.comboBoxType.Location = new System.Drawing.Point(124, 13);
            this.comboBoxType.Name = "comboBoxType";
            this.comboBoxType.Size = new System.Drawing.Size(141, 23);
            this.comboBoxType.TabIndex = 11;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(12, 21);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(37, 15);
            this.label5.TabIndex = 12;
            this.label5.Text = "種別";
            // 
            // textBoxData
            // 
            this.textBoxData.Location = new System.Drawing.Point(457, 22);
            this.textBoxData.Name = "textBoxData";
            this.textBoxData.Size = new System.Drawing.Size(348, 22);
            this.textBoxData.TabIndex = 13;
            this.textBoxData.Text = "c:\\test.bin";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(454, 4);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(193, 15);
            this.label6.TabIndex = 14;
            this.label6.Text = "送信データのバイナリファイルパス";
            // 
            // button4
            // 
            this.button4.Location = new System.Drawing.Point(803, 22);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(64, 22);
            this.button4.TabIndex = 15;
            this.button4.Text = "...";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // listBox1
            // 
            this.listBox1.FormattingEnabled = true;
            this.listBox1.ItemHeight = 15;
            this.listBox1.Location = new System.Drawing.Point(15, 228);
            this.listBox1.Name = "listBox1";
            this.listBox1.Size = new System.Drawing.Size(994, 274);
            this.listBox1.TabIndex = 16;
            // 
            // checkBoxReconnect
            // 
            this.checkBoxReconnect.AutoSize = true;
            this.checkBoxReconnect.Location = new System.Drawing.Point(15, 165);
            this.checkBoxReconnect.Name = "checkBoxReconnect";
            this.checkBoxReconnect.Size = new System.Drawing.Size(104, 19);
            this.checkBoxReconnect.TabIndex = 17;
            this.checkBoxReconnect.Text = "再接続有無";
            this.checkBoxReconnect.UseVisualStyleBackColor = true;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(12, 197);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(110, 15);
            this.label7.TabIndex = 18;
            this.label7.Text = "再接続時間(ms)";
            // 
            // textBoxReconnectTime
            // 
            this.textBoxReconnectTime.Location = new System.Drawing.Point(128, 194);
            this.textBoxReconnectTime.Name = "textBoxReconnectTime";
            this.textBoxReconnectTime.Size = new System.Drawing.Size(141, 22);
            this.textBoxReconnectTime.TabIndex = 19;
            this.textBoxReconnectTime.Text = "1000";
            // 
            // checkBoxReSend
            // 
            this.checkBoxReSend.AutoSize = true;
            this.checkBoxReSend.Location = new System.Drawing.Point(292, 165);
            this.checkBoxReSend.Name = "checkBoxReSend";
            this.checkBoxReSend.Size = new System.Drawing.Size(119, 19);
            this.checkBoxReSend.TabIndex = 20;
            this.checkBoxReSend.Text = "連続送信有無";
            this.checkBoxReSend.UseVisualStyleBackColor = true;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(289, 197);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(110, 15);
            this.label8.TabIndex = 21;
            this.label8.Text = "再送信時間(ms)";
            // 
            // timerReconnect
            // 
            this.timerReconnect.Tick += new System.EventHandler(this.timerReconnect_Tick);
            // 
            // timerResend
            // 
            this.timerResend.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(405, 194);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(121, 22);
            this.textBox1.TabIndex = 24;
            this.textBox1.Text = "1000";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1021, 521);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.checkBoxReSend);
            this.Controls.Add(this.textBoxReconnectTime);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.checkBoxReconnect);
            this.Controls.Add(this.listBox1);
            this.Controls.Add(this.button4);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.textBoxData);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.comboBoxType);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.textBoxDstPort);
            this.Controls.Add(this.textBoxDstIp);
            this.Controls.Add(this.textBoxSrcPort);
            this.Controls.Add(this.textBoxSrcIp);
            this.Controls.Add(this.buttonDisconnect);
            this.Controls.Add(this.buttonSend);
            this.Controls.Add(this.buttonConnect);
            this.Name = "Form1";
            this.Text = "Form1";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Form1_FormClosed);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button buttonConnect;
        private System.Windows.Forms.Button buttonSend;
        private System.Windows.Forms.Button buttonDisconnect;
        private System.Windows.Forms.TextBox textBoxSrcIp;
        private System.Windows.Forms.TextBox textBoxSrcPort;
        private System.Windows.Forms.TextBox textBoxDstIp;
        private System.Windows.Forms.TextBox textBoxDstPort;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox comboBoxType;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox textBoxData;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.ListBox listBox1;
        private System.Windows.Forms.CheckBox checkBoxReconnect;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox textBoxReconnectTime;
        private System.Windows.Forms.CheckBox checkBoxReSend;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Timer timerReconnect;
        private System.Windows.Forms.Timer timerResend;
        private System.Windows.Forms.TextBox textBox1;
    }
}

