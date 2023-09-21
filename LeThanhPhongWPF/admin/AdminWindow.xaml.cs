using Business.Service.Interface;
using Domain.Models;
using LeThanhPhongWPF.common;
using LeThanhPhongWPF.Extensions;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.ConstrainedExecution;
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

namespace LeThanhPhongWPF.admin
{
    /// <summary>
    /// Interaction logic for AdminWindow.xaml
    /// </summary>
    public partial class AdminWindow : Window
    {
        private readonly ICustomerService customerService;
        private readonly ICarService carService;
        private readonly IRentingTransactionService transactionService;
        private readonly IServiceProvider serviceProvider;
        private ObservableCollection<CarInformation> Cars { get; set; } = new();
        private ObservableCollection<Customer> Customers { get; set; } = new();

        public Action LogoutHandler {get;set;}
        private ObservableCollection<RentingTransaction> RentingTransactions { get; set; } = new();

        public AdminWindow(ICustomerService _customerServivce, ICarService carService, IRentingTransactionService rentingTransactionService, IServiceProvider serviceProvider)
        {
            InitializeComponent();
            this.customerService = _customerServivce;
            this.carService = carService;
            this.transactionService = rentingTransactionService;
            this.serviceProvider = serviceProvider;
            Loaded += (object sender, RoutedEventArgs e) =>
            {

                customerDatagrid.ItemsSource = Customers;
                customerService.GetCustomers().ToList().ForEach(customer => Customers.Add(customer));
                carDatagrid.ItemsSource = Cars;
                carService.GetAll().ToList().ForEach(car => Cars.Add(car));
                rentingDatagrid.ItemsSource = RentingTransactions;
                transactionService.GetAll().ToList().ForEach(trans => RentingTransactions.Add(trans));
                customerSearchType.ItemsSource = new List<String>() { "name","email", "telephone" };
                customerSearchType.SelectedItem = "name";
            };

        }
        private void LoadCustomer()
        {
            customerDatagrid.ItemsSource = null;
            Customers = new ObservableCollection<Customer>();
            customerService.GetCustomers().ToList().ForEach(customer => Customers.Add(customer));
            customerDatagrid.ItemsSource = Customers;
        }
        private void LoadCar()
        {
            carDatagrid.ItemsSource = null;
            Cars = new ObservableCollection<CarInformation>();
            carService.GetAll().ToList().ForEach(car => Cars.Add(car));
            carDatagrid.ItemsSource = Cars;
        }
        private void LoadTrans()
        {
            RentingTransactions = new();
            transactionService.GetAll().ToList().ForEach(trans => RentingTransactions.Add(trans));
            rentingDatagrid.ItemsSource = RentingTransactions;
        }
        private void DataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void TabItem_GotFocus(object sender, RoutedEventArgs e)
        {

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }

        private void CustomerAddClick(object sender, RoutedEventArgs e)
        {
            try
            {
                var customerAddWindow = ((ServiceProvider)this.Tag).GetService<CustomerManipulateWindow>();
                customerAddWindow.SaveSuccessCallback = () =>
                {
                    LoadCustomer();
                };
                customerAddWindow.ShowDialog();

            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }
        private void CustomerUpdateClick(object sender, RoutedEventArgs e)
        {
            try
            {
                var customerAddWindow = ((ServiceProvider)this.Tag).GetService<CustomerManipulateWindow>();
                var customer = (Customer)customerDatagrid.SelectedItem;
                if (customer is null)
                {
                    Utils.ErrorAlert("Choose a row to update");
                    return;
                }
                customerAddWindow.CustomerInformation = customer;
                customerAddWindow.ShowDialog();
                //tODO FIX
                LoadCustomer();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void CustomerDeleteClick(object sender, RoutedEventArgs e)
        {
            try
            {
                var customer = (Customer)customerDatagrid.SelectedItem;
                var confirmResult = Utils.ConfirmationBox("Do you really want to delete this customer ?");
                if (confirmResult == MessageBoxResult.Yes)
                {
                    customerService.Delete(customer);
                    LoadCustomer();
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }

        private void CarAddClick(object sender, RoutedEventArgs e)
        {
            try
            {
                var carEditWindow = ((ServiceProvider)this.Tag).GetService<CarManipulateWindow>();
                carEditWindow.SaveSuccessCallback = () =>
                {

                };
                carEditWindow.ShowDialog();
                LoadCar();
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }
        private void CarUpdateClick(object sender, RoutedEventArgs e)
        {
            try
            {
                var carEditWindow = ((ServiceProvider)this.Tag).GetService<CarManipulateWindow>();
                var car = (CarInformation)carDatagrid.SelectedItem;
                if (car is null)
                {
                    Utils.ErrorAlert("Choose a row to update");
                    return;
                }
                carEditWindow.carInformation = car;
                carEditWindow.ShowDialog();
                LoadCar();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void CarDeleteClick(object sender, RoutedEventArgs e)
        {
            try
            {
                var car = (CarInformation)carDatagrid.SelectedItem;
                var confirmResult = Utils.ConfirmationBox("Do you really want to delete this car ?");
                if (confirmResult == MessageBoxResult.Yes)
                {
                    carService.Delete(car);
                    LoadCar();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void TransAddClick(object sender, RoutedEventArgs e)
        {
            var transAddWindow = serviceProvider.GetRequiredService<TransactionManipulateWindow>();
            transAddWindow.IsManipulation = true;
            transAddWindow.ShowDialog();
            LoadTrans();
        }
        private void TransUpdateClick(object sender, RoutedEventArgs e)
        {
            var trans = (RentingTransaction)rentingDatagrid.SelectedItem;
            var transAddWindow = serviceProvider.GetRequiredService<TransactionManipulateWindow>();
            transAddWindow.IsManipulation = true;
            if(trans is null)
            {
                Utils.ErrorAlert("Select row to update");
                return;
            }
            transAddWindow.RentingTransaction = trans;
            transAddWindow.ShowDialog();
            LoadTrans();


        }
        private void TransDeleteClick(object sender, RoutedEventArgs e)
        {
            try
            {
                var trans = (RentingTransaction)rentingDatagrid.SelectedItem;
                
                var confirmResult = Utils.ConfirmationBox("Do you really want to delete this car ?");
                if (confirmResult == MessageBoxResult.Yes)
                {
                    transactionService.DeleteById(trans.RentingTransationId);

                    LoadTrans();
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }

        private void btnSearchCar_Click(object sender, RoutedEventArgs e)
        {
            carDatagrid.ItemsSource = null;
            Cars = new ObservableCollection<CarInformation>();
            carService.SearchCar(txtSearchCar.Text).ToList().ForEach(car => Cars.Add(car));
            carDatagrid.ItemsSource = Cars;
        }
        private void CustomerSearchClick(object sender, RoutedEventArgs e)
        {
            customerDatagrid.ItemsSource = null;
            var type = customerSearchType.SelectedItem as string;
            Customers = new ObservableCollection<Customer>();
            customerService.SearchCustomer(type, txtSearchCustomer.Text).ToList().ForEach(customer => Customers.Add(customer));
            customerDatagrid.ItemsSource = Customers;
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            LogoutHandler?.Invoke();
        }
    }
}
