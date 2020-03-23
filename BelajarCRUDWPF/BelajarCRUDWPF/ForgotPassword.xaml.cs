using BelajarCRUDWPF.MyContext;
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
using System.Net;
using System.Net.Mail;
using System.Drawing;
using System.Configuration;
using System.Data.SqlClient;

namespace BelajarCRUDWPF
{
    /// <summary>
    /// Interaction logic for ForgotPassword.xaml
    /// </summary>
    public partial class ForgotPassword : Window
    {
        myContext connection = new myContext();
        public ForgotPassword()
        {
            InitializeComponent();
        }

        //Code for send password button
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var check_email = connection.Users.Where(S => S.Email == Tbx_Email.Text).FirstOrDefault(); //get the data first data from database which match the id
            if (check_email == null)
            {
                MessageBox.Show("Your email is wrong!");
                Tbx_Email.Focus(); //Move cursor to text box Name
                return; //nothing will happen
            }
            var password = Guid.NewGuid().ToString(); //generate password with guid
            check_email.Password = password; // changes the password with new password
            connection.SaveChanges(); 
            MailMessage mm = new MailMessage("projectbootcamp35@gmail.com", check_email.Email);
            string today = DateTime.Now.ToString("dd/MM/yyyy");
            mm.Subject = "Password Recovery (" + today + ")";
            mm.Body = string.Format("Hi {0},<br /><br />Your password has been changed to: <br />{1}<br /><br />Thank You.", check_email.Email, password);
            mm.IsBodyHtml = true;
            SmtpClient smtp = new SmtpClient();
            smtp.Host = "smtp.gmail.com";
            smtp.EnableSsl = true;
            //Definition of sender
            System.Net.NetworkCredential NetworkCred = new System.Net.NetworkCredential();
            NetworkCred.UserName = "projectbootcamp35@gmail.com";
            NetworkCred.Password = "girhoz16!";
            smtp.UseDefaultCredentials = true;
            smtp.Credentials = NetworkCred;
            smtp.Port = 587;
            try
            {
                smtp.Send(mm);
            }
            catch
            {

            }
            MessageBox.Show("New password has been sent to your email!");
            Login login = new Login();
            login.Show();
            this.Close();
        }
    }
}
