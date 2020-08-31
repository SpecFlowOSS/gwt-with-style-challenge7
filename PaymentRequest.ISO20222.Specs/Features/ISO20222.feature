Feature: ISO20222 payment requests

  The ISO20222 payment request module is responsible for interpreting
  messages in the [ISO20222 XML](https://www.iso20022.org) and translate
  them to our native payment orders. 

  A single ISO payment message may contain multiple payment information blocks, and each
  can contain multiple transfers. This module should extract all transfers
  and convert them into payment transactions.

  This module needs to validate XML structure and consistency,
  in particular the number of contained transactions and
  the control sum for all transactions in the message,
  and each of the payment information blocks

  IBAN should be translated to BBAN according to
  https://www.ecbs.org/iban/estonia-bank-account-number.html

 
Background:

	Given the following message templates
		| template        | file           |
		| credit transfer | sepa-sct.xml   |
	And the current time is 2020-08-13 14:25:00


Scenario: Single payment, single transfer

	Given the "credit transfer" message with the following header  
		| MsgId | NbOfTxs | CtrlSum | 
		| ID1   |       1 |   22.00 |
	And the message contains a Payment Information block with the following header 
		| PmtMtd | NbOfTxs | CtrlSum|
		| TRF    |       1 |  22.00 |
	And the Payment Information block contains the following transfers
		| CdtrAcct.Id.IBAN      | Amt.InstdAmt | Amt.InstdAmt Ccy |
		| EE382200221020145685  | 22.00        | EUR              |

	When the payment order is generated for the message

	Then the payment order contains the following transactions
		| Account          | Currency | Credit | Settlement Date | External reference |
		| 2200221020145685 | EUR      | 22.00  | 2020-08-13      | ID1-1              |

Scenario: Single payment, multiple transfers

	Given the "credit transfer" message with the following header
		| MsgId | NbOfTxs | CtrlSum | 
		| ID1   |       2 |   50.00 |
	And the message contains a Payment Information block with the following header
		| PmtMtd | NbOfTxs | CtrlSum|
		| TRF    |       2 |  50.00 |
	And the Payment Information block contains the following transfers
		| CdtrAcct.Id.IBAN      | Amt.InstdAmt | Amt.InstdAmt Ccy |
		| EE382200221020145685  | 22.00        | EUR              |
		| EE382200221020145685  | 28.00        | EUR              |

	When the payment order is generated for the message

	Then the payment order contains the following transactions
		| Account          | Currency | Credit | Settlement Date | External reference |
		| 2200221020145685 | EUR      | 22.00  | 2020-08-13      | ID1-1              |
		| 2200221020145685 | EUR      | 28.00  | 2020-08-13      | ID1-2              |
