using Business.Service.Interface;
using Domain.Models;
using LeThanhPhongWPF.Extensions;
using Microsoft.Extensions.DependencyInjection;
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
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace LeThanhPhongWPF.common
{
    /// <summary>
    /// Interaction logic for TransactionDetailViewWindow.xaml
    /// </summary>
    public partial class TransactionDetailViewWindow : Window
    {
        public RentingTransaction RentingTransaction { get; set; }
        public bool IsManipulation { get; set; }
        private IRentingDetailService rentingDetailService;
        private IServiceProvider serviceProvider;
        private ICustomerService customerService;
        private IRentingTransactionService transService;
        private List<RentingDetail> rentingDetails;
        private List<Customer> customers;
        private bool isUpdate;
        public TransactionDetailViewWindow(IRentingDetailService rentingDetailService, IServiceProvider serviceProvider, ICustomerService customerService,   IRentingTransactionService transService)
        {
            InitializeComponent();
            this.rentingDetailService = rentingDetailService;
            this.serviceProvider = serviceProvider;
            this.customerService = customerService;
            this.transService = transService;
            isUpdate = IsManipulation && RentingTransaction is not null;
            Loaded += LoadedHanler;
        }

        private void  LoadedHanler (object sender, RoutedEventArgs e)
        {
            if(RentingTransaction != null)
            {
                detailsGrid.ItemsSource = rentingDetailService.GetRentingDetailsByTransactionId(RentingTransaction.RentingTransationId);
                this.txtId.Text = RentingTransaction.RentingTransationId.ToString();
                this.txtDate.Text = RentingTransaction.RentingDate.ToString();
                this.txtTotalPrice.Text = RentingTransaction.TotalPrice.ToString();
            }else
            {
                //Create mode
                if (IsManipulation) {
                    txtId.Visibility = Visibility.Collapsed;
                    txtDate.Text = DateTime.Now.ToString();
                }
            }
            LoadCustomers();
        }
        private void LoadCustomers() {
            customers = customerService.GetAll().ToList();
        }
        private void ReloadDetailsGrid()
        {
            detailsGrid.ItemsSource = null;
            detailsGrid.ItemsSource = rentingDetails;
            txtTotalPrice.Text =  rentingDetails.Sum((rd) =>
            {
                TimeSpan duration = (TimeSpan)(rd.EndDate - rd.StartDate);
                int numberOfDays = duration.Days + 1;
                return (numberOfDays * rd.Price);
            }).ToString();
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            var rentingWindow = serviceProvider.GetRequiredService<CarRentingWindow>();
            if(rentingDetails is null)
            {
                rentingDetails = new();
            }
            rentingWindow.rentingDetails= rentingDetails;
            rentingWindow.AddCallback =  (rd) => {
                rentingDetails.Add(rd); 
                ReloadDetailsGrid();
            };
            rentingWindow.ShowDialog();
            
        }

        private void btnRemove_Click(object sender, RoutedEventArgs e)
        {
            var selectedRenting = (RentingDetail) detailsGrid.SelectedItem;
            if (selectedRenting != null)
            {
                rentingDetails.Remove(selectedRenting);
                ReloadDetailsGrid();
            }
            else
            {
                Utils.ErrorAlert("Select a row to remove");
            }
        }

        private void SaveClick(object sender, RoutedEventArgs e) {
          
        }
    }
}
