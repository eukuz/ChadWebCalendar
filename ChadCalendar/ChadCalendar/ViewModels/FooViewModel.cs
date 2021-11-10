using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChadCalendar.ViewModels
{
    public class FooViewModel
    {
        public IEnumerable<Models.Event> Events { get; set; }
        public IEnumerable<Models.Task> Tasks { get; set; }
        public IEnumerable<Models.Project> Projects { get; set; }
        public string SelectProjectName { get; set; }
        public int? SelectProjectId { get; set; }
        public int? Id { get; set; } // не трогайте поле без надобности, используется в Home - Index для передачи Id при завершении задачи
    }
}
