# How to update the data values while editing mode in WPF DataGrid (SfDataGrid)? 

## About the sample
This example illustrates how to update the data values while editing mode in [WPF DataGrid](https://www.syncfusion.com/wpf-controls/datagrid) (SfDataGrid)? 

[WPF DataGrid](https://www.syncfusion.com/wpf-controls/datagrid) (SfDataGrid) does not provide the direct support to update the data values while editing. You can update the values while editing by overriding [OnInitializeEditElement](https://help.syncfusion.com/cr/wpf/Syncfusion.UI.Xaml.Grid.Cells.GridCellTextBoxRenderer.html#Syncfusion_UI_Xaml_Grid_Cells_GridCellTextBoxRenderer_OnInitializeEditElement_Syncfusion_UI_Xaml_Grid_DataColumnBase_System_Windows_Controls_TextBox_System_Object_) method and customize the **TextBox.TextChanged** event in [GridCellTextBoxRenderer](https://help.syncfusion.com/cr/wpf/Syncfusion.UI.Xaml.Grid.Cells.GridCellTextBoxRenderer.html) in [WPF DataGrid](https://www.syncfusion.com/wpf-controls/datagrid) (SfDataGrid).

```C#

this.dataGrid.CellRenderers.Remove("TextBox");
this.dataGrid.CellRenderers.Add("TextBox", new GridCellTextBoxRendererExt(dataGrid));

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
}bj

```

[WPF DataGrid](https://www.syncfusion.com/wpf-controls/datagrid) (SfDataGrid) provides support for various built-in column types. Each column has its own properties and renderer for more details please refer the below documentation link.

**Documentation Link:** https://help.syncfusion.com/wpf/datagrid/column-types

![Shows the update the data values while editing in SfDataGrid](UpdateDataValues.gif)
https://github.com/SyncfusionExamples/how-to-maintain-the-group-expanded-states-while-ungrouping-and-grouping-the-columns-at-runtime-in-wp/pull/2

KB article - [How to update the data values while editing mode in WPF DataGrid (SfDataGrid)?](https://www.syncfusion.com/kb/12652/how-to-update-the-data-values-while-editing-mode-in-wpf-datagrid-sfdatagrid)

## Requirements to run the demo
Visual Studio 2015 and above versions
