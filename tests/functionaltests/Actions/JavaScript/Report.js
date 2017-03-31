
var tbl = document.getElementById("tbl-rpt");
var row,arrTotal,i,j,ivalue,rowTotal,arrRowTotal;
rowTotal= tbl.getElementsByClassName("RptFooter")[0];
arrTotal =new Array();
arrRowTotal= new Array();
for (j=1;j<rowTotal.cells.length;j++)
	{
		iValue = parseFloat(rowTotal.cells[j].textContent.replace(",","")) * (-1);
                arrTotal.push(iValue );
          
	}

for(i=0;i<tbl.rows.length;i++)
	{	
		irow = tbl.rows[i];
		if (irow.className =='RptTitle' || irow.className =='RptHeader02' ||irow.className =='RptHeader'||irow.className =='RptFooter') continue ;
		for (j=1;j<irow.cells.length;j++)
		{
			iCol= irow.cells[j];
			iValue= parseFloat(iCol.textContent.replace(",",""));
			arrTotal[j-1] = arrTotal[j-1]  + iValue;
                        
		}
		
	}
var isTotalCorrect = 1;
for (j=0;j<arrTotal.length && isTotalCorrect ==1 ;j++)
	{
		if (arrTotal[j] > 0.5 ) isTotalCorrect = 0;
		        
	}
return isTotalCorrect;
