using Syncfusion.Data;
using Syncfusion.UI.Xaml.Grid;
using Syncfusion.UI.Xaml.Grid.Cells;
using Syncfusion.UI.Xaml.ScrollAxis;
using Syncfusion.Windows.Shared;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Windows;
using System.Windows.Controls;

namespace SfDataGridDemo
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = new ViewModel();
            //customization the GridNumericColumn
            this.dataGrid.CellRenderers.Remove("Numeric");
            this.dataGrid.CellRenderers.Add("Numeric", new CustomizedGridCellNumericRenderer(dataGrid));
            //customization the GridTextColumn
            this.dataGrid.CellRenderers.Remove("TextBox");
            this.dataGrid.CellRenderers.Add("TextBox", new GridCellTextBoxRendererExt(dataGrid));
        }
    }

    public class GridCellTextBoxRendererExt : GridCellTextBoxRenderer
    {
        RowColumnIndex RowColumnIndex;
        SfDataGrid DataGrid { get; set; }
        string newvalue = null;

        public GridCellTextBoxRendererExt(SfDataGrid dataGrid)
        {
            DataGrid = dataGrid;
        }
        
        public override void OnInitializeEditElement(DataColumnBase dataColumn, TextBox uiElement, object dataContext)
        {
            base.OnInitializeEditElement(dataColumn, uiElement, dataContext);
          
            uiElement.TextChanged += UiElement_TextChanged;
            this.RowColumnIndex.ColumnIndex = dataColumn.ColumnIndex;
            this.RowColumnIndex.RowIndex = dataColumn.RowIndex;
        }           

        private void UiElement_TextChanged(object sender, TextChangedEventArgs e)
        {
            var textbox = (e.OriginalSource as TextBox);
            if (textbox.IsFocused)
            {
                newvalue = (e.OriginalSource as TextBox).Text;
                UpdateValues(this.RowColumnIndex.RowIndex, this.RowColumnIndex.ColumnIndex);
            }
        }     

        private void UpdateValues(int rowIndex, int columnIndex)
        {
            string editEelementText = newvalue;
            columnIndex = this.DataGrid.ResolveToGridVisibleColumnIndex(columnIndex);
            if (columnIndex < 0)
                return;
            var mappingName = DataGrid.Columns[columnIndex].MappingName;
            var recordIndex = this.DataGrid.ResolveToRecordIndex(rowIndex);
            if (recordIndex < 0)
                return;
            if (DataGrid.View.TopLevelGroup != null)
            {
                var record = DataGrid.View.TopLevelGroup.DisplayElements[recordIndex];
                if (!record.IsRecords)
                    return;
                var data = (record as RecordEntry).Data;
                data.GetType().GetProperty(mappingName).SetValue(data, editEelementText);
            }
            else
            {
                var record1 = DataGrid.View.Records.GetItemAt(recordIndex);
                record1.GetType().GetProperty(mappingName).SetValue(record1, editEelementText);
            }
        }
    }

    public class CustomizedGridCellNumericRenderer : GridCellNumericRenderer
    {
        RowColumnIndex RowColumnIndex;
        SfDataGrid DataGrid { get; set; }
        string newvalue = null;

        public CustomizedGridCellNumericRenderer(SfDataGrid dataGrid)
        {
            DataGrid = dataGrid;
        }

        public override void OnInitializeEditElement(DataColumnBase dataColumn, DoubleTextBox uiElement, object dataContext)
        {
            base.OnInitializeEditElement(dataColumn, uiElement, dataContext);
            uiElement.ValueChanging += UiElement_ValueChanging;
            this.RowColumnIndex.ColumnIndex = dataColumn.ColumnIndex;
            this.RowColumnIndex.RowIndex = dataColumn.RowIndex;
        }

        private void UiElement_ValueChanging(object sender, Syncfusion.Windows.Shared.ValueChangingEventArgs e)
        {
            newvalue = e.NewValue.ToString();
            UpdateValues(this.RowColumnIndex.RowIndex, this.RowColumnIndex.ColumnIndex);
        }

        private void UpdateValues(int rowIndex, int columnIndex)
        {
            string editEelementText = newvalue == "0" ? "0" : newvalue;
            columnIndex = this.DataGrid.ResolveToGridVisibleColumnIndex(columnIndex);
            if (columnIndex < 0)
                return;
            var mappingName = DataGrid.Columns[columnIndex].MappingName;
            var recordIndex = this.DataGrid.ResolveToRecordIndex(rowIndex);
            if (recordIndex < 0)
                return;
            if (DataGrid.View.TopLevelGroup != null)
            {
                var record = DataGrid.View.TopLevelGroup.DisplayElements[recordIndex];
                if (!record.IsRecords)
                    return;
                var data = (record as RecordEntry).Data;
                data.GetType().GetProperty(mappingName).SetValue(data, (int.Parse(editEelementText)));
            }
            else
            {
                var record1 = DataGrid.View.Records.GetItemAt(recordIndex);
                record1.GetType().GetProperty(mappingName).SetValue(record1, (int.Parse(editEelementText)));
            }
        }
    }

    public class ViewModel
    {
        private ObservableCollection<OrderInfo> _orders;
        public ObservableCollection<OrderInfo> Orders
        {
            get { return _orders; }
            set { _orders = value; }
        }

        public ViewModel()
        {
            _orders = new ObservableCollection<OrderInfo>();
            this.GenerateOrders();
        }

        private void GenerateOrders()
        {
            _orders.Add(new OrderInfo(1001, "Maria Anders", "Germany", "ALFKI", 25000));
            _orders.Add(new OrderInfo(1002, "Ana Trujilo", "Germany", "ANATR", 36000));
            _orders.Add(new OrderInfo(1003, "Antonio Moreno", "Germany", "ANTON", 40040));
            _orders.Add(new OrderInfo(1004, "Thomas Hardy", "Germany", "AROUT", 10700));
            _orders.Add(new OrderInfo(1005, "Christina Berglund", "Sweden", "BERGS", 20300));
            _orders.Add(new OrderInfo(1006, "Hanna Moos", "Sweden", "BLAUS", 50700));
            _orders.Add(new OrderInfo(1007, "Frederique Citeaux", "Sweden", "BLONP", 80100));
            _orders.Add(new OrderInfo(1008, "Martin Sommer", "Sweden", "BOLID", 35000));
            _orders.Add(new OrderInfo(1009, "Laurence Lebihan", "France", "BONAP", 20030));
            _orders.Add(new OrderInfo(1010, "Elizabeth Lincoln", "France", "BOTTM", 54000));
        }
    }

    public class OrderInfo : INotifyPropertyChanged
    {
        int orderID;
        string customerId;
        string country;
        string customerName;
        int unitPrice;

        [Display(Name = "Order ID")]
        public int OrderID
        {
            get { return orderID; }
            set
            {
                orderID = value;
                OnPropertyChanged("OrderID");
            }
        }

        [Display(Name = "Customer ID")]
        public string CustomerID
        {
            get { return customerId; }
            set
            {
                customerId = value;
                OnPropertyChanged("CustomerID");
            }
        }

        [Display(Name = "Customer Name")]
        public string CustomerName
        {
            get { return customerName; }
            set
            {
                customerName = value;
                OnPropertyChanged("CustomerName");
            }
        }

        [Display(Name = "Country")]
        public string Country
        {
            get { return country; }
            set
            {
                country = value;
                OnPropertyChanged("Country");
            }
        }

        [Display(Name = "Unit Price")]
        public int UnitPrice
        {
            get { return unitPrice; }
            set
            {
                unitPrice = value;
                OnPropertyChanged("UnitPrice");
            }
        }

        public OrderInfo(int orderId, string customerName, string country, string customerId, int unitPrice)
        {
            this.OrderID = orderId;
            this.CustomerName = customerName;
            this.Country = country;
            this.CustomerID = customerId;
            this.UnitPrice = unitPrice;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(string PropertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(PropertyName));
        }
    }
}
