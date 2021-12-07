using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChadWebCalendar.Data.Services
{
    public static class Constants
    {
        // constants for NotificationsService
        public const int MillisecondsForSleepAfterWorkerIteration = 60000;
        public const int AdditionInMinutesForMinDT = 35;
        public const int MinutesBeforeForInterval = 0;
        public const int CountOfSymbolsForFullDateTime = 18;
        public const int NumberOfInitialTimePosition = 11;
        public const int LenForFullDT = 4;
        public const int LenForTrimmedDT = 5;

        // constants for task Components
        public const int TempIdForDefaultChoice = -1;

        //здесь другие константы
    }
}
