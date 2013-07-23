using System.Windows.Media.Imaging;

namespace Lab3.Filtering
{
    public class ImageCreator
    {
        public static BitmapImage GetImage(IFilter filter)
        {
            using (var bitmap = filter.GetFilteredImage())
            {
                return bitmap.ConvertToBitmapImage();
            }
        }
    }
}
