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
    /// Interaction logic for viewItemsPage.xaml
    /// </summary>
    public partial class viewItemsPage : Page
    {
        DataClasses1DataContext databaseConnection;

        public viewItemsPage(DataClasses1DataContext db)
        {
            InitializeComponent();
            databaseConnection = db;
            var items = databaseConnection.ITEMs.Select(i => new { Name = i.itemName, UPC= i.upc, Price = i.unitPrice, Qty = i.qtyInInventory });
            var gridView = new GridView();
            listView.View = gridView;
            gridView.Columns.Add(new GridViewColumn
            {
                Header = "Name",
                DisplayMemberBinding = new Binding("Name")
            });

            gridView.Columns.Add(new GridViewColumn
            {
                Header = "UPC",
                DisplayMemberBinding = new Binding("UPC")
            });

            gridView.Columns.Add(new GridViewColumn
            {
                Header = "Price",
                DisplayMemberBinding = new Binding("Price")
            });
            gridView.Columns.Add(new GridViewColumn
            {
                Header = "Qty",
                DisplayMemberBinding = new Binding("Qty")
            });



            foreach (var i in items)
            {
                listView.Items.Add(new { Name = i.Name, UPC = i.UPC, Price = i.Price, Qty = i.Qty });

            }
        }



    }
}
