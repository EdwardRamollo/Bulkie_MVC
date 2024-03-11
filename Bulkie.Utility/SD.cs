using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bulkie.Utility
{
    // We will have all the constants for our website in this class

    /* Based on the roles here, we might have some functionality that we want to turn on and off. */
    public static class SD
    {
        public const string Role_Customer = "Customer"; 
        public const string Role_Company = "Company";
        public const string Role_Admin = "Admin"; 
        public const string Role_Employee = "Employee";

        /* Order Status Constants */
        public const string StatusPending = "Pending";
        public const string StatusApproved = "Approved";
        public const string StatusInProcess = "Processing";
        public const string StatusShipped = "Shipped";
        public const string StatusCancelled = "Cancelled";
        public const string StatusRefunded = "Refunded";

        /* Payment Status Constants */
        public const string PaymentStatusPending = "Pending";
        public const string PaymentStatusApproved = "Approved";
        public const string PaymentStatusDelayedPayment = "ApprovedForDelayedPayment";
        public const string PaymentStatusRejected = "Rejected";

        public const string SessionCart = "SessionShoppingCart";
    }
}
