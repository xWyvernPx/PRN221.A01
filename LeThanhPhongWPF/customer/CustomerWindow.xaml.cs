using Business.Service.Interface;
using Domain.Models;
using LeThanhPhongWPF.common;
using LeThanhPhongWPF.Extensions;
using LeThanhPhongWPF.State;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

namespace LeThanhPhongWPF.customer
{
    /// <summary>
    /// Interaction logic for CustomerWindow.xaml
    /// </summary>
    public partial class CustomerWindow : Window {
        private Customer customer;
        private IServiceProvider serviceProvider;
        private ICustomerService customerService;
        private IRentingTransactionService transactionService;
        private ObservableCollection<RentingTransaction> RentingTransactions { get; set; } = new();


        public CustomerWindow(IServiceProvider serviceProvider, ICustomerService customerService, IRentingTransactionService transService) {
            InitializeComponent();
            Loaded += OnLoadedHandler;
            this.serviceProvider = serviceProvider;
            this.customerService = customerService;
            this.transactionService = transService;
        }
        private void ReloadProfile() {
            var user = customerService.GetCustomerByEmail(customer.Email);
            if (user is not null)
            {
                this.customer = AppState.CustomerInformation = user;
                this.txtName.Text = customer.CustomerName;
                this.txtEmail.Text = customer.Email;
                this.txtPhone.Text = customer.Telephone;
                this.txtPassword.Password = customer.Password;
                this.dob.Text = customer.CustomerBirthday.ToString();
            }
        }
        private void OnLoadedHandler(object sender, RoutedEventArgs e) {
            if (AppState.CustomerInformation is not null) {
                customer = AppState.CustomerInformation as Customer;
            }
            transactionService.GetAll().ToList().ForEach(t=>RentingTransactions.Add(t));
                transDatagrid.ItemsSource = RentingTransactions;
            if(customer is not null) {
                this.txtName.Text = customer.CustomerName;
                this.txtEmail.Text = customer.Email;
                this.txtPhone.Text = customer.Telephone;
                this.txtPassword.Password = customer.Password;
                this.dob.Text = customer.CustomerBirthday.ToString();
            }
        }
        private void UpdateClickHandler(object sender, RoutedEventArgs e) {
            try {
                var editCustomer = serviceProvider.GetRequiredService<CustomerManipulateWindow>();
                editCustomer.CustomerInformation = customer;
                var result = editCustomer.ShowDialog() ?? false;
                if (result) {
                    ReloadProfile();
                }
            }
            catch (Exception ex) {

                Utils.ErrorAlert(ex.Message);
            }
        }
        private void LogoutClickHandler(object sender, RoutedEventArgs e) {
            AppState.CustomerInformation = null;
        }
        private void TabItem_GotFocus(object sender, RoutedEventArgs e)
        {
            //MessageBox.Show("Focus 1");
        }

        private void TabItem_GotFocus_1(object sender, RoutedEventArgs e)
        {
            //MessageBox.Show("Focus 2");
        }

        private void ViewDetailClick(object sender, RoutedEventArgs e) {
            var trans = (RentingTransaction)transDatagrid.SelectedItem;
            var transViewDetailWindow = serviceProvider.GetRequiredService<TransactionDetailViewWindow>();
            transViewDetailWindow.RentingTransaction = trans;
            transViewDetailWindow.ShowDialog();
        }
    }
}
