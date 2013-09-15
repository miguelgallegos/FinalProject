using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Concurrent;
using System.Threading;

namespace FinalProject 
{
    [Serializable]
    class CustomerList
    {
        BlockingCollection<Customer> availQueue;
        BlockingCollection<Customer> unAvailQueue;


        public CustomerList()
        {
            availQueue = new BlockingCollection<Customer>();
            unAvailQueue = new BlockingCollection<Customer>();
        }


        public Customer GetRandomCustomer(CancellationToken cToken )
        {
            Customer cust=null;
            try
            {
              
                availQueue.TryTake(out cust, 100, cToken);

                if (cust != null)
                {
                    unAvailQueue.Add(cust);
                }
                
            }
            catch (OperationCanceledException oce)
            {
            }
            return cust;
        }


        public void SetCustomer(Customer cust)
        {

            availQueue.Add(cust);
        }

        public void MakeCustomerAvailable(Customer cust, CancellationToken cToken)
        {
            try
            {
                unAvailQueue.TryTake(out cust, 100, cToken);
                availQueue.Add(cust);
            }
            catch (OperationCanceledException)
            {
            }
            
        }
    }
}
