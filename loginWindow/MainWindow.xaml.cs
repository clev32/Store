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
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        DataClasses1DataContext db = new DataClasses1DataContext();
        private void signIn_btn_Click(object sender, RoutedEventArgs e)
        {
            
            String username = username_txt.Text;
            String password = psswrd_box.Password;
            
            String pswrd = db.CUSTOMERs.Where(c => c.username == username).Select(c => c.password).FirstOrDefault();
            
            if (pswrd == password)
            {
                Hide();
                int custID = db.CUSTOMERs.Where(c => c.password == pswrd).Select(c => c.custID).First();

                Window1 main = new Window1(db, custID);
                main.Show();

            }
            else if(pswrd == null)
            {
                MessageBox.Show("Invalid Username");
            }
            else
            {
                MessageBox.Show("Invalid Password");
            }
        }
    }
}
