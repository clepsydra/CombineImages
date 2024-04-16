namespace CombineImages
{
    internal class Program
    {
        static void Main(string[] args)
        {
            if (args.Length != 1)
            {
                Console.WriteLine("Usage: CombineImages <directory>");
                return;
            }

            try
            {
                string directory = args[0];

                string outputDirectory = Path.Combine(directory, "Out");
                if (!Directory.Exists(outputDirectory))
                {
                    Directory.CreateDirectory(outputDirectory);
                }

                string[] files = Directory.GetFiles(directory, "*.jpg");

                Dictionary<double, List<string>> filesPerRatio = new Dictionary<double, List<string>>();

                foreach (var file in files)
                {
                    var bitmap = new System.Drawing.Bitmap(file);

                    var ratio = (double)bitmap.Width / bitmap.Height;
                    if (ratio < 1)
                        ratio = 1 / ratio;
                    Console.WriteLine($"{file}: Width/Height = {ratio} ({bitmap.Width}x{bitmap.Height}) ");

                    if (!filesPerRatio.ContainsKey(ratio))
                        filesPerRatio[ratio] = new List<string>();

                    filesPerRatio[ratio].Add(file);
                }

                int count = 1;

                foreach (var ratio in filesPerRatio.Keys.OrderByDescending(r => r))
                {
                    Console.WriteLine($"{ratio}: {filesPerRatio[ratio].Count}");

                    int width = 5000;
                    int imageHeight = (int)(width / ratio);
                    int targetImageHeight = 2 * imageHeight;
                    var filesForRatio = filesPerRatio[ratio];

                    for (int i = 0; i < filesForRatio.Count; i += 2)
                    {
                        var combinedImage = new System.Drawing.Bitmap(width, targetImageHeight);

                        var fileName = filesForRatio[i];
                        var nextFileName = (i < filesForRatio.Count - 1) ? filesForRatio[i + 1] : null;

                        Console.WriteLine($"{Path.GetFileName(fileName)} + {Path.GetFileName(nextFileName)}");

                        using (var g = System.Drawing.Graphics.FromImage(combinedImage))
                        {
                            g.Clear(System.Drawing.Color.White);

                            var bitmap1 = new System.Drawing.Bitmap(fileName);
                            var bitmap2 = nextFileName != null ? new System.Drawing.Bitmap(nextFileName) : null;

                            if (bitmap1.Width < bitmap1.Height)
                                bitmap1.RotateFlip(System.Drawing.RotateFlipType.Rotate90FlipNone);

                            if (bitmap2 != null && bitmap2.Width < bitmap2.Height)
                                bitmap2.RotateFlip(System.Drawing.RotateFlipType.Rotate90FlipNone);

                            g.DrawImage(bitmap1, new System.Drawing.Rectangle(0, 0, width, imageHeight));

                            if (bitmap2 != null)
                            {
                                g.DrawImage(bitmap2, new System.Drawing.Rectangle(0, imageHeight + 1, width, imageHeight));
                            }
                        }

                        combinedImage.Save(Path.Combine(outputDirectory, $"Out{count++}.jpg"), System.Drawing.Imaging.ImageFormat.Jpeg);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception occurred: {ex}");
            }
        }
    }
}
