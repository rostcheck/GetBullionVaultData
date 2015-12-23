GetBullionVaultData - utility program to download historical data from BullionVault.
BullionVault does not provide a sufficient spreadsheet download within the UI so
this command-line program uses its (rich) API. 

When run, the utility will ask for your BullionVault username and password, then 
download all transactions from 1/1/2006 to the present and save them to a file 
named BullionVault.txt.