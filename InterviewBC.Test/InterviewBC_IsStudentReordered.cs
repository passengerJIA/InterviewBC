using InterviewBC.Controllers;
using InterviewBC.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;

namespace InterviewBC.Test
{
    public class InterviewBC_IsStudentReordered
    {
        [Fact]
        public async Task IsStudentTotalLargeEnough()
        {
            var mockLogger = new Mock<ILogger<StudentsController>>();
            mockLogger.SetReturnsDefault(Microsoft.Extensions.Logging.LoggerFactory.Create(config => config.AddConsole()).CreateLogger("console"));
            var controller = new StudentsController(mockLogger.Object);
            var result = (OkObjectResult)await controller.Get() ?? null;
            Assert.NotNull(result);
            var lists = result.Value as Tuple<List<Student>, List<Student>> ?? null;
            Assert.NotNull(lists);
            Assert.True(lists.Item1.Count >= 20);
            Assert.True(lists.Item1.Count == lists.Item2.Count);
            Assert.True(lists.Item1.Select(s => s.Id).Any(id1 => lists.Item2.Select(s => s.Id).Contains(id1)));
        }
    }
}
