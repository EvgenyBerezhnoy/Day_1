using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HospitalApp.Utils
{
    class ScheduleGenerator
    {
        private DateTime _startDate;
        private DateTime _endDate;
        private List<Entitites.DoctorSchedule> _doctorSchedule;

        public ScheduleGenerator(DateTime startDate, DateTime endDate, List<Entitites.DoctorSchedule> schedule)
        {
            _startDate = startDate;
            _endDate = endDate;
            _doctorSchedule = schedule.Where(p => p.Date >= _startDate.Date && p.Date <= _endDate.Date).ToList();
        }
        public List<Entitites.ScheduleHeader> GenerateHeaders()
        {
            var result = new List<Entitites.ScheduleHeader>();

            var startDate = _startDate;
            while(startDate.Date < _endDate.Date)
            {
                result.Add(new Entitites.ScheduleHeader
                {
                    Date = startDate.Date
                });
                startDate = startDate.AddDays(1);
            }


            return result;
        }

        public List<List<Entitites.ScheduleAppointment>> GenerateAppointments(List<Entitites.ScheduleHeader> headers)
        {
            var result = new List<List<Entitites.ScheduleAppointment>>();

            if(_doctorSchedule.Count()>0)
            {
                var minStartTime = _doctorSchedule.Min(p => p.StartTime);
                var maxEndTime = _doctorSchedule.Max(p => p.EndTime);

                var startTime = minStartTime;
                while(startTime < maxEndTime)
                {
                    var appointmentsPerInterval = new List<Entitites.ScheduleAppointment>();

                    foreach(var header in headers)
                    {
                        var currentSchedule = _doctorSchedule.FirstOrDefault(p => p.Date == header.Date);
                        var scheduleAppointment = new Entitites.ScheduleAppointment
                        {
                            ScheduleId = currentSchedule?.Id ?? -1,
                            StartTime = startTime,
                            EndTime = startTime.Add(TimeSpan.FromMinutes(30))
                        };
                        if (currentSchedule != null)
                        {
                            var busyAppointment = currentSchedule.Appointment.FirstOrDefault(p => p.StartTime == startTime);
                            if (busyAppointment != null)
                            {
                                scheduleAppointment.AppointnemntType = Entitites.AppointnemntType.Busy;
                            }
                            else
                            {
                                if (startTime < currentSchedule.StartTime || startTime >= currentSchedule.EndTime)
                                {
                                    scheduleAppointment.AppointnemntType = Entitites.AppointnemntType.DayOff;
                                }
                                else
                                {
                                    scheduleAppointment.AppointnemntType = Entitites.AppointnemntType.Free;
                                }
                            }
                        }
                        else
                        {
                            scheduleAppointment.AppointnemntType = Entitites.AppointnemntType.DayOff;
                        }

                        appointmentsPerInterval.Add(scheduleAppointment);
                    }

                    result.Add(appointmentsPerInterval);
                    startTime = startTime.Add(TimeSpan.FromMinutes(30));  
                }
            }

            return result;
        }
    }
}
