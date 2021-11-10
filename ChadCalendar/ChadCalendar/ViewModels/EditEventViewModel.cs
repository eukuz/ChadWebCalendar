using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChadCalendar.ViewModels
{
    public class EditEventViewModel
    {
       public Models.Event _event { get; set; }
       public int projectIDforMutation { get; set; }
    }
}
