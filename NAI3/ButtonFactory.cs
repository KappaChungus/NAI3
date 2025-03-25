
namespace NAI3;

using System.Windows.Forms;

public class ButtonFactory
{
    private List<Perceptron>? _perceptrons;
    private const double Alpha= 0.5;
    private const double Theta = 1;
    private const int AlphabetSize = 26;

    private Dictionary<string, int>? _languageToInt;
    private List<string>? _languageFromInt;

    private List<KeyValuePair<List<double>,int>>? _trainData;
    private int generation = 0;
    public Button GetButton(string name,Action<object, EventArgs, string> clickHandler)
    {
        Button btn = new Button();
        btn.Text = name;
        btn.Size = new Size(150, 30);
        btn.Font = new Font("Arial", 10,FontStyle.Regular);
        btn.Click += (sender,e)=>clickHandler(sender!,e,name);
        return btn;
    }

    public void ChoseTrainDataDirClick(object sender, EventArgs e, string name)
    {
        using var folderBrowserDialog = new FolderBrowserDialog();
        folderBrowserDialog.Description = "Select train directory";
        
        if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
        {
            _trainData = new List<KeyValuePair<List<double>, int>>();
            _languageToInt = new Dictionary<string, int>();
            _languageFromInt= new List<string>();
            string trainDataDir = folderBrowserDialog.SelectedPath;
            if(generation == 0)
                _perceptrons = new List<Perceptron>();
            List<double> initVector = new List<double>();
            Random random = new Random();
            foreach (var languageDirectory in Directory.GetDirectories(trainDataDir))
            {
                initVector = new List<double>();
                for (int i = 0; i < AlphabetSize; i++) initVector.Add(random.NextDouble());
                string directoryName = Path.GetFileName(languageDirectory);
                _languageToInt.Add(directoryName, _languageToInt.Count);
                _languageFromInt.Add(directoryName);
                if(generation == 0)
                    _perceptrons!.Add(new Perceptron(Alpha,Theta,initVector));
                foreach (var file in Directory.GetFiles(languageDirectory))
                    _trainData.Add(new KeyValuePair<List<double>, int>(FilterData(file),_languageToInt[directoryName]));
            }
            
            _trainData =  _trainData.OrderBy(_ => Guid.NewGuid()).ToList();
            foreach (var vec in _trainData)
            {
                for (int i = 0; i < _perceptrons!.Count; i++)
                {
                    _perceptrons[i].TrainMode = true;
                    bool xd = _perceptrons[i].MakeDecision(vec.Key, vec.Value == i);
                    Console.WriteLine("perceptron "+i+" " +xd);
                }
            }

            generation++;

            Form1.Instance!.ChoseTestFileButton.Enabled = true;
        }
    }

    public void ChoseTestFileClick(object sender, EventArgs e, string name)
    {
        OpenFileDialog openFileDialog = new OpenFileDialog();
        openFileDialog.Title = "Select test data file";
        if (openFileDialog.ShowDialog() == DialogResult.OK)
        {
            List<double> filteredData = FilterData(openFileDialog.FileName);
            double biggestNet=-1;
            int closestPerceptronNumber=0;
            for(int i=0;i<_perceptrons!.Count;i++)
            {
                _perceptrons[i].TrainMode = false;
                double perceptronNet = _perceptrons[i].Net(filteredData);
                Console.WriteLine(perceptronNet);
                foreach (var x in _perceptrons[i].GetVector())
                {
                    Console.Write(x+", ");
                }
                Console.WriteLine();
                if (perceptronNet > biggestNet)
                {
                    biggestNet = perceptronNet;
                    closestPerceptronNumber = i;
                }
            }

            MessageBox.Show("Language is: "+_languageFromInt![closestPerceptronNumber]);
        }
    }

    public List<double> FilterData(string path)
    {
        Console.WriteLine(path);
        using var reader = new StreamReader(path);
        int[] noOccurrences  = new int[AlphabetSize];
        int noLetters=0;
        while (reader.ReadLine() is { } line)
        {
            foreach (var c in line.ToLower())
            {
                if (c - 'a' >= 0 && c - 'a' < AlphabetSize)
                {
                    noOccurrences[c - 'a']++;
                    noLetters++;
                }
            }
        }
        
        return noOccurrences.Select(count => (double)count/noLetters).ToList();
    }

}