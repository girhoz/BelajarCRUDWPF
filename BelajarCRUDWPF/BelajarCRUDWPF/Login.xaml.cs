using BelajarCRUDWPF.Model;
using BelajarCRUDWPF.MyContext;
using System;
using System.Collections.Generic;
using System.Data.Common;
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

namespace BelajarCRUDWPF
{
    /// <summary>
    /// Interaction logic for Login.xaml
    /// </summary>
    public partial class Login : Window
    {
        myContext connection = new myContext();
        public Login()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var check_email = connection.Users.Where(S => S.Email == Tbx_Email.Text).FirstOrDefault(); //get the data first data from database which match the id
            if (check_email == null)
            {
                MessageBox.Show("Your email is wrong!");
                Tbx_Email.Focus(); //Move cursor to text box Name
                return; //nothing will happen
            }
            else if (check_email.Password != Tbx_Password.Password.ToString())
            {
                MessageBox.Show("Your password is wrong!");
                Tbx_Password.Focus(); //Move cursor to text box Name
                return; //nothing will happen
            }
            MainWindow main = new MainWindow(check_email.Email);
            main.Show();
            this.Close();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            ForgotPassword forgot = new ForgotPassword();
            forgot.Show();
            this.Close();
        }

    }
}
