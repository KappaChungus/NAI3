
namespace NAI3
{
    public sealed class Form1 : Form
    {
        public static Form1? Instance { get; set; }
        public Button ChoseTrainDataDirButton { get; }
        public Button ChoseTestFileButton{ get; }
        public Form1()
        {
            Instance = this;
            FlowLayoutPanel panel = new FlowLayoutPanel();
            panel.Dock = DockStyle.Fill;
            Size = new Size(400, 300);
            
            ButtonFactory bf = new ButtonFactory();
            ChoseTrainDataDirButton = bf.GetButton("Choose train data directory.", bf.ChoseTrainDataDirClick);
            ChoseTestFileButton = bf.GetButton("Choose test file.", bf.ChoseTestFileClick);
            ChoseTestFileButton.Enabled = false;
            
            panel.Controls.Add(ChoseTrainDataDirButton);
            panel.Controls.Add(ChoseTestFileButton);
            
            Controls.Add(panel);
            Text = "language guesser";
            StartPosition = FormStartPosition.CenterScreen;
            
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