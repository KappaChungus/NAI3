namespace NAI3;

public class DataFilter
{
    private const int AlphabetSize = 26;
    public List<double> FilterData(string path)
    {
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