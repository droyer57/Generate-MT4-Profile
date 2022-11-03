using System;
using System.IO;

namespace GenerateMT4Profile;

public sealed class Generator
{
    private const string DataFolder = "Data";
    private const string ProfilesFolder = "Profiles";

    private string _profilePath = null!;

    public void Start()
    {
        Directory.CreateDirectory(DataFolder);
        Directory.CreateDirectory(ProfilesFolder);

        Console.Write("Nom du profil: ");
        var profileName = Console.ReadLine();
        if (string.IsNullOrWhiteSpace(profileName))
        {
            profileName = Guid.NewGuid().ToString("N");
        }

        _profilePath = $"{ProfilesFolder}/{profileName}";

        var files = Directory.GetFiles(DataFolder, "*.set", SearchOption.AllDirectories);
        if (files.Length == 0)
        {
            throw new Exception($"Pas de fichier dans le dossier {DataFolder}");
        }

        if (Directory.Exists(_profilePath))
        {
            Directory.Delete(_profilePath, true);
        }

        Directory.CreateDirectory(_profilePath);

        for (var i = 0; i < files.Length; i++)
        {
            GenerateChart(files[i], i + 1);
        }

        Console.WriteLine($"Profil généré: {_profilePath}");
    }

    private void GenerateChart(string path, int index)
    {
        var file = new FileSet(path);

        using var sw = new StreamWriter($"{_profilePath}/chart{index:00}.chr");

        sw.WriteLine("<chart>");

        sw.WriteLine($"id={index}");
        sw.WriteLine($"symbol={file.Symbol}");
        sw.WriteLine($"period={file.Period}");
        sw.WriteLine("grid=0");
        sw.WriteLine("window_type=3");

        sw.WriteLine("\n<expert>");

        sw.WriteLine($"name={file.RobotName}");
        sw.WriteLine("flags=279");

        sw.WriteLine("<inputs>");

        sw.WriteLine(file.GetInputs());
        sw.WriteLine($"EA_Magic_Number={DateTime.Now:yyMMdd}{index}");
        sw.WriteLine($"EA_Comment={Path.GetFileNameWithoutExtension(path)}");

        sw.WriteLine("</inputs>");
        sw.WriteLine("</expert>");
        sw.WriteLine("</chart>");
    }
}