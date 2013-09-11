using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace FinalProject
{
    class BankQueue
    {

        private BlockingCollection<Transaction> queue;
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
            return queue.Take(cancelToken);
        }

    }
}
