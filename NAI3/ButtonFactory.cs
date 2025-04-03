
namespace NAI3;

using System.Windows.Forms;

public class ButtonFactory
{
    Network? _network;
    public Button GetButton(string name,Action<object, EventArgs, string> clickHandler)
    {
        var btn = new Button();
        btn.Text = name;
        btn.Size = new Size(150, 30);
        btn.Font = new Font("Arial", 10,FontStyle.Regular);
        btn.Click += (sender,e)=>clickHandler(sender!,e,name);
        return btn;
    }
    
    public TextBox GetTextBox(string name, Action<object, KeyEventArgs, string> keyHandler)
    {
        var tb = new TextBox();
        tb.Text = name;
        tb.Size = new Size(305, 160);
        tb.Font = new Font("Arial", 10, FontStyle.Regular);
        tb.KeyDown += (sender, e) => keyHandler(sender!, e, name);
        tb.Enter += (sender, e) => tb.Text = "";
        return tb;
    }
    public void ChoseTrainDataDirClick(object sender, EventArgs e, string name)
    {
        using var folderBrowserDialog = new FolderBrowserDialog();
        folderBrowserDialog.Description = "Select train directory";
        
        if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
        {
            _network = new Network(folderBrowserDialog.SelectedPath);

            _network.TrainPerceptrones();

            Form1.Instance!.ChoseTestFileButton.Enabled = true;
            Form1.Instance!.textBox.Enabled = true;
        }
    }
    
    public void ChoseTestFileClick(object sender, EventArgs e, string name)
    {
        OpenFileDialog openFileDialog = new OpenFileDialog();
        openFileDialog.Title = "Select test data file";
        if (openFileDialog.ShowDialog() == DialogResult.OK)
        {
            DataFilter df = new DataFilter();
            string res = _network.TestPerceptrones(df.FilterData(openFileDialog.FileName));
            MessageBox.Show("Language is: " + res);
        }
    }
    
    
    public void TestBoxKeyDown(object sender, KeyEventArgs e, string name)
    {
        if (e.KeyCode == Keys.Enter)
        {
            string path = "tmp.txt";
            File.WriteAllText(path, ((TextBox)sender).Text);
            ((TextBox)sender).Text = "";
            var df = new DataFilter();
            e.SuppressKeyPress = true;
            string res = _network.TestPerceptrones(df.FilterData(path));;
            MessageBox.Show("Language is: " + res);
        }
    }

}