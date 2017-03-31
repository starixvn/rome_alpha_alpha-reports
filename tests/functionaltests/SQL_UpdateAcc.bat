call sqlcmd -S 10.18.200.233 -U testteam -P @quality# -d CustDB -Q "exec QC_Update_AccountInfo @LineIDs='%4'"
