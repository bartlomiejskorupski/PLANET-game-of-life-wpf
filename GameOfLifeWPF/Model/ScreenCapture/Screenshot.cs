using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using System.Windows.Media;
using System.Windows;

namespace GameOfLifeWPF.Model.ScreenCapture
{
    public class Screenshot
    {
        public static void SaveUIElementToJpeg(UIElement element, string filePath, double dpi = 96)
        {
            var renderTargetBitmap = new RenderTargetBitmap(
                (int)element.RenderSize.Width, (int)element.RenderSize.Height,
                dpi, dpi, PixelFormats.Default
            );

            renderTargetBitmap.Render(element);

            var jpegEncoder = new JpegBitmapEncoder();
            jpegEncoder.Frames.Add(BitmapFrame.Create(renderTargetBitmap));

            using var fs = new FileStream(filePath, FileMode.Create);
            jpegEncoder.Save(fs);
        }
    }
}
