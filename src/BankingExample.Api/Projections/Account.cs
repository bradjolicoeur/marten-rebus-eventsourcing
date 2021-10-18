﻿using BankingExample.Api.Events;
using System;

namespace BankingExample.Api.Projections
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

        public void Apply(AccountCreated created)
        {
            Id = created.AccountId;
            Owner = created.Owner;
            Balance = created.StartingBalance;
            AvailableBalance = created.StartingBalance;
            CreatedAt = UpdatedAt = created.CreatedAt;

            Console.ForegroundColor = ConsoleColor.DarkMagenta;
            Console.WriteLine($"Account created for {Owner} with Balance of {Balance.ToString("C")}");
        }

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

        public void Apply(AccountDebited debit)
        {
            debit.Apply(this);
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"Debiting {Owner} ({debit.Amount.ToString("C")}): {debit.Description}");
        }

        public void Apply(AccountCredited credit)
        {
            credit.Apply(this);
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"Crediting {Owner} {credit.Amount.ToString("C")}: {credit.Description}");
        }

        public void Apply(AccountDebitSettled debit)
        {
            debit.Apply(this);
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"Settled Debit {Owner} ({debit.Amount.ToString("C")}): {debit.Description}");
        }

        public void Apply(AccountCreditSettled credit)
        {
            credit.Apply(this);
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"Settled Credit {Owner} {credit.Amount.ToString("C")}: {credit.Description}");
        }

        public override string ToString()
        {
            Console.ForegroundColor = ConsoleColor.White;
            return $"{Owner} ({Id}) : {Balance.ToString("C")}";
        }
    }
}
