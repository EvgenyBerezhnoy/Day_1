using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace HospitalApp.UserControls
{
    /// <summary>
    /// Логика взаимодействия для ScheduleAppointmentControl.xaml
    /// </summary>
    public partial class ScheduleAppointmentControl : UserControl
    {
        public ScheduleAppointmentControl()
        {
            InitializeComponent();
        }

       
            private void UserControl_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
            {
                if (e.NewValue is Entitites.ScheduleAppointment currentAppointment)
                {
                    BtnAppointment.Content = $"{currentAppointment.StartTime.ToString(@"hh\:mm")}" + $" { currentAppointment.EndTime.ToString(@"hh\:mm") }";
                
                switch (currentAppointment.AppointnemntType)
                {
                    case Entitites.AppointnemntType.Free:
                    {
                            BtnAppointment.IsEnabled = true;
                            BtnAppointment.Foreground = new SolidColorBrush(Colors.White);
                            BtnAppointment.Visibility = Visibility.Visible;
                        }
                        break;
                    case Entitites.AppointnemntType.Busy:
                        {
                            BtnAppointment.IsEnabled = false;
                            BtnAppointment.Foreground = new SolidColorBrush(Colors.Gray);
                            BtnAppointment.Visibility = Visibility.Visible;
                        }
                        break;

                    case Entitites.AppointnemntType.DayOff:
                        {
                            BtnAppointment.Visibility = Visibility.Hidden;
                        }
                        break;
                }
            }
           
         }
        
    }
}
