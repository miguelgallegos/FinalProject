using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace FinalProject
{
    [Serializable]
    class BankQueue
    {
        [NonSerialized]
        private BlockingCollection<Transaction> queue;
        [NonSerialized]
        private CancellationToken cancelToken;

        public BankQueue(CancellationToken ct) {
            this.cancelToken = ct;
            this.queue = new BlockingCollection<Transaction>();
        }

        public void Enqueue(Transaction t)
        {
            queue.Add(t);
        }

        public Transaction Dequeue() {

            Transaction tryer=null;
            try
            {
             tryer= queue.Take(cancelToken);
             return tryer;
            }
            catch (OperationCanceledException)
            {

            }
            return tryer;
        }

    }
}
