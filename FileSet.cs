using System;
using System.IO;
using System.Linq;

namespace GenerateMT4Profile;

public sealed class FileSet
{
    private readonly string _path;

    public string RobotName { get; }
    public string Symbol { get; }
    public int Period { get; }

    public FileSet(string path)
    {
        _path = path;

        var fileName = Path.GetFileNameWithoutExtension(path);

        var data = fileName.Split(" - ");
        if (data.Length != 3)
        {
            throw new Exception($"Fichier {fileName} invalide");
        }

        RobotName = data[0];
        Symbol = data[1];
        Period = GetPeriod(data[2]);
    }

    private static int GetPeriod(string period)
    {
        return period switch
        {
            "M1" => 1,
            "M5" => 5,
            "M15" => 15,
            "M30" => 30,
            "H1" => 60,
            "H4" => 240,
            "D1" => 1440,
            "W1" => 10080,
            "MN" => 40320,
            _ => 0
        };
    }

    public string GetInputs()
    {
        var lines = File.ReadAllLines(_path);
        var inputs = lines.Where(item =>
                !item.Contains(',') && !item.Contains("EA_Magic_Number") && !item.Contains("EA_Comment"))
            .Aggregate(string.Empty, (current, item) => current + item + "\n");
        
        inputs = inputs.Remove(inputs.Length - 1, 1);

        return inputs;
    }
}