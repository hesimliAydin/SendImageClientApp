using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SendImageAppClient.Helper
{
    public class ImageHelper
    {
        public static byte[] GetBytes(string imagePath)
        {
            var image = new Bitmap(imagePath);
            ImageConverter imageconverter = new ImageConverter();
            var imagebytes = ((byte[])imageconverter.ConvertTo(image, typeof(byte[])));
            return imagebytes;
        }
    }
}
