using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using AutomationFramework.Browsers;

namespace AutomationFramework.Utilities
{
    public class ScreenshotProvider
    {
        public static string PublishScreenshot(string name)
        {
            var image = GetImage(Browser.GetInstance().GetScreenshot());
            var fileLocation = Path.Combine(FileProvider.GetOutputDirectory(), name);
            image.Save(fileLocation, ImageFormat.Png);
            var imageLocation = $"{fileLocation}.png";
            File.Move(fileLocation, imageLocation);
            return imageLocation;
        }

        private static Image GetImage(byte[] imageSource)
        {
            using (var ms = new MemoryStream(imageSource))
            {
                return Image.FromStream(ms);
            }
        }
    }
}
