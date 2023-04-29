using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital.Domain.Shared
{
    public class SharedEnums
    {
        public enum DayOfWeekEnum
        {
            Saturday = 1,
            Sunday,
            Monday,
            Tuesday,
            Wednesday,
            Thursday,
            Friday
        }
        public enum Gender
        {
            Male = 1,
            Female = 2,
            Other = 3
        }

        public enum AppointmentStatus
        {
            Requested = 0,
            WaitngToAcceptByHospital = 1,
            ProcessingByHospital = 2,
            Done = 3,
            Canceled = 4,
            NoShow = 5,
            Rescheduled = 6
        }
        public enum OperationAction
        {
            Export = 1,
            Edit = 2,
            Create = 3,
            Read = 4
        }
    }
}
