using Microsoft.Xaml.Behaviors;
using Syncfusion.Data.Extensions;
using Syncfusion.UI.Xaml.Grid;
using Syncfusion.UI.Xaml.TreeGrid;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DragDropBetweenDataGridTreeGrid
{
   public class DragDropBehavior:Behavior<MainWindow>
    {
       
        protected override void OnAttached()
        {
            base.OnAttached();
            AssociatedObject.sfDataGrid.RowDragDropController.Drop += sfDataGrid_Drop;
            AssociatedObject.sfTreeGrid.RowDragDropController.Drop += sfTreeGrid_Drop;
        }

        /// <summary>
        /// Customized TreeGrid Drop event.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void sfTreeGrid_Drop(object sender, TreeGridRowDropEventArgs e)
        {
            if (e.IsFromOutSideSource)
            {
                //Getting the  dragging record from e.Data
                var draggingRecord = e.Data.GetData("Records") as ObservableCollection<object>;

                var record = draggingRecord[0] as EmployeeInfo;

                //Getting the Drop position
                var dropPosition = e.DropPosition.ToString();

                var newItem = new EmployeeInfo();

                //Getting the Target node index
                var rowIndex = AssociatedObject.sfTreeGrid.ResolveToRowIndex(e.TargetNode.Item);

                if (dropPosition != "None" && rowIndex != -1)
                {
                    if (AssociatedObject.sfTreeGrid.View is TreeGridSelfRelationalView)
                    {
                        var treeNode = e.TargetNode;
                        if (treeNode == null)
                            return;

                        var targetData = treeNode.Item;

                        var dropIndex = -1;

                        TreeNode parentNode = null;

                        IList sourceCollection = null;

                        if (dropPosition == "DropBelow" || dropPosition == "DropAbove")
                        {
                            //Create the new item based on ParentNode and Its ChildPropertyName then get its source collection
                            parentNode = treeNode.ParentNode;
                            if (parentNode == null)
                            {
                                var treeNodeItem = treeNode.Item as EmployeeInfo;
                                newItem = new EmployeeInfo() { FirstName = record.FirstName, LastName = record.LastName, ID = record.ID, Salary = record.Salary, Title = record.Title, ReportsTo = treeNodeItem.ReportsTo };
                                sourceCollection = GetSourceListCollection(AssociatedObject.sfTreeGrid.View.SourceCollection);
                            }
                            else
                            {
                                var parentNodeItems = parentNode.Item as EmployeeInfo;
                                newItem = new EmployeeInfo() { FirstName = record.FirstName, LastName = record.LastName, ID = record.ID, Salary = record.Salary, Title = record.Title, ReportsTo = parentNodeItems.ID };
                                var collection = AssociatedObject.sfTreeGrid.View.GetPropertyAccessProvider().GetValue(treeNode.ParentNode.Item, AssociatedObject.sfTreeGrid.ChildPropertyName) as IEnumerable;
                                sourceCollection = GetSourceListCollection(collection);
                            }

                            //Get the drop index based on DropPosition

                            dropIndex = sourceCollection.IndexOf(targetData);

                            if (dropPosition == "DropBelow")
                            {
                                dropIndex += 1;
                            }
                        }

                        else if (dropPosition == "DropAsChild")
                        {
                            //Expand the parent node if it is not expanded
                            if (!treeNode.IsExpanded)
                                AssociatedObject.sfTreeGrid.ExpandNode(treeNode);

                            //Create the new item based on ParentNode and Its ChildPropertyName then get its source collection
                            parentNode = treeNode;
                            var parentNodeItems = parentNode.Item as EmployeeInfo;
                            newItem = new EmployeeInfo() { FirstName = record.FirstName, LastName = record.LastName, ID = record.ID, Salary = record.Salary, Title = record.Title, ReportsTo = parentNodeItems.ID };

                            var collection = AssociatedObject.sfTreeGrid.View.GetPropertyAccessProvider().GetValue(targetData, AssociatedObject.sfTreeGrid.ChildPropertyName) as IEnumerable;

                            sourceCollection = GetSourceListCollection(collection);

                            if (sourceCollection == null)
                            {
                                var list = targetData.GetType().GetProperty(AssociatedObject.sfTreeGrid.ChildPropertyName).PropertyType.CreateNew() as IList;

                                if (list != null)
                                {
                                    AssociatedObject.sfTreeGrid.View.GetPropertyAccessProvider().SetValue(treeNode.Item, AssociatedObject.sfTreeGrid.ChildPropertyName, list);
                                    sourceCollection = list;
                                }
                            }
                            dropIndex = sourceCollection.Count;
                        }

                        //Insert the new item to the source collection based on DropIndex
                        sourceCollection.Insert(dropIndex, newItem);

                        e.Handled = true;
                    }
                }

                //Remove the record from the source collection
                AssociatedObject.sfDataGrid.View.Remove(record);
            }
        }

      
      
        /// <summary>
        /// Gets the source collection of TreeGrid
        /// </summary>
        /// <param name="collection"></param>
        /// <returns></returns>
        private IList GetSourceListCollection(IEnumerable collection)
        {
            IList list = null;
            if (collection == null)
                collection = AssociatedObject.sfTreeGrid.View.SourceCollection;
            if ((collection as IList) != null)
            {
                list = collection as IList;
            }
            return list;
        }

        /// <summary>
        /// Customize the Drop event.restrict the certain record and Drop position from drop.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void sfDataGrid_Drop(object sender, GridRowDropEventArgs e)
        {
            
            if (e.IsFromOutSideSource)
            {
                //Getting the  dragging record from e.Data
                var draggingRecord = e.Data.GetData("Nodes") as ObservableCollection<TreeNode>;
                
                var record = draggingRecord[0].Item as EmployeeInfo;

                //Getting the Drop Index
                int dropIndex = (int)e.TargetRecord;

              //Getting the Drop position
                var dropPosition = e.DropPosition.ToString();

                //Restrict the certain record from drop
                if (record.Title == "Manager")
                {
                    e.Handled = true;
                    return;
                }


                //Get the Source Collection
                IList collection = null;

                collection = AssociatedObject.sfDataGrid.View.SourceCollection as IList;

                //Insert the new item to the source collection based on DropIndex
                if (dropPosition == "DropAbove")
                {
                    dropIndex--;
                    collection.Insert(dropIndex, record);
                    
                }
                else
                {
                    dropIndex++;
                    collection.Insert(dropIndex, record);
                   
                }

                //Remove the record from the source collection
                AssociatedObject.sfTreeGrid.View.Remove(record);
                e.Handled = true;
            }
        }

        protected override void OnDetaching()
        {
            base.OnDetaching();
           
            AssociatedObject.sfDataGrid.RowDragDropController.Drop -= sfDataGrid_Drop;
            AssociatedObject.sfTreeGrid.RowDragDropController.Drop -= sfTreeGrid_Drop;
        }

    }
}
