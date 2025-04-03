

namespace NAI3;

public class Perceptron
{
    public bool TrainMode { get; set; }
    private readonly double _alpha;
    private double _theta;
    private readonly List<double> _weights;

    public Perceptron(double alpha, double theta, List<Double> weights)
    {
        _alpha = alpha;
        _theta = theta;
        _weights = weights;
        Normalize();
        TrainMode = true;
    }

    public bool MakeDecision(List<double> x,bool d)
    {
        if(x.Count != _weights.Count)
            throw new ArgumentException("size mismatch");
        bool y = Net(x)>0;
        if (TrainMode && y != d)
        {
            Learn(x, d ? 1 : 0, y ? 1 : 0);
        }

        return y;
    }

    private void Learn(List<Double> x,int d, int y)
    {
        for (int i = 0; i < x.Count; i++)
            _weights[i]+=(d-y)*x[i]*_alpha;
        _theta = - _theta*(d-y)*_alpha;
    }

    public double Net(List<double> x)
    {
        return x.Select((t, i) => _weights[i] * t).Sum() - _theta;
    }

    private void Normalize()
    {
        double lenght = double.Sqrt(_weights.Sum(x => x*x));
        for (int i = 0; i < _weights.Count; i++)
        {
            _weights[i] /= lenght;
        }

    }

    public List<double> GetVector()
    {
        return _weights;
    }
}