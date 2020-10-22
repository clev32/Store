using System;
using System.Collections.Generic;
using System.Data.SqlClient;
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
    /// Interaction logic for MakePurchasePage.xaml
    /// </summary>

    public partial class MakePurchasePage : Page
    {
        DataClasses1DataContext dbConnection;
        int customerID;
        bool wasChecked = false;
        CUSTOMER cust;
        Decimal limit = 1000;
        Boolean allowed = false;

        public MakePurchasePage(DataClasses1DataContext db, int custID)
        {
            InitializeComponent();
            dbConnection = db;
            customerID = custID;
            cust = dbConnection.CUSTOMERs.Where(c => c.custID == customerID).First();
            if(cust.balance >= limit)
            {
                MessageBox.Show("Sorry your balance exceeds the limit. Please make a payment before purchasing items");
            }
            else
            {
                allowed = true;
            }
            var upcs = dbConnection.ITEMs.Select(i => i.upc);
            foreach(var i in upcs) {
                upcDrpDown.Items.Add(i);
             }
           

        }

        private void existingOrderChkBox_Checked(object sender, RoutedEventArgs e)
        {
            if (!wasChecked)
            {
                OrderIDLbl.Content = "Choose from the following orders:";
                var existingOrders = dbConnection.SALES_ORDERs.Where(c => c.custId == customerID).Select(c => c.salesOrderID);
                foreach (var order in existingOrders)
                {
                    ordersDrpDown.Items.Add(order);
                }
                ordersDrpDown.Visibility = Visibility.Visible;

                wasChecked = true;
            }
        }

        private void upBtn_Click(object sender, RoutedEventArgs e)
        {
            int num = int.Parse(qty_txt.Text);
            num++;
            qty_txt.Text = num.ToString();
        }

        private void dwnBtn_Click(object sender, RoutedEventArgs e)
        {
            int num = int.Parse(qty_txt.Text);
            if(num > 1)
            {
                num--;
            }
            qty_txt.Text = num.ToString();

        }

        private void completePurchaseBtn_Click(object sender, RoutedEventArgs e)
        {
            if (allowed == true)
            {
                if (wasChecked)
                {
                    try
                    {
                        int salesOrderID = int.Parse(ordersDrpDown.SelectedItem.ToString());
                        long upc = long.Parse(upcDrpDown.SelectedItem.ToString());
                        int qty = int.Parse(qty_txt.Text);

                        dbConnection.usp_addASalesOrderItem(salesOrderID, upc, qty);
                        Decimal itemCost = dbConnection.ITEMs.Where(i => i.upc == upc).Select(i => i.unitPrice).FirstOrDefault();
                        Decimal purchaseCost = itemCost * qty;

                        cust.balance += purchaseCost;

                        dbConnection.SubmitChanges();
                        MessageBox.Show("Item added to purchase");

                
                    }
                    catch (NullReferenceException ex)
                    {
                        MessageBox.Show("Please fill out all fields");
                    }

                    catch (SqlException sqle)
                    {
                        MessageBox.Show("Looks like you already have this item in your order or the item is out of stock");

                    }

                }
                else
                {
                    try
                    {
                        long upc = long.Parse(upcDrpDown.SelectedItem.ToString());
                        int qty = int.Parse(qty_txt.Text);
                        

                        SALES_ORDER newOrder = new SALES_ORDER();
                        newOrder.cashierId = 3;
                        newOrder.custId = customerID;
                        newOrder.minPurchaseForDiscount = 15;
                        dbConnection.SALES_ORDERs.InsertOnSubmit(newOrder);
                        dbConnection.SubmitChanges();

                        int orderID = newOrder.salesOrderID;

                        dbConnection.usp_addASalesOrderItem(orderID, upc, qty);

                        Decimal itemCost = dbConnection.ITEMs.Where(i => i.upc == upc).Select(i => i.unitPrice).FirstOrDefault();
                        Decimal purchaseCost = itemCost * qty;

                        cust.balance += purchaseCost;

                        dbConnection.SubmitChanges();

                    }
                    catch (NullReferenceException ex)
                    {
                        MessageBox.Show("Please fill out all fields");
                    }

                    catch(SqlException sqle)
                    {
                        MessageBox.Show("Something went wrong");
                    }
                }



            }
            else
            {
                MessageBox.Show("Please pay your balance");
            }
        }
    }
}
