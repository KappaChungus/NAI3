namespace NAI3;

public class Network
{
    private const double Alpha= 0.5;
    private const double Theta = 1;
    private const int AlphabetSize = 26;
    private const int NoGenerations = 50;

    private List<Perceptron>? _perceptrons;
    private Dictionary<string, int>? _languageToInt;
    private List<string>? _languageFromInt;
    private List<KeyValuePair<List<double>,int>>? _trainData;
    
    public Network(String path)
    {
        _trainData = [];
        _languageToInt = [];
        _languageFromInt= [];
        _perceptrons = [];
        var random = new Random();
        foreach (var languageDirectory in Directory.GetDirectories(path))
        {
            List<double> initVector = [];
            for (int i = 0; i < AlphabetSize; i++) initVector.Add(random.NextDouble());
            string directoryName = Path.GetFileName(languageDirectory);
            _languageToInt.Add(directoryName, _languageToInt.Count);
            _languageFromInt.Add(directoryName);
            _perceptrons!.Add(new Perceptron(Alpha,Theta,initVector));
            var df = new DataFilter();
            foreach (var file in Directory.GetFiles(languageDirectory))
                _trainData.Add(new KeyValuePair<List<double>, int>(df.FilterData(file),_languageToInt[directoryName]));
        }
    }

    public void TrainPerceptrones()
    {
        for (int gen = 0; gen < NoGenerations; gen++)
        {
            _trainData = _trainData!.OrderBy(_ => Guid.NewGuid()).ToList();
            foreach (var vec in _trainData)
            {
                for (int i = 0; i < _perceptrons!.Count; i++)
                {
                    _perceptrons[i].TrainMode = true;
                    _perceptrons[i].MakeDecision(vec.Key, vec.Value == i);
                }
            }
        }
    }

    public String TestPerceptrones(List<double> filteredData)
    {
        double biggestNet=-1;
        int closestPerceptronNumber=0;
        for(int i=0;i<_perceptrons!.Count;i++)
        {
            _perceptrons[i].TrainMode = false;
            double perceptronNet = _perceptrons[i].Net(filteredData);
            if (perceptronNet > biggestNet)
            {
                biggestNet = perceptronNet;
                closestPerceptronNumber = i;
            }
        }
        return _languageFromInt![closestPerceptronNumber];
    }
    
    
}