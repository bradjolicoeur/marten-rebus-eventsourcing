Feature: TransferMoney
	Simple calculator for adding two numbers

@mytag
Scenario: Transfer Money Between Accounts
	Given an account for Bob is created with a beginning balance of 250.00
	And and account for Tim is created with a beginning balance of 300.00
	When 50.00 is transfert from Bob to Tim
	Then the balance for Tim will be 350.00
	And the balance for Bob will be 200.00
