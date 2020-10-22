using System;
using System.Collections.Generic;
using System.ComponentModel;
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

namespace DB_APP_STORE
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class Window1 : Window
    {
        DataClasses1DataContext databaseConnection;
        int customerID;

        public Window1(DataClasses1DataContext db, int custID)
        {
            InitializeComponent();
            databaseConnection = db;
            customerID = custID;
            Main.Content = new WelcomePage(db, custID);
        }

        private void ViewItemsItem_Click(object sender, RoutedEventArgs e)
        {

            Main.Content = new viewItemsPage(databaseConnection);
    
        }

        private void MakePurchaseItem_Click(object sender, RoutedEventArgs e)
        {
            Main.Content = new MakePurchasePage(databaseConnection, customerID);
        }

        private void ListPurchasesItem_Click(object sender, RoutedEventArgs e)
        {
            Main.Content = new ViewPurchasesPage(databaseConnection, customerID);
        }

        private void ViewAccountBalance_Click(object sender, RoutedEventArgs e)
        {
            Main.Content = new ViewBalancePage(databaseConnection, customerID);
        }

        private void Window_Closing(object sender, CancelEventArgs e)
        {
            MessageBox.Show("Have a great day!");
            databaseConnection.Dispose();
        }
    }
}
