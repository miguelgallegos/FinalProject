using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Concurrent;
using System.Threading;

namespace FinalProject 
{
    class CustomerList
    {
        BlockingCollection<Customer> availQueue;
        BlockingCollection<Customer> unAvailQueue;


        CustomerList()
        {

            availQueue = new BlockingCollection<Customer>;
            unAvailQueue = new BlockingCollection<Customer>;

        }


        public Customer GetRandomCustomer(CancellationToken cToken )
        {

            Customer cust;
            
            availQueue.TryTake(out cust,100, cToken);

            if (cust != null)
            {
                unAvailQueue.Add(cust);
            }
            return cust;
        }


        public void SetCustomer(Customer cust)
        {

            availQueue.Add(cust);
        }
    }
}
