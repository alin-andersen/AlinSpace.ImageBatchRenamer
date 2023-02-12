namespace AlinSpace.ImageBatchRenamer
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var pathToDirectory = args.FirstOrDefault();

            pathToDirectory = PathHelper.MakeRoot(pathToDirectory);

            if (string.IsNullOrWhiteSpace(pathToDirectory))
            {
                Console.WriteLine($"Path to directory argument missing.");
                return;
            }


            var files = Directory.GetFiles(pathToDirectory);

            if (files.Empty())
            {
                Console.WriteLine($"No files in the give directory found.");
                return;
            }

            Console.WriteLine($"Found {files.Length} file(s).");

            var outputPath = Path.Combine(pathToDirectory, "output");
            PathHelper.CreateDirectoryIfNotExist(outputPath);

            Console.WriteLine($"New files will be saved at {outputPath}.");

            var newFiles = files
                .Select(x => (x, ImageDataTakenHelper.GetDateTakenFromImage(x)))
                .Where(x => x.Item2 != null)
                .OrderBy(x => x.Item2)
                .Select(x => x.Item1)
                .ToList();

            var counter = 1;

            foreach(var newFile in newFiles)
            {
                var extension = Path.GetExtension(newFile)[1..];
                var finalPath = Path.Combine(outputPath, $"{counter.ToString("D10")}.{extension}");

                Console.WriteLine($"Copying file {newFile} to {finalPath} ...");

                File.Copy(newFile, finalPath);

                counter++;
            }

            Console.WriteLine($"Done.");
        }
    }
}