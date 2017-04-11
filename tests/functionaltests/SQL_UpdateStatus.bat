call sqlcmd -S 10.18.200.206 -U testteam -P @quality# -d CustDB -Q "Update custinfo WITH (ROWLOCK, UPDLOCK)  
Set closed =0, autosuspend=0,enabled=1,disableHorseRacing=0,disableCasino=0,disableoutright=0,disableOpenTote=0,disableBingo=0,DisableNumber=0  
    ,disable151_AU=0,disable152_UK=0,disable153_AU=0,disableLivecasino=0,disableVirtualSports=0, abnormal=0,disableHRFixedOdds=0, DisableGoldDeluxe=0  
where username like 'SuperTestAuto04%'"
