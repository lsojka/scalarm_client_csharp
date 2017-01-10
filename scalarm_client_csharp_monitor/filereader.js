// read nurbs from .txt file

    var readenFile = "nurbsfile goes here";

function extractFile(file, onLoadCallback)
{
    var reader = new FileReader();
    reader.onload = onLoadCallback;
    //Uncaught TypeError: Failed to execute 'readAsText' on 'FileReader': parameter 1 is not of type 'Blob'. (19:08:35:148 | error, javascript)
    reader.readAsText(file);
}

function fileDataToControlPoints(str)
{

    var cPoints = new Array();
    var fRows; 
    var fCols; 
    
    var lineTemp = str.split("\n");
    fRows = lineTemp.length;
    pointStrings = str.split(";");
    
    if( pointStrings[pointStrings.length - 1].length == 0 )
        pointStrings.pop();
    
    fCols = pointStrings.length / fRows;
   
    for(var ii = 0; ii<fRows; ii++)
    {
            cPoints[ii] = new Array();
            for(var jj = 0; jj<fCols; jj++)
            {
                cPoints[ii][jj] = new Point();
            }
    }
    // -------------------------------------------------------------------------
    for(var i = 0; i < pointStrings.length; i++)
    {  
        pointStringsCoords = pointStrings[i].split(" ");
        var inputXYZ = new Array();
        var itoken = 0;
        for(var j = 0; j < pointStringsCoords.length; j++)
        {
            var temp = parseFloat(pointStringsCoords[j]);
            if( (temp < 0) || (temp == 0) || (temp > 0) )
            {
                inputXYZ[itoken] = temp * 50;
                itoken = itoken + 1;
            }
        }
        if(inputXYZ.length > 0/*== 3*/)
        {   // undefined?       
            console.log(" i = " + i + ", j = " + j)
            //console.log()
            cPoints[(i-(i%fCols))/fCols][i%fCols].setXYZ(inputXYZ[0], inputXYZ[1], inputXYZ[2]);       
        }
    }
    console.log("Control points :"+cPoints.length);
    return cPoints;
}

function HHello()
{
    alert(' no hello ');
}