using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace FinalProject
{
    class BankVault
    {

        private decimal bankBalance;
        private object vaultLock;

        public BankVault(decimal bankBalance)
        {
            this.bankBalance = bankBalance;
            this.vaultLock = new object();
        }

        public decimal Deposit(decimal amount)
        {
            lock (vaultLock) {
                bankBalance += amount;
                return bankBalance;
            }
            
        }
        
        //stops sim if false
        public bool Withdraw(decimal amount, decimal v)
        {
            lock (vaultLock) {

                if (amount <= bankBalance)
                {
                    bankBalance -= amount;
                    return true;
                }
                else
                {
                    return false;
                }
            }

        }

    }
}
