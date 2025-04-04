﻿
namespace NAI3
{
    public sealed class Form1 : Form
    {
        public static Form1? Instance { get; private set; }
        private Button ChoseTrainDataDirButton { get; }
        public Button ChoseTestFileButton{ get; }
        
        public TextBox TextBox { get; set; }

        private Form1()
        {
            Instance = this;
            FlowLayoutPanel panel = new FlowLayoutPanel();
            panel.Dock = DockStyle.Fill;
            Size = new Size(400, 300);
            
            ButtonFactory bf = new ButtonFactory();
            ChoseTrainDataDirButton = bf.GetButton("Choose train data directory.", bf.ChoseTrainDataDirClick);
            ChoseTestFileButton = bf.GetButton("Choose test file.", bf.ChoseTestFileClick);
            TextBox = bf.GetTextBox("enter text to be tested here", bf.TestBoxKeyDown);
            ChoseTestFileButton.Enabled = false;
            TextBox.Enabled = false;
            panel.Controls.Add(ChoseTrainDataDirButton);
            panel.Controls.Add(ChoseTestFileButton);
            panel.Controls.Add(TextBox);
            Controls.Add(panel);
            Text = "language guesser";
            StartPosition = FormStartPosition.CenterScreen;
            ActiveControl = ChoseTrainDataDirButton;
            
        }

        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }
    }
}