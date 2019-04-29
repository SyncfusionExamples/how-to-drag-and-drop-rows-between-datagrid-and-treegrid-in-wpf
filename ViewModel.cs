using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DragDropBetweenDataGridTreeGrid
{
    public class ViewModel
    {
        #region Private Variables
        private ObservableCollection<EmployeeInfo> _employeeTreeGrid;
        private ObservableCollection<EmployeeInfo> _employeeDataGrid;
        #endregion

        #region ctr

        /// <summary>
        /// Initializes a new instance of the <see cref="ViewModel"/> class.
        /// </summary>
        public ViewModel()
        {
            this.EmployeeTreeGrid = GetEmployeesDetailsForTreeGrid();
            this.EmployeeDataGrid = GetEmployeeDetailsForDataGrid();

        }

        #endregion

        #region Public properties
        /// <summary>
        /// contains the details of employees for treegrid
        /// </summary>
        public ObservableCollection<EmployeeInfo> EmployeeTreeGrid
        {
            get { return _employeeTreeGrid; }
            set
            {
                _employeeTreeGrid = value;

            }
        }

        /// <summary>
        /// contains the details of employees for datagrid
        /// </summary>
        public ObservableCollection<EmployeeInfo> EmployeeDataGrid
        {
            get { return _employeeDataGrid; }
            set
            {
                _employeeDataGrid = value;

            }
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Get the Employee details  For TreeGrid
        /// </summary>
        /// <returns></returns>
        private ObservableCollection<EmployeeInfo> GetEmployeesDetailsForTreeGrid()
        {
            ObservableCollection<EmployeeInfo> treegridEmployee = new ObservableCollection<EmployeeInfo>();
            treegridEmployee.Add(new EmployeeInfo() { FirstName = "Ferando", LastName = "Joseph", Title = "Management", Salary = 2000000, ReportsTo = -1, ID = 2 });
            treegridEmployee.Add(new EmployeeInfo() { FirstName = "John", LastName = "Adams", Title = "Accounts", Salary = 2000000, ReportsTo = -1, ID = 3 });

            treegridEmployee.Add(new EmployeeInfo() { FirstName = "Andrew", LastName = "Fuller", ID = 9, Salary = 1200000, ReportsTo = 2, Title = "Vice President" });
            treegridEmployee.Add(new EmployeeInfo() { FirstName = "Janet", LastName = "Leverling", ID = 10, Salary = 1000000, ReportsTo = 2, Title = "GM" });
            treegridEmployee.Add(new EmployeeInfo() { FirstName = "Steven", LastName = "Buchanan", ID = 11, Salary = 900000, ReportsTo = 2, Title = "Manager" });

            // Accounts
            treegridEmployee.Add(new EmployeeInfo() { FirstName = "Nancy", LastName = "Davolio", ID = 12, Salary = 850000, ReportsTo = 3, Title = "Accounts Manager" });
            treegridEmployee.Add(new EmployeeInfo() { FirstName = "Margaret", LastName = "Peacock", ID = 13, Salary = 700000, ReportsTo = 3, Title = "Accountant" });
            treegridEmployee.Add(new EmployeeInfo() { FirstName = "Michael", LastName = "Suyama", ID = 14, Salary = 700000, ReportsTo = 3, Title = "Accountant" });
            treegridEmployee.Add(new EmployeeInfo() { FirstName = "Robert", LastName = "King", ID = 15, Salary = 650000, ReportsTo = 3, Title = "Accountant" });

            return treegridEmployee;
        }

        /// <summary>
        /// Get the Employee details  For DataGrid
        /// </summary>
        /// <returns></returns>
        private ObservableCollection<EmployeeInfo> GetEmployeeDetailsForDataGrid()
        {
            ObservableCollection<EmployeeInfo> datagridEmployee = new ObservableCollection<EmployeeInfo>();
            datagridEmployee.Add(new EmployeeInfo() { FirstName = "Susmi", LastName = "Joseph", ID = 800, Title = "Management", Salary = 2000000, ReportsTo = 1,  });
            datagridEmployee.Add(new EmployeeInfo() { FirstName = "Ramya", LastName = "Fuller", ID = 100, Salary = 1200000, ReportsTo = 2, Title = "Vice President" });
            datagridEmployee.Add(new EmployeeInfo() { FirstName = "Priya", LastName = "Leverling", ID = 200, Salary = 1000000, ReportsTo = 2, Title = "GM" });
            datagridEmployee.Add(new EmployeeInfo() { FirstName = "Vicky", LastName = "Buchanan", ID = 300, Salary = 900000, ReportsTo = 8, Title = "Manager" });
            datagridEmployee.Add(new EmployeeInfo() { FirstName = "Nancy", LastName = "Davolio", ID = 400, Salary = 850000, ReportsTo = 4, Title = "Accounts Manager" });
            datagridEmployee.Add(new EmployeeInfo() { FirstName = "Margaret", LastName = "Peacock", ID = 500, Salary = 700000, ReportsTo = 3, Title = "Accountant" });
            datagridEmployee.Add(new EmployeeInfo() { FirstName = "Michael", LastName = "Suyama", ID = 600, Salary = 700000, ReportsTo = 3, Title = "Accountant" });
            datagridEmployee.Add(new EmployeeInfo() { FirstName = "Robert", LastName = "King", ID = 700, Salary = 650000, ReportsTo = 7, Title = "Accountant" });
            return datagridEmployee;
        }
        #endregion
    }
}
