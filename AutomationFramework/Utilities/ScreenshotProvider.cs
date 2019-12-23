using System.Drawing.Imaging;
using System.IO;
using AShotNet;
using AShotNet.Coordinates;
using AShotNet.ScreenTaker;
using AutomationFramework.Browsers;

namespace AutomationFramework.Utilities
{
    public class ScreenshotProvider
    {
        public static string PublishScreenshot(string name)
        {
            var image = new AShot().ShootingStrategy(new ViewportPastingStrategy(100))
                .CoordsProvider(new WebDriverCoordsProvider()).TakeScreenshot(Browser.GetInstance().GetDriver())
                .getImage();
            var fileLocation = Path.Combine(FileProvider.GetOutputDirectory(), name);
            image.Save(fileLocation, ImageFormat.Png);
            var imageLocation = $"{fileLocation}.png";
            File.Move(fileLocation, imageLocation);
            return imageLocation;
        }
    }
}
