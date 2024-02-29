using System.Diagnostics;

namespace HanoiTowers
{
    internal class Program
    {
        static void Main(string[] args)
        {
            
            short numPegs = 4;
            // Izbira vrste
            HanoiType selectedType = Hanoi.SelectHanoiType();
            Console.WriteLine($"Selected Hanoi Type: {selectedType}");

            Console.Write("Enter number of discs: ");
            short numDiscs = (short)int.Parse(Console.ReadLine());

            Console.WriteLine($"Running case: {selectedType} with {numDiscs} discs:");
            // Instantiate Hanoi object with desired parameters
            //Hanoi hanoi = HanoiFactory.GetHanoi(numDiscs, numPegs, selectedType);
            Hanoi hanoi = HanoiFactory.GetHanoi(numDiscs, numPegs, selectedType);


            // Perform calculations
            string path;
            //int shortestPath = hanoi.Move(out path);
            int shortestPath = hanoi.MakeMoveForSmallDimension(out path);


            // Output results

            
            Console.WriteLine();
            Console.WriteLine($"Shortest Path: {shortestPath}");
            Console.ReadLine(); // Keep console open to view the output


        }
    }
}