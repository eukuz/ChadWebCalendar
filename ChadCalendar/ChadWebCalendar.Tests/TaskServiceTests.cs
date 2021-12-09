using System;
using Xunit;
using ChadWebCalendar;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace ChadWebCalendar.Tests
{
    public class TaskServiceTests
    {
        [Fact]
        public void Add_the_task()
        {
            // Arrange

            Data.Services.TaskService taskService = new Data.Services.TaskService();
            Data.Task task = CreateTestTask();

            // Act
            bool isAdded = taskService.AddTask(task, null, null);

            // Assert
            Assert.True(isAdded);

            // Remove added commons for testing
            Data.ApplicationContext db = new Data.ApplicationContext();
            Data.Task taskInDbWithIdForDelete = db.Tasks.AsNoTracking().FirstOrDefault(t => t.Name == task.Name);
            taskService.Delete(taskInDbWithIdForDelete);
        }

        [Fact]
        public void Edit_the_task()
        {
            // Arrange
            Data.Services.TaskService taskService = new Data.Services.TaskService();
            Data.Task task = CreateTestTask();
            Data.Task changedTask = new Data.Task();
            taskService.AddTask(task, null, null);

            // Act
            changedTask = task;
            changedTask.Name = "changedName";
            changedTask.Description = "changedDesc";
            changedTask.Deadline = task.Deadline?.AddDays(1);
            changedTask.TimeTakes = task.TimeTakes?.Add(new TimeSpan(150000));
            changedTask.IsCompleted = true;
            taskService.Edit(changedTask, null, "");
            task = taskService.GetTask(task.Id);

            // Assert
            Assert.Equal(task.Name, changedTask.Name);
            Assert.Equal(task.Description, changedTask.Description);

            // Remove added commons for testing
            taskService.Delete(task);

        }

        private Data.Task CreateTestTask()
        {
            Data.Task task = new Data.Task
            {
                Name = "testName",
                Description = "testDescription",
                IsCompleted = false,
                TimeTakes = new TimeSpan(150000),
                Deadline = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, DateTime.Now.Hour, DateTime.Now.Minute, 0)
            };
            return task;
        }
    }
}
