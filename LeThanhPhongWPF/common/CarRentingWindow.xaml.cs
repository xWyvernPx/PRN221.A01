using Business.Service.Interface;
using Domain.Models;
using LeThanhPhongWPF.Extensions;
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
using System.Windows.Shapes;

namespace LeThanhPhongWPF.common
{
    /// <summary>
    /// Interaction logic for CarRentingWindow.xaml
    /// </summary>
    public partial class CarRentingWindow : Window
    {
        private readonly ICarService carService;
        private readonly IRentingDetailService rentingDetailService;
        public List<RentingDetail> rentingDetails {  get; set; }

        public CarRentingWindow(ICarService carService, IRentingDetailService rentingDetailService)
        {
            InitializeComponent();
            this.carService = carService;
            this.rentingDetailService = rentingDetailService;
            Loaded += LoadedHanler;
        }
        public Action<RentingDetail> AddCallback { get; set; }
        private void ReloadScreen()
        {
            var carSelected = (CarInformation)carCombobox.SelectedItem;
            var fromDate = dateFrom.SelectedDate;
            var toDate = dateTo.SelectedDate;
            if (carSelected is not null)
            {
                txtPricePerDay.Text = carSelected.CarRentingPricePerDay.ToString();
                if(fromDate is not null && toDate is not null)
                {
                    TimeSpan duration = (TimeSpan)(toDate - fromDate);
                    int numberOfDays = duration.Days + 1;
                    txtPriceInTotal.Text = (numberOfDays * carSelected.CarRentingPricePerDay).ToString();
                }
            }
        }
        private void LoadedHanler(object sender, RoutedEventArgs e)
        {
            var carList = carService.GetAll().ToList();
            carCombobox.ItemsSource = carList;
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }

        private void dateFrom_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            ReloadScreen();
            dateTo.DisplayDateStart = dateFrom.SelectedDate;
        }

        private void dateTo_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            ReloadScreen();
        }

        private void carCombobox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ReloadScreen();
        }
        private bool CheckAvailable(RentingDetail rentingDetail)
        {
            bool result = true;

            var persistedRentings = rentingDetailService.GetAll().Where(rd => rd.CarId == rentingDetail.CarId).ToList();
            rentingDetails.ForEach(rd => {
                if (rd.CarId == rentingDetail.CarId)
                {
                    if (DateTime.Compare(rentingDetail.StartDate, rd.StartDate) > 0)
                    {
                        if (!(DateTime.Compare(rentingDetail.StartDate, rd.EndDate) > 0))
                        {
                            result = false;
                        }
                    }
                    else
                    {

                        if (!(DateTime.Compare(rentingDetail.EndDate, rd.StartDate) < 0))
                        {
                            result = false;
                        }
                    }
                }
            });
            persistedRentings.ForEach(rd => {
                if (DateTime.Compare(rentingDetail.StartDate, rd.StartDate) > 0)
                {
                    if (!(DateTime.Compare(rentingDetail.StartDate, rd.EndDate) > 0))
                    {
                        result = false;
                    }
                }
                else
                {

                    if (!(DateTime.Compare(rentingDetail.EndDate, rd.StartDate) < 0))
                    {
                        result = false;
                    }
                }
            });
            return result;
        }
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            var carSelected = (CarInformation)carCombobox.SelectedItem;
            var fromDate = dateFrom.SelectedDate;
            var toDate = dateTo.SelectedDate ;
            if(carSelected is null ||  fromDate is null  ||  toDate is null)
            {
                MessageBox.Show("Please select all required fields");
            }else
            {

            var newRenting = new RentingDetail()
            {
                Car = carSelected,
                CarId = carSelected.CarId,
                EndDate = toDate?? DateTime.Now,
                StartDate = fromDate ?? DateTime.Now,
                Price = carSelected.CarRentingPricePerDay,
            };
                if (!CheckAvailable(newRenting))
                {
                    Utils.ErrorAlert("This car is not available on selected dates");
                    return;
                }
            AddCallback.Invoke(newRenting);
            this.DialogResult = false;
            }
        }
    }
}
