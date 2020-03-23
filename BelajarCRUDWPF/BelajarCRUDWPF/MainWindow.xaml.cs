using BelajarCRUDWPF.Model;
using BelajarCRUDWPF.MyContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
using System.Net;
using System.Net.Mail;
using System.Drawing;
using System.Configuration;
using System.Data.SqlClient;

namespace BelajarCRUDWPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        myContext connection = new myContext(); //initialize connection to myContext
        int cb_sup;
        string email;
        public MainWindow(string user_email)
        {
            InitializeComponent();
            TB_M_Supplier.ItemsSource = connection.Suppliers.ToList(); // refresh datagrid supplier
            TB_M_Item.ItemsSource = connection.Items.ToList(); // refresh datagrid item
            Cb_Supplier.ItemsSource = connection.Suppliers.ToList(); // refresh combo box
            this.email = user_email;
            Disable_Access();
        }

        private void Disable_Access()
        {
            var check_access = connection.Users.Where(S => S.Email == email).FirstOrDefault();
            if (check_access.Role == "member")
            {
                var tab = Menu.Items[1] as TabItem;
                tab.IsSelected = true;
                tab = Menu.Items[0] as TabItem;
                tab.IsEnabled = false;
            }
            else
            {
                var tab = Menu.Items[0] as TabItem;
                tab.IsEnabled = true;
                tab = Menu.Items[1] as TabItem;
                tab.IsEnabled = true;
            }
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void Clear_Textbox_Supplier()
        {
            Tbx_Id.Clear();
            Tbx_Name.Clear();
            Tbx_Address.Clear();
            Tbx_Email.Clear();
        }

        private void Send_Password(string email, string password)
        {
            MailMessage mm = new MailMessage("projectbootcamp35@gmail.com", email);
            string today = DateTime.Now.ToString("dd/MM/yyyy");
            mm.Subject = "Password New Account ("+today+")";
            mm.Body = string.Format("Hi {0},<br /><br />Your password for your account is: <br />{1}<br /><br />Thank You.", email, password);
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
        }

        // code for insert button
        private void Btn_Insert_Click(object sender, RoutedEventArgs e)
        {
            var check_exist = connection.Users.Where(S => S.Email == Tbx_Email.Text).FirstOrDefault();
            if (Tbx_Name.Text.Trim() == string.Empty) //if textbox name empty
            {
                MessageBox.Show("Please fill supplier Name!");
                Tbx_Name.Focus(); //Move cursor to text box Name
                return; //nothing will happen
            }else if (Tbx_Address.Text.Trim() == string.Empty) // if textbox address empty
            {
                MessageBox.Show("Please fill supplier Address!");
                Tbx_Address.Focus(); //Move cursor to text box Name
                return; //nothing will happen
            }
            else if (Tbx_Email.Text.Trim() == string.Empty) // if textbox email empty
            {
                MessageBox.Show("Please fill supplier Email!");
                Tbx_Address.Focus(); //Move cursor to text box Name
                return; //nothing will happen
            }
            //Validation Character
            else if (System.Text.RegularExpressions.Regex.IsMatch(Tbx_Email.Text, "[^a-zA-Z0-9@.]"))
            {
                MessageBox.Show("Email format wrong!");
                Tbx_Email.Focus();
                return;
            }
            else if (!Tbx_Email.Text.Contains("@gmail.com") && !Tbx_Email.Text.Contains("@yahoo.com"))
            {
                MessageBox.Show("Email format wrong!");
                Tbx_Email.Focus();
                return;
            }
            //Validation Exist
            else if (check_exist != null)
            {
                MessageBox.Show("Email already used!");
                Tbx_Email.Focus();
                return;
            }

            var input = new Supplier(Tbx_Name.Text, Tbx_Address.Text, Tbx_Email.Text); //get user input from textbox
            connection.Suppliers.Add(input); // if not empty then add input
            var success = connection.SaveChanges(); //update the database (add the input to database)
            // Add User
            var password = Guid.NewGuid().ToString(); //generate password with guid
            var input2 = new User(Tbx_Email.Text, password, "member");
            connection.Users.Add(input2);
            connection.SaveChanges();
            Send_Password(Tbx_Email.Text, password); //send password to email
            TB_M_Supplier.ItemsSource = connection.Suppliers.ToList(); //refresh the datagrid 
            MessageBox.Show(success + " Data Successfully Inputted & Password Has Been Sent to Email!");
            Clear_Textbox_Supplier(); //Clear all text box
        }

        // code for delete button
        private void Btn_Delete_Click(object sender, RoutedEventArgs e)
        {
            int Id = Convert.ToInt32(Tbx_Id.Text); //get id from textbox id and convert to int
            var check_id = connection.Suppliers.Where(S => S.Id == Id).FirstOrDefault(); //get the data first data from database which match the id
            connection.Suppliers.Remove(check_id); //remove the data
            if (MessageBox.Show("Are you sure this data will be deleted?", "Confirmation", MessageBoxButton.YesNo) == MessageBoxResult.Yes) //confirmation box
            {
                var success = connection.SaveChanges(); //if yes, then update database
                MessageBox.Show(success + " Data Succesfully Deleted!");
            }
            else
            {
                return; //if no, nothing happen
            }           
            TB_M_Supplier.ItemsSource = connection.Suppliers.ToList(); //refresh datagrid
            Clear_Textbox_Supplier(); //Clear all textbox
        }

        // code for update button
        private void Btn_Update_Click(object sender, RoutedEventArgs e)
        {
            int Id = Convert.ToInt32(Tbx_Id.Text); //get id from textbox id and convert to int
            var check_id = connection.Suppliers.Where(S => S.Id == Id).FirstOrDefault(); //get the data first data from database which match the id
            if (Tbx_Name.Text.Trim() == string.Empty) //if textbox name empty
            {
                MessageBox.Show("Please fill supplier Name!");
                Tbx_Name.Focus(); //Move cursor to text box Name
                return; //nothing will happen
            }
            else if (Tbx_Address.Text.Trim() == string.Empty) // if textbox address empty
            {
                MessageBox.Show("Please fill supplier Address!");
                Tbx_Address.Focus(); //Move cursor to text box Name
                return; //nothing will happen
            }
            check_id.Name = Tbx_Name.Text; //assign the name with value from textbox name
            check_id.Address = Tbx_Address.Text; //assign the address with value from textbox address
            check_id.Email = Tbx_Email.Text;
            var success = connection.SaveChanges(); //if not empty then update the database
            TB_M_Supplier.ItemsSource = connection.Suppliers.ToList(); //refresh datagrid
            MessageBox.Show(success + " Data Successfully Updated!");
            Clear_Textbox_Supplier(); //Clear all textbox
        }

        private void Tbx_Name_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {

        }

        private void Tbx_Id_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {

        }

        private void Tbx_Address_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {

        }

        //Code for datagrid
        private void TB_M_Supplier_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var data = TB_M_Supplier.SelectedItem; //get the data of the selected item
            if (data != null) //if the data not null then
            {
                string Id = (TB_M_Supplier.SelectedCells[0].Column.GetCellContent(data) as TextBlock).Text; //get the id from datagrid
                Tbx_Id.Text = Id; //set textbox id value into the id
                string Name = (TB_M_Supplier.SelectedCells[1].Column.GetCellContent(data) as TextBlock).Text; //get the name from datagrid
                Tbx_Name.Text = Name; //set textbox name value into the name
                string Address = (TB_M_Supplier.SelectedCells[2].Column.GetCellContent(data) as TextBlock).Text; //get the address from datagrid
                Tbx_Address.Text = Address; //set textbox address value into the address
                string Email = (TB_M_Supplier.SelectedCells[3].Column.GetCellContent(data) as TextBlock).Text; //get the address from datagrid
                Tbx_Email.Text = Email; //set textbox address value into the address
            }
        }


        private void Clear_Textbox_Item()
        {
            Tbx_Id_Item.Clear();
            Tbx_Name_Item.Clear();
            Tbx_Price.Clear();
            Tbx_Stock.Clear();
            Cb_Supplier.SelectedValue = null;
        }

        private void Cb_Supplier_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            cb_sup = Convert.ToInt32(Cb_Supplier.SelectedValue.ToString()); // get the id from selected combo box
        }

        private void Btn_Insert_Item_Click(object sender, RoutedEventArgs e)
        {
            var supplierdata = connection.Suppliers.Where(S => S.Id == cb_sup).FirstOrDefault(); //search the supplier data that match the supplier id
            var input = new Item(Tbx_Name_Item.Text, Convert.ToInt32(Tbx_Price.Text), Convert.ToInt32(Tbx_Stock.Text), supplierdata); //get user input from textbox
            if (Tbx_Name_Item.Text.Trim() == string.Empty) //if textbox name empty
            {
                MessageBox.Show("Please fill item Name!");
                Tbx_Name_Item.Focus(); //Move cursor to text box Name
                return; //nothing will happen
            }
            else if (Tbx_Price.Text.Trim() == string.Empty) // if textbox Price empty
            {
                MessageBox.Show("Please fill item Price!");
                Tbx_Price.Focus(); //Move cursor to text box Name
                return; //nothing will happen
            }
            else if (Tbx_Stock.Text.Trim() == string.Empty) // if textbox Stock empty
            {
                MessageBox.Show("Please fill item Stock!");
                Tbx_Stock.Focus(); //Move cursor to text box Name
                return; //nothing will happen
            }
            else if (supplierdata == null)
            {
                MessageBox.Show("Please choose Supplier!");
                Cb_Supplier.Focus(); //Move cursor to text box Name
                return; //nothing will happen
            }
            else if (System.Text.RegularExpressions.Regex.IsMatch(Tbx_Name_Item.Text, "[^a-zA-Z0-9]"))
            {
                MessageBox.Show("Item name format wrong!");
                Tbx_Name_Item.Focus();
                return;
            }
            connection.Items.Add(input); // if not empty then add input
            var success = connection.SaveChanges(); //update the database (add the input to database)
            TB_M_Item.ItemsSource = connection.Items.ToList(); // refresh datagrid item
            Cb_Supplier.ItemsSource = connection.Suppliers.ToList(); //refresh 
            MessageBox.Show(success + " Data Successfully Inputted!");
            Clear_Textbox_Item(); //Clear all text box
        }
        

        private void Btn_Update_Item_Click(object sender, RoutedEventArgs e)
        {
            int Id = Convert.ToInt32(Tbx_Id_Item.Text); //get id from textbox id and convert to int
            var check_id = connection.Items.Where(S => S.Id == Id).FirstOrDefault(); //get the data first data from database which match the id
            var supplierdata = connection.Suppliers.Where(S => S.Id == cb_sup).FirstOrDefault(); //search the supplier data that match the supplier id
            if (Tbx_Name_Item.Text.Trim() == string.Empty) //if textbox name empty
            {
                MessageBox.Show("Please fill your Name!");
                Tbx_Name_Item.Focus(); //Move cursor to text box Name
                return; //nothing will happen
            }
            else if (Tbx_Price.Text.Trim() == string.Empty) // if textbox Price empty
            {
                MessageBox.Show("Please fill item Price!");
                Tbx_Price.Focus(); //Move cursor to text box Name
                return; //nothing will happen
            }
            else if (Tbx_Stock.Text.Trim() == string.Empty) // if textbox Stock empty
            {
                MessageBox.Show("Please fill item Stock!");
                Tbx_Stock.Focus(); //Move cursor to text box Name
                return; //nothing will happen
            }
            else if (supplierdata == null)
            {
                MessageBox.Show("Please choose Supplier!");
                Cb_Supplier.Focus(); //Move cursor to text box Name
                return; //nothing will happen
            }
            check_id.Name = Tbx_Name_Item.Text;
            check_id.Price = Convert.ToInt32(Tbx_Price.Text);
            check_id.Stock = Convert.ToInt32(Tbx_Stock.Text);
            check_id.Supplier = supplierdata;
            var success = connection.SaveChanges(); //update the database (add the input to database)
            TB_M_Item.ItemsSource = connection.Items.ToList(); // refresh datagrid item
            Cb_Supplier.ItemsSource = connection.Suppliers.ToList(); //refresh 
            MessageBox.Show(success + " Data Successfully Inputted!");
            Clear_Textbox_Item(); //Clear all text box
        }

        private void Btn_Delete_Item_Click(object sender, RoutedEventArgs e)
        {
            int Id = Convert.ToInt32(Tbx_Id_Item.Text); //get id from textbox id and convert to int
            var check_id = connection.Items.Where(S => S.Id == Id).FirstOrDefault(); //get the data first data from database which match the id
            connection.Items.Remove(check_id); //remove the data
            if (MessageBox.Show("Are you sure this data will be deleted?", "Confirmation", MessageBoxButton.YesNo) == MessageBoxResult.Yes) //confirmation box
            {
                var success = connection.SaveChanges(); //if yes, then update database
                MessageBox.Show(success + " Data Succesfully Deleted!");
            }
            else
            {
                return; //if no, nothing happen
            }
            TB_M_Item.ItemsSource = connection.Items.ToList(); //refresh datagrid
            Clear_Textbox_Item(); //Clear all textbox
        }

        private void Tbx_Id_Item_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {

        }

        private void Tbx_Name_Item_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {

        }

        private void Tbx_Price_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {

        }

        private void Tbx_Stock_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {

        }

        private void Cb_Supplier_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {

        }

        private void TB_M_Item_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var data = TB_M_Item.SelectedItem; //get the data of the selected item
            if (data != null) //if the data not null then
            {
                string Id = (TB_M_Item.SelectedCells[0].Column.GetCellContent(data) as TextBlock).Text; //get the id from datagrid
                Tbx_Id_Item.Text = Id; //set textbox id value into the id
                string Name = (TB_M_Item.SelectedCells[1].Column.GetCellContent(data) as TextBlock).Text; //get the name from datagrid
                Tbx_Name_Item.Text = Name; //set textbox name value into the name
                string Price = (TB_M_Item.SelectedCells[2].Column.GetCellContent(data) as TextBlock).Text; //get the Price from datagrid
                Tbx_Price.Text = Price; //set textbox address value into the address
                string Stock = (TB_M_Item.SelectedCells[3].Column.GetCellContent(data) as TextBlock).Text; //get the Price from datagrid
                Tbx_Stock.Text = Stock; //set textbox address value into the address
                string Supplier = (TB_M_Item.SelectedCells[4].Column.GetCellContent(data) as TextBlock).Text; //get the Price from datagrid
                Cb_Supplier.Text = Supplier;
            }
        }

        private void Tbx_Name_Item_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void Tbx_Email_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {

        }

        private void Menu_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }

        private void LogOut_Click(object sender, RoutedEventArgs e)
        {
            Login login = new Login();
            login.Show();
            this.Close();
        }

        private void LogOut2_Click(object sender, RoutedEventArgs e)
        {
            Login login = new Login();
            login.Show();
            this.Close();
        }
    }
}
