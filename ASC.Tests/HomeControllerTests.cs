using ASC.Solution.Configuration;
using ASC.Solution.Controllers;
using ASC.Tests.TestUtilities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;
using ASC.Utilities;

namespace ASC.Tests
{
    public class HomeControllerTests
    {
        private readonly Mock<IOptions<ApplicationSettings>> optionsMock;
        private readonly Mock<HttpContext> mockHttpContext;
        private readonly Mock<ILogger<HomeController>> mockLogger;

        public HomeControllerTests()
        {
            // Tạo Mock cho IOptions<ApplicationSettings>
            optionsMock = new Mock<IOptions<ApplicationSettings>>();
            optionsMock.Setup(ap => ap.Value).Returns(new ApplicationSettings
            {
                ApplicationTitle = "ASC"
            });

            // Tạo Mock cho ILogger<HomeController>
            mockLogger = new Mock<ILogger<HomeController>>();

            // Tạo Mock cho HttpContext và Session giả lập
            mockHttpContext = new Mock<HttpContext>();
            mockHttpContext.Setup(p => p.Session).Returns(new FakeSession());
        }

        [Fact]
        public void HomeController_Index_View_Test()
        {
            // Khởi tạo HomeController với Mock ILogger
            var controller = new HomeController(mockLogger.Object, optionsMock.Object);
            controller.ControllerContext.HttpContext = mockHttpContext.Object;

            // Kiểm tra kiểu trả về là ViewResult
            Assert.IsType<ViewResult>(controller.Index());
        }

        [Fact]
        public void HomeController_Index_NoModel_Test()
        {
            var controller = new HomeController(mockLogger.Object, optionsMock.Object);
            controller.ControllerContext.HttpContext = mockHttpContext.Object;

            // Kiểm tra Model là null
            Assert.Null((controller.Index() as ViewResult)?.ViewData.Model);
        }

        [Fact]
        public void HomeController_Index_Validation_Test()
        {
            var controller = new HomeController(mockLogger.Object, optionsMock.Object);
            controller.ControllerContext.HttpContext = mockHttpContext.Object;

            // Kiểm tra số lượng lỗi ModelState = 0
            Assert.Equal(0, (controller.Index() as ViewResult)?.ViewData.ModelState.ErrorCount);
        }

        [Fact]
        public void HomeController_Index_Session_Test()
        {
            var controller = new HomeController(mockLogger.Object, optionsMock.Object);
            controller.ControllerContext.HttpContext = mockHttpContext.Object;
            controller.Index();

            // Kiểm tra session không null
            Assert.NotNull(controller.HttpContext.Session.GetSession<ApplicationSettings>("Test"));
        }
    }
}
