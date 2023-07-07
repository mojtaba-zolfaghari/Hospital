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
            NotBook=0,
            Requested=1,
            WaitngToAcceptByHospital = 2,
            ProcessingByHospital=3,
            Done = 4,
            Canceled=5,
            NoShow=6,
            Rescheduled=7
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
