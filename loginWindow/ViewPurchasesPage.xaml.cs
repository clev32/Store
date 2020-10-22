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
    /// Interaction logic for ViewPurchasesPage.xaml
    /// </summary>
    public partial class ViewPurchasesPage : Page
    {

        DataClasses1DataContext dbConnection;
        int customerID;
        public ViewPurchasesPage(DataClasses1DataContext db, int custID)
        {
            InitializeComponent();
            dbConnection = db;
            customerID = custID;

            var orders = dbConnection.SALES_ORDERs.Where(c => c.custId == customerID).Select(so=> new {OrderID = so.salesOrderID, date = so.dateOfSale, total = so.totalSale });
            var details = dbConnection.SALES_ORDER_DETAILs.Join(orders, sod => sod.salesOrderID, so => so.OrderID,
                (sod, so) => new {UPC= sod.upc, price = sod.unitPrice, qty= sod.qtySold, orderID= sod.salesOrderID});


            displayOrders(orders, details);
         
         

        }

        private void filterBtn_Click(object sender, RoutedEventArgs e)
        {

            DateTime strtDate = startDate.SelectedDate.GetValueOrDefault();
            DateTime end = endDate.SelectedDate.GetValueOrDefault();
        
            decimal strtPrice;
            decimal endPrice;
            Boolean hasStartPrice = Decimal.TryParse(startPrice_txt.Text, out  strtPrice);
            Boolean hasEndPrice = Decimal.TryParse(endPrice_txt.Text, out endPrice);

            if (hasStartPrice && hasEndPrice && strtDate != default(DateTime) && end != default(DateTime))
            {
         
                var orders = dbConnection.SALES_ORDERs.Where((so) => so.custId == customerID && so.dateOfSale >= strtDate && so.dateOfSale <= end
                && so.totalSale >= strtPrice && so.totalSale <= endPrice ).Select(so => new { OrderID = so.salesOrderID, date = so.dateOfSale, total = so.totalSale });
                var details = dbConnection.SALES_ORDER_DETAILs.Join(orders, sod => sod.salesOrderID, so => so.OrderID,
                    (sod, so) => new { UPC = sod.upc, price = sod.unitPrice, qty = sod.qtySold, orderID = sod.salesOrderID });


                displayOrders(orders, details);

            }

            else if(hasStartPrice && hasEndPrice)
            {
            
                var orders = dbConnection.SALES_ORDERs.Where((so) => so.custId == customerID && so.totalSale >= strtPrice && so.totalSale <= endPrice)
                    .Select(so => new { OrderID = so.salesOrderID, date = so.dateOfSale, total = so.totalSale });
                var details = dbConnection.SALES_ORDER_DETAILs.Join(orders, sod => sod.salesOrderID, sod => sod.OrderID,
               (sod, so) => new { UPC = sod.upc, price = sod.unitPrice, qty = sod.qtySold, orderID = sod.salesOrderID });


                displayOrders(orders, details);
            }

            else if(strtDate != default(DateTime) && end != default(DateTime))
            {
            
                var orders = dbConnection.SALES_ORDERs.Where((so) => so.custId == customerID && so.dateOfSale >= strtDate && so.dateOfSale <= end)
                .Select(so => new { OrderID = so.salesOrderID, date = so.dateOfSale, total = so.totalSale });
                var details = dbConnection.SALES_ORDER_DETAILs.Join(orders, sod => sod.salesOrderID, sod => sod.OrderID,
               (sod, so) => new { UPC = sod.upc, price = sod.unitPrice, qty = sod.qtySold, orderID = sod.salesOrderID });


                displayOrders(orders, details);
            }

        }

        private void displayOrders(IQueryable<dynamic> orders, IQueryable<dynamic> details)
        {
              treeView.Items.Clear();

            foreach (var Order in orders)
            {

                TreeViewItem Parent = new TreeViewItem();

                Parent.Header = "Sales Order: " + Order.OrderID + " Order Date: " + Order.date + " Total: " + Order.total;


                foreach (var detail in details)
                {
                    if (detail.orderID == Order.OrderID)
                    {
                        TreeViewItem Child = new TreeViewItem();
                        Child.Header = " Item: " + detail.UPC + " Qty: " + detail.qty + " Price: " + detail.price;
                        Parent.Items.Add(Child);
                    }
                }

                treeView.Items.Add(Parent);
            }
        }
    }
}
