using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MVC5training_2.Models
{
    public class Purchase
    {

        // Purchase ID
        public int PurchaseId { get; set; }

        // Buyer's full name
        public string Name { get; set; }

        // Buyer's full address
        public string Address { get; set; }

        // Purchased book ID
        public int BookID { get; set; }

        // Purchase Date\Time
        public DateTime Date { get; set; }

    }
}