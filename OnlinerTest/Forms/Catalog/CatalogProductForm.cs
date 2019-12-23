using System;
using System.IO;
using AShotNet;
using AShotNet.Coordinates;
using AShotNet.Cropper.Indent;
using AutomationFramework.Browsers;
using AutomationFramework.Elements;
using AutomationFramework.Forms;
using AutomationFramework.Utilities;
using OpenQA.Selenium;

namespace OnlinerTest.Forms.Catalog
{
    public class CatalogProductForm : BaseForm
    {
        private Label TitleLabel => new Label(By.ClassName("catalog-masthead__title"), "Title");

        public CatalogProductForm() : base(By.XPath("//div[contains(@class, 'product_details')]"), "Product")
        {
        }

        public string GetTitle()
        {
            //Browser.GetInstance().Scroll(200);
            new AShot().CoordsProvider(new WebDriverCoordsProvider())
                .ImageCropper(new IndentCropper(1000).AddIndentFilter(new BlurFilter()))
                .TakeScreenshot(Browser.GetInstance().GetDriver(), TitleLabel.GetElement())
                .getImage()
                .Save(Path.Combine(FileProvider.GetOutputDirectory(), $"ScreenshotTest_{DateTime.Now.ToFileTime()}.png"));
            return TitleLabel.GetText().Trim();
        }
    }
}