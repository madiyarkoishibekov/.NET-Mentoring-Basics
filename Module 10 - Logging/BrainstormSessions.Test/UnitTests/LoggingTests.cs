using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BrainstormSessions.Api;
using BrainstormSessions.Controllers;
using BrainstormSessions.Core.Interfaces;
using BrainstormSessions.Core.Model;
using Moq;
using Serilog;
using Serilog.Sinks.TestCorrelator;
using Xunit;

namespace BrainstormSessions.Test.UnitTests
{
    public class LoggingTests
    {
        public LoggingTests()
        {
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .WriteTo.TestCorrelator()
                .CreateLogger();
        }

        [Fact]
        public async Task HomeController_Index_LogInfoMessages()
        {
            // Arrange
            var mockRepo = new Mock<IBrainstormSessionRepository>();
            mockRepo.Setup(repo => repo.ListAsync())
            .ReturnsAsync(GetTestSessions());
            var controller = new HomeController(mockRepo.Object);

            using (TestCorrelator.CreateContext())
            {
                // Act
                var result = await controller.Index();
                
                // Assert
                Assert.Contains(TestCorrelator.GetLogEventsFromCurrentContext(), e => e.Level == Serilog.Events.LogEventLevel.Information);
            }
        }

        [Fact]
        public async Task HomeController_IndexPost_LogWarningMessage_WhenModelStateIsInvalid()
        {
            // Arrange
            var mockRepo = new Mock<IBrainstormSessionRepository>();
            mockRepo.Setup(repo => repo.ListAsync())
                .ReturnsAsync(GetTestSessions());
            var controller = new HomeController(mockRepo.Object);
            controller.ModelState.AddModelError("SessionName", "Required");
            var newSession = new HomeController.NewSessionModel();

            using (TestCorrelator.CreateContext())
            {
                // Act
                var result = await controller.Index(newSession);

                // Assert
                Assert.Contains(TestCorrelator.GetLogEventsFromCurrentContext(), e => e.Level == Serilog.Events.LogEventLevel.Warning);
            }
        }

        [Fact]
        public async Task IdeasController_CreateActionResult_LogErrorMessage_WhenModelStateIsInvalid()
        {
            // Arrange & Act
            var mockRepo = new Mock<IBrainstormSessionRepository>();
            var controller = new IdeasController(mockRepo.Object);
            controller.ModelState.AddModelError("error", "some error");

            using (TestCorrelator.CreateContext())
            {
                // Act
                var result = await controller.CreateActionResult(model: null);

                // Assert
                Assert.Contains(TestCorrelator.GetLogEventsFromCurrentContext(), e => e.Level == Serilog.Events.LogEventLevel.Error);
            }
        }

        [Fact]
        public async Task SessionController_Index_LogDebugMessages()
        {
            // Arrange
            int testSessionId = 1;
            var mockRepo = new Mock<IBrainstormSessionRepository>();
            mockRepo.Setup(repo => repo.GetByIdAsync(testSessionId))
                .ReturnsAsync(GetTestSessions().FirstOrDefault(
                    s => s.Id == testSessionId));
            var controller = new SessionController(mockRepo.Object);

            using (TestCorrelator.CreateContext())
            {
                // Act
                var result = await controller.Index(testSessionId);

                // Assert
                Assert.True(TestCorrelator.GetLogEventsFromCurrentContext().Count(e => e.Level == Serilog.Events.LogEventLevel.Debug) == 2);
            }

        }

        private List<BrainstormSession> GetTestSessions()
        {
            var sessions = new List<BrainstormSession>();
            sessions.Add(new BrainstormSession()
            {
                DateCreated = new DateTime(2016, 7, 2),
                Id = 1,
                Name = "Test One"
            });
            sessions.Add(new BrainstormSession()
            {
                DateCreated = new DateTime(2016, 7, 1),
                Id = 2,
                Name = "Test Two"
            });
            return sessions;
        }

    }
}
