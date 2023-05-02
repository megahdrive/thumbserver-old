using System;
using System.Drawing.Imaging;
using System.Drawing;
using System.IO;
using ImageMagick;

namespace ThumbnailServer
{
    internal class PostProcess
    {
        public static Bitmap Screenshot()
        {
            Rectangle rect = new Rectangle(0, 0, AppConfig.WindowSizeX, AppConfig.WindowSizeY);

            DLLImport.SetForegroundWindow(ClientInterface.RobloxApp);
            Bitmap bmp = new Bitmap(rect.Width, rect.Height, PixelFormat.Format32bppArgb);
            Graphics gfx = Graphics.FromImage(bmp);
            gfx.CopyFromScreen(rect.Left, rect.Top, 0, 0, bmp.Size, CopyPixelOperation.SourceCopy);
            return bmp;
        }

        public static string BitmapToBase64(Bitmap bitmap)
        {
            MemoryStream ms = new System.IO.MemoryStream();
            bitmap.Save(ms, ImageFormat.Png);
            byte[] byteImage = ms.ToArray();
            return Convert.ToBase64String(byteImage);
        }

        public static Bitmap CleanBitmap(Bitmap bitmap, MagickColor color)
        {
            /*Color clr = bitmap.GetPixel(25, 25);
            Debug.WriteLine(clr);
            bitmap.MakeTransparent(clr);*/

            MemoryStream ms = new MemoryStream();
            bitmap.Save(ms, ImageFormat.Png);
            byte[] i = ms.ToArray();

            try
            {
                using (MagickImage image = new MagickImage(i))
                {
                    //Remove border from image
                    image.Shave(3, 3);
                    image.Settings.TextGravity = Gravity.South;
                    //image.Chop(new MagickGeometry(0, 3));
                    //image.Resize(180, 220);


                    image.ColorFuzz = new Percentage(42);
                    image.Transparent(color);
                    ms.SetLength(0);
                    image.Write(ms);
                }
            } catch (Exception ex)
            {
                Logger.Add(ex.Message, "post_process");
                return new Bitmap(ms);
            }
            return new Bitmap(ms);
        }
    }
}
