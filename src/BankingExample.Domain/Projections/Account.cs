﻿using BankingExample.Domain.Events;
using System;
using Marten.Events.Aggregation;

namespace BankingExample.Domain.Projections
{
    public class Account 
    {
        public Guid Id { get; set; }
        public string Owner { get; set; }
        public decimal Balance { get; set; }
        public decimal Pending { get; set; }
        public decimal AvailableBalance { get; set; }

        public DateTimeOffset CreatedAt { get; set; }

        public DateTimeOffset UpdatedAt { get; set; }

        public bool HasSufficientFunds(AccountDebited debit)
        {
            var result = (AvailableBalance - debit.Amount) >= 0;
            if (!result)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine($"{Owner} has insufficient funds for debit ({debit.Amount.ToString("C")}): {debit.Description}");
            }
            return result;
        }

        public override string ToString()
        {
            Console.ForegroundColor = ConsoleColor.White;
            return $"{Owner} ({Id}) : {Balance.ToString("C")}";
        }
    }
}
