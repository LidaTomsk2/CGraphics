using System.Drawing;

namespace Lab3.Filtering
{
    public interface IFilter
    {
        Bitmap GetFilteredImage();
    }
}