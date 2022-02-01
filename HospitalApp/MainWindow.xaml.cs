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

namespace HospitalApp
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            ComboSpecialization.ItemsSource = App.DataBase.Specialization.ToList();
            ComboSpecialization.SelectedIndex = 0;
        }

        private void ComboSpecialization_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selectedSpecialization = ComboSpecialization.SelectedItem as Entitites.Specialization;
            if(selectedSpecialization != null)
            {
                var doctors = App.DataBase.Doctor.ToList().Where(p => p.Specialization == selectedSpecialization);
                ComboDoctor.ItemsSource = doctors;
                ComboDoctor.SelectedIndex = 0;

            }
        }

        private void ComboDoctor_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selectedDoctor = ComboDoctor.SelectedItem as Entitites.Doctor;
            if (selectedDoctor != null)
            {
                GenerateSchedule(selectedDoctor);
                TxtBlockDoctor.Text = selectedDoctor.FullName;
            }
        }
      private void GenerateSchedule(Entitites.Doctor selectedDoctor)
        {
            var startDate = DateTime.Parse("11-12-2021");
            var endDate = startDate.AddDays(5);

            var scheduleGenetrator = new Utils.ScheduleGenerator(startDate, endDate, selectedDoctor.DoctorSchedule.ToList());
           
            var headers = scheduleGenetrator.GenerateHeaders();
            var appointments = scheduleGenetrator.GenerateAppointments(headers);
            LoadSchedule(headers, appointments);
        }
        
        private void LoadSchedule(List<Entitites.ScheduleHeader> headers, List<List<Entitites.ScheduleAppointment>> appointments)
        {
            DGridSchedule.Columns.Clear();
            for(int i = 0; i < headers.Count(); i++)
            {
                FrameworkElementFactory headerFactory = new FrameworkElementFactory(typeof(UserControls.ScheduleHeaderControl));
                headerFactory.SetValue(DataContextProperty, headers[i]);

                var headerTemplate = new DataTemplate
                {
                    VisualTree = headerFactory
                };

                FrameworkElementFactory cellfatcory = new FrameworkElementFactory(typeof(UserControls.ScheduleAppointmentControl));
                
                cellfatcory.SetBinding(DataContextProperty, new Binding($"[{i}]"));

                cellfatcory.AddHandler(MouseDownEvent, new MouseButtonEventHandler(BtnAppointment_MouseDown), true);

                var cellTemplate = new DataTemplate
                {
                    VisualTree = cellfatcory
                };


                var columnTemplate = new DataGridTemplateColumn
                {
                    HeaderTemplate = headerTemplate,
                    Width = new DataGridLength(1, DataGridLengthUnitType.Star),
                    CellTemplate = cellTemplate
                };

                DGridSchedule.Columns.Add(columnTemplate);
            }

            DGridSchedule.ItemsSource = appointments;
           
        }

        private void BtnAppointment_MouseDown(object sender, MouseButtonEventArgs e)
        {
            var currentControl = sender as UserControls.ScheduleAppointmentControl;
            var currentAppointment = currentControl.DataContext as Entitites.ScheduleAppointment;
            if(currentAppointment!=null && currentAppointment.ScheduleId > 0 && currentAppointment.AppointnemntType == Entitites.AppointnemntType.Free)
            {
                App.DataBase.Appointment.Add(new Entitites.Appointment
                {
                    DoctorScheduleId = currentAppointment.ScheduleId,
                    StartTime = currentAppointment.StartTime,
                    EndTime = currentAppointment.EndTime,
                    ClientId = 1
                });
                App.DataBase.SaveChanges();
                MessageBox.Show("Вы записаны на прием");
                ComboDoctor_SelectionChanged(ComboDoctor, null);
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            PrintDialog printDialog = new PrintDialog();
            if (printDialog.ShowDialog() == true)
            {
                IDocumentPaginatorSource idpSource = flowDocumentAllData;
                printDialog.PrintDocument(idpSource.DocumentPaginator, $"Report_AllData_From_{DateTime.Now.ToShortDateString()}");
            }
        }
    }
}
