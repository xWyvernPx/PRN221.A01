using Business.Service.Interface;
using LeThanhPhongWPF.admin;
using LeThanhPhongWPF.customer;
using LeThanhPhongWPF.Extensions;
using LeThanhPhongWPF.State;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
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

namespace LeThanhPhongWPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly ICustomerService _customerService;
        public MainWindow(ICustomerService customerService)
        {
            InitializeComponent();
            _customerService = customerService;
        }
        

        private (string,string) RetriveAdminAccount()
        {
            var config = new ConfigurationBuilder()
                          .SetBasePath(Directory.GetCurrentDirectory())
                          .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true).Build();
            string adminEmail = config.GetSection("Admin:Email").Value;
            string adminPassword = config.GetSection("Admin:Password").Value;
            return (adminEmail, adminPassword);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var email = txtEmail.Text;
            var password = txtPassword.Password;
            var (adminEmail, adminPassword) = RetriveAdminAccount();
            try
            {
                email.CheckEmail("Email");
                password.CheckNotEmpty("password");
                

                if(email.Equals(adminEmail) && password.Equals(adminPassword))
                {
                    var adminWindow = ((IServiceProvider)this.Tag).GetService<AdminWindow>();
                    adminWindow.Tag = this.Tag;
                    adminWindow.LogoutHandler = () =>
                    {
                        txtEmail.Text = "";
                        txtPassword.Password = "";
                        adminWindow.Close();
                        this.Show();
                    };
                    this.Hide();
                    adminWindow.Show();
                }else{   
                    var user = _customerService.CheckAuth(email, password);
                    if(user != null)
                    {
                        if(user.CustomerStatus < 1)
                        {
                            throw new Exception("This account has been banned");
                        }
                        var customerWindow = ((IServiceProvider)this.Tag).GetService<CustomerWindow>();
                        AppState.CustomerInformation = user;
                        customerWindow.LogoutHandler = () =>
                        {
                            AppState.CustomerInformation = null;
                            txtEmail.Text = "";
                            txtPassword.Password = "";
                            customerWindow.Close();
                            this.Show();
                        };
                        this.Hide();
                        customerWindow.Show();
                    }else
                    {
                        throw new Exception("Email or password is not valid");
                    }
                }

            }
            catch (Exception ex)
            {
                Utils.ErrorAlert(ex.Message);
            }
        }
    }
}
