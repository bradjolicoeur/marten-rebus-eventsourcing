Feature: Transfer Money Overdraft
	Make sure overdraft does not transfer money

@co-101
Scenario: Transfer Money with Overdraft
	Given an account for Sam is created with a beginning balance of 250.00
	And and account for Ralph is created with a beginning balance of 20.00
	When 50.00 is transfert from Ralph to Sam
	And we wait 5 seconds for the transaction to process
	Then the balance for Sam will be 250.00
	And the balance for Ralph will be 20.00
	And Ralphs ledger will include an overdraft event