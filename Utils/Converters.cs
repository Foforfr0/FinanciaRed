using System.IO;
using System.Windows.Media.Imaging;

namespace FinanciaRed.Utils {
    internal class Converters {
        public static BitmapImage ConvertByteToBitmapImage (byte[] bytesImage) {
            BitmapImage bitmapImage = new BitmapImage ();
            using (MemoryStream ms = new MemoryStream (bytesImage)) {
                bitmapImage.BeginInit ();
                bitmapImage.StreamSource = ms;
                bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapImage.EndInit ();
            }
            return bitmapImage;
        }
    }
}
