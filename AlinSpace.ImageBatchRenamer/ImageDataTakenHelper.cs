using MetadataExtractor;
using MetadataExtractor.Formats.Exif;

namespace AlinSpace.ImageBatchRenamer
{
    public static class ImageDataTakenHelper
    {
        public static DateTime? GetDateTakenFromImage(string path)
        {
            path = PathHelper.MakeRoot(path);

            var stream = File.OpenRead(path);
            var directories = ImageMetadataReader.ReadMetadata(stream);

            // Find the so-called Exif "SubIFD" (which may be null)
            var subIfdDirectory = directories.OfType<ExifSubIfdDirectory>().FirstOrDefault();

            var dateTime = subIfdDirectory?.GetDateTime(ExifDirectoryBase.TagDateTimeOriginal);

            return dateTime;
        }
    }
}
