using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DragDropBetweenDataGridTreeGrid
{
    public class EmployeeInfo
    {
        #region private variables
        int _id;
        private string _firstName;
        private string _lastName;
        private string _title;
        private double? _salary;
        private int _reportsTo;
        #endregion

        #region public variables

        /// <summary>
        /// Denotes the employee first name
        /// </summary>
        [Display(Name = "First Name")]
        public string FirstName
        {
            get { return _firstName; }
            set { _firstName = value; }
        }

        /// <summary>
        /// Denotes the employee last name
        /// </summary>        
        [Display(Name = "Last Name")]
        public string LastName
        {
            get { return _lastName; }
            set { _lastName = value; }
        }


        /// <summary>
        /// Denotes the employee id
        /// </summary>
        [Display(Name = "ID")]
        public int ID
        {
            get { return _id; }
            set { _id = value; }
        }

        /// <summary>
        /// Denotes the employee title
        /// </summary>
        [Display(Name = "Title")]
        public string Title
        {
            get { return _title; }
            set { _title = value; }
        }

        /// <summary>
        /// Denotes the employee salary
        /// </summary>
        [Display(Name = "Salary")]
        public double? Salary
        {
            get { return _salary; }
            set { _salary = value; }
        }

        /// <summary>
        /// Denotes the employee who has reports to
        /// </summary>
        [Display(Name = "Reports To")]
        public int ReportsTo
        {
            get { return _reportsTo; }
            set { _reportsTo = value; }
        }
        #endregion
    }
}
