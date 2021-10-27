Feature: Transfer Money
	Move Money from one account to another

@CO-100
Scenario: Transfer Money Between Accounts
	Given an account for Bob is created with a beginning balance of 250.00
	And and account for Tim is created with a beginning balance of 250.00
	When 50.00 is transfert from Bob to Tim
	And we wait 5 seconds for the transaction to process
	Then the balance for Tim will be 300.00
	And the balance for Bob will be 200.00

