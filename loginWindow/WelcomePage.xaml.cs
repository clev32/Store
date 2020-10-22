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
    /// Interaction logic for WeldomePage.xaml
    /// </summary>
    public partial class WelcomePage : Page
    {

        DataClasses1DataContext dbConnection;
        int customerID;

        public WelcomePage(DataClasses1DataContext db, int custID)
        {
            InitializeComponent();
            dbConnection = db;
            customerID = custID;
            String name = dbConnection.PERSONs.Where(p => p.personID == custID).Select(p => p.firstName + " " +  p.lastName).First();
            nameLbl.Content = name;
        }
    }
}
