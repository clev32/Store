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

namespace DB_APP_STORE
{
    /// <summary>
    /// Interaction logic for ViewBalancePage.xaml
    /// </summary>
    public partial class ViewBalancePage : Page

    {
        DataClasses1DataContext dbConnection;
        int customerID;
        Decimal limit = 1000;
        public ViewBalancePage(DataClasses1DataContext db, int custID)
        {
            InitializeComponent();
            dbConnection = db;
            customerID = custID;
            Decimal currBalance = dbConnection.CUSTOMERs.Where(c => c.custID == custID).Select(c=> c.balance).FirstOrDefault();
            BalanceLbl.Content =  currBalance;
            if(currBalance>= limit)
            {
                BalanceLbl.Foreground = new SolidColorBrush(Colors.Red); ;
            }
          
        }

        private void PaymentBtn_Click(object sender, RoutedEventArgs e)
        {
            decimal amount;
            bool amntEntered = Decimal.TryParse(amnt_txt.Text, out amount);
            if(!amntEntered)
            {
                MessageBox.Show("Please enter valid amount");
            }
            else
            {
                CUSTOMER cust = dbConnection.CUSTOMERs.Where(c => c.custID == customerID).First();
                
                cust.balance -= amount;

                dbConnection.SubmitChanges();
                MessageBox.Show($"Thank you for your payment. Your balance is now {cust.balance}");
            }
        }
    }
}
