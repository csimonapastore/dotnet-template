using BasicDotnetTemplate.MainProject.Models.Settings;
using BasicDotnetTemplate.MainProject.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BasicDotnetTemplate.MainProject.Tests
{
    [TestClass]
    public class AppSettingsUtils_Tests
    {
        private readonly AppSettings _appSettings;

        public AppSettingsUtils_Tests()
        {
            _appSettings = new AppSettings();
        }

        [TestMethod]
        public void AppSettingsUtils_Tests_IsValid()
        {
            bool result = AppSettingsUtils.CheckAppSettings(_appSettings);

            Assert.IsTrue(result);
        }
    }
}
