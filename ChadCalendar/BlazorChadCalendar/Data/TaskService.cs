using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorChadCalendar.Data
{
    public class TaskService
    {
        List<Data.Task> tasks = new List<Task>();
        int counter = 0;
        public List<Data.Task> GetTasks()
        {
            return tasks;
        }
        private void taskInitialization(ref Data.Task task)
        {
            task.Id = counter++;
        }
        public void AddTask(Data.Task task)
        {
            taskInitialization(ref task);
            tasks.Add(task);
        }
    }
}
