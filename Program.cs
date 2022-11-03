using System;
using System.Runtime.InteropServices;

namespace GenerateMT4Profile;

internal static class Program
{
    private static void Main()
    {
        try
        {
            var generator = new Generator();
            generator.Start();
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine(ex.Message);
        }

        if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux)) return;
        
        Console.WriteLine("\nAppuyez sur une touche pour quitter");
        Console.ReadKey();
    }
}