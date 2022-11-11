# How to make drag and drop between the WPF DataGrid (SfDataGrid) with other controls 

[WPF DataGrid](https://www.syncfusion.com/wpf-controls/datagrid) (SfDataGrid) offers a built-in support for row drag and drop functionality by enabling <b>SfDataGrid.AllowDraggingRows</b> and [AllowDrop](https://learn.microsoft.com/en-us/dotnet/api/system.windows.uielement.allowdrop?view=windowsdesktop-7.0) property. This topic demonstrates row drag-and-drop operations between SfDataGrid with other controls.

```
<syncfusion:SfDataGrid x:Name="dataGrid" 
                        AllowDraggingRows="True"
                        AllowDrop="True"
                        LiveDataUpdateMode="AllowDataShaping"
                        ItemsSource="{Binding UserDetails}" >
```

## Row drag and drop between different controls

You can be able to perform the row drag and drop between SfDataGrid with other control like [ListView](https://docs.microsoft.com/en-us/dotnet/framework/wpf/controls/listview-overview) and others also. For this operation, you should override a <b>RowDragDropController.ProcessOnDragOver and RowDragDropController.ProcessOnDrop</b> to handle the drag and drop operations.

```
this.sfDataGrid.RowDragDropController = new GridRowDragDropControllerExt();
 
public class GridRowDragDropControllerExt : GridRowDragDropController
{
    ObservableCollection<object> draggingRecords = new ObservableCollection<object>();
 
    /// <summary>
    /// Occurs when the input system reports an underlying dragover event with this element as the potential drop target.
    /// </summary>
    /// <param name="args">An <see cref="T:Windows.UI.Xaml.DragEventArgs">DragEventArgs</see> that contains the event data.</param>
    /// <param name="rowColumnIndex">Specifies the row column index based on the mouse point.</param>
    protected override void ProcessOnDragOver(DragEventArgs args, RowColumnIndex rowColumnIndex)
    {
        if (args.Data.GetDataPresent("ListViewRecords"))
            draggingRecords = args.Data.GetData("ListViewRecords") as ObservableCollection<object>;
        else
            draggingRecords = args.Data.GetData("Records") as ObservableCollection<object>;
 
        if (draggingRecords == null)
            return;
 
        //To get the dropping position of the record
        var dropPosition = GetDropPosition(args, rowColumnIndex, draggingRecords);
 
        //To Show the draggable popup with the DropAbove/DropBelow message
        ShowDragDropPopup(dropPosition, draggingRecords, args);
        //To Show the up and down indicators while dragging the row
        ShowDragIndicators(dropPosition, rowColumnIndex, args);
 
        args.Handled = true;
    }
 
    ListView listview;
 
    /// <summary>
    /// Occurs when the input system reports an underlying drop event with this element as the drop target.
    /// </summary>
    /// <param name="args">An <see cref="T:Windows.UI.Xaml.DragEventArgs">DragEventArgs</see> that contains the event data.</param>
    /// <param name="rowColumnIndex">Specifies the row column index based on the mouse point.</param>
    protected override void ProcessOnDrop(DragEventArgs args, RowColumnIndex rowColumnIndex)
    {
        if (args.Data.GetDataPresent("ListView"))
            listview = args.Data.GetData("ListView") as ListView;
 
        if (!DataGrid.SelectionController.CurrentCellManager.CheckValidationAndEndEdit())
            return;
 
        //To get the dropping position of the record
        var dropPosition = GetDropPosition(args, rowColumnIndex, draggingRecords);
        if (dropPosition == DropPosition.None)
            return;
 
        // to get the index of dropping record
        var droppingRecordIndex = this.DataGrid.ResolveToRecordIndex(rowColumnIndex.RowIndex);
 
        if (droppingRecordIndex < 0)
            return;
 
        // to insert the dragged records based on dragged position
        foreach (var record in draggingRecords)
        {
            if (listview != null)
            {
                (listview.ItemsSource as ObservableCollection<Orders>).Remove(record as Orders);
                var sourceCollection = this.DataGrid.View.SourceCollection as IList;
 
                if(dropPosition==DropPosition.DropBelow)
                    sourceCollection.Insert(droppingRecordIndex+1, record);
                else
                    sourceCollection.Insert(droppingRecordIndex, record);
            }
            else
            {
                var draggingIndex = this.DataGrid.ResolveToRowIndex(draggingRecords[0]);
 
                if (draggingIndex < 0)
                {
                    return;
                }
 
                // to get the index of dragging row
                var recordindex = this.DataGrid.ResolveToRecordIndex(draggingIndex);
                // to ger the record based on index
                var recordEntry = this.DataGrid.View.Records[recordindex];
                this.DataGrid.View.Records.Remove(recordEntry);
 
                // to insert the dragged records to particular position
                if (draggingIndex < rowColumnIndex.RowIndex && dropPosition == DropPosition.DropAbove)
                    this.DataGrid.View.Records.Insert(droppingRecordIndex - 1, this.DataGrid.View.Records.CreateRecord(record));
                else if (draggingIndex > rowColumnIndex.RowIndex && dropPosition == DropPosition.DropBelow)
                    this.DataGrid.View.Records.Insert(droppingRecordIndex + 1, this.DataGrid.View.Records.CreateRecord(record));
                else
                    this.DataGrid.View.Records.Insert(droppingRecordIndex, this.DataGrid.View.Records.CreateRecord(record));
            }
        }
 
        //Closes the Drag arrow indication all the rows
        CloseDragIndicators();
        //Closes the Drag arrow indication all the rows
        CloseDraggablePopUp();
    }
}
```
You should set the <b>AllowDrop</b> property of the ListView as <b>true</b> while doing the drag and drop operation from SfDataGrid with ListView and wire the <b>PreviewMouseMove, DragOver and Drop</b> events of ListView to handle the row drag and drop operation.

```
<ListView x:Name="listView" AllowDrop="True" ItemsSource="{Binding OrderDetails1}"
          DisplayMemberPath="ShipName" >
```
By the <b>PreviewMouseMove</b> event you can set the specified <b>DragDropEffects</b> by using [DragAndDrop](https://docs.microsoft.com/en-us/dotnet/desktop/wpf/advanced/drag-and-drop-overview?view=netframeworkdesktop-4.8) as illustrated in the following code example.

```
this.listView.PreviewMouseMove += ListView_PreviewMouseMove;
this.listView.DragOver += ListView_DragOver;
 
/// <summary>
/// to select and dragged the record from ListView to other control
/// </summary>
/// <param name="sender"></param>
/// <param name="e"></param>
private void ListView_PreviewMouseMove(object sender, MouseEventArgs e)
{
    if (e.LeftButton == MouseButtonState.Pressed)
    {
        ListBox dragSource = null;
        var records = new ObservableCollection<object>();
        ListBox parent = (ListBox)sender;
        dragSource = parent;
        object data = GetDataFromListBox(dragSource, e.GetPosition(parent));
 
        records.Add(data);
 
        var dataObject = new DataObject();
        dataObject.SetData("ListViewRecords", records);
        dataObject.SetData("ListView", listView);
 
        if (data != null)
        {
            DragDrop.DoDragDrop(parent, dataObject, DragDropEffects.Move);
        }
    }
    e.Handled = true;
}
 
 
/// <summary>
/// to move the dragged items form the ListView control
/// </summary>
/// <param name="sender"></param>
/// <param name="e"></param>
private void ListView_DragOver(object sender, DragEventArgs e)
{
    if (e.Data.GetDataPresent("Records"))
        records = e.Data.GetData("Records") as ObservableCollection<object>;
}
```
By handling the <b>Drop</b> event you can drop those items in to the ListView.

```
this.listView.Drop += ListView_Drop;
 
/// <summary>
/// to add the dropped records in the ListView control
/// </summary>
/// <param name="sender"></param>
/// <param name="e"></param>
private void ListView_Drop(object sender, DragEventArgs e)
{
    foreach (var item in records)
    {
        this.sfDataGrid.View.Remove(item as Orders);
 
        (this.DataContext as ViewModel).OrderDetails1.Add(item as Orders);
    }
}
```