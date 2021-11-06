using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChadCalendar.TaskHandlers
{
    public class ErrorsHandler
    {
        public IEnumerable<string> GetErrors(ref Models.Task task)
        {
            List<string> errors = new List<string>();
            //Тут обновить логику
            errors.Add("mama ymerla");
            errors.Add("daun na midere");
            //
            return errors;
        }
    }
}
