/* 
ControlPoints TO SurfacePoints
 */

function ControlPointsToSurfacePoints(controlPoints, discretizationPointsCount/* KnotVectorType.PERIODIC*/)
{
    var knotVectorType = "PERIODIC";
    var controlPointsRowsCount = controlPoints.length;
    var controlPointsColsCount = controlPoints[0].length;
    
    var basisFunctionOrder_e = 2;
    var basisFunctionOrder_n = 2;
    
    var basisFunctionsCount_e = controlPointsColsCount + basisFunctionOrder_e;
    var basisFunctionsCount_n = controlPointsRowsCount;
    
    // knots
    var knotsVector_e = CalculateArrayOfKnots(controlPointsColsCount, basisFunctionOrder_e, "PERIODIC");
    var knotsVector_n = CalculateArrayOfKnots(controlPointsRowsCount, basisFunctionOrder_n, "OPEN");
    
    // discretization
    var eDiscretizationPointsCount = discretizationPointsCount;
    var nDiscretizationPointsCount = discretizationPointsCount;
    
    var discretizationPoints_e = KnotsVectorDiscretization(knotsVector_e, 
                "PERIODIC", basisFunctionOrder_e, eDiscretizationPointsCount);
                
    var discretizationPoints_n = KnotsVectorDiscretization(knotsVector_n, 
                "OPEN", basisFunctionOrder_n, nDiscretizationPointsCount);
    
    // wartości dla discretization points
    var resultPointsCount = eDiscretizationPointsCount * nDiscretizationPointsCount; // not needed after all
    var result = new Array(resultPointsCount); // 1D array
    
    for(var i = 0; i < nDiscretizationPointsCount; ++i)
    {
        for(var j = 0; j<eDiscretizationPointsCount; ++j)
        {
            var tmp = 0;
            var x_tmp = 0;
            var y_tmp = 0;
            var z_tmp = 0;
            
            for(var nn = 0; nn < basisFunctionsCount_n; ++nn)
            {
                var N_n = CoxDeBoor(discretizationPoints_n[i], nn, basisFunctionOrder_n, basisFunctionsCount_n, knotsVector_n);
                
                var ee;
                for(ee = 0; ee < controlPointsColsCount; ++ee)
                {
                    var N_e = CoxDeBoor(discretizationPoints_e[j], ee, basisFunctionOrder_e, basisFunctionsCount_e, knotsVector_e);
                    
                    x_tmp += N_e * N_n * controlPoints[nn][ee].x;
                    y_tmp += N_e * N_n * controlPoints[nn][ee].y;
                    z_tmp += N_e * N_n * controlPoints[nn][ee].z;
                    tmp += N_e * N_n;
                }
                
                var controlPointIndex = 0;
                for(; ee < basisFunctionsCount_e; ++ee, ++controlPointIndex)
                {
                    var N_e = CoxDeBoor(discretizationPoints_e[j], ee, basisFunctionOrder_e, basisFunctionsCount_e, knotsVector_e);
                    
                    x_tmp += N_e * N_n * controlPoints[nn][controlPointIndex].x;
                    y_tmp += N_e * N_n * controlPoints[nn][controlPointIndex].y;
                    z_tmp += N_e * N_n * controlPoints[nn][controlPointIndex].z;
                    tmp += N_e * N_n;
                }
            }
            
            if(i == nDiscretizationPointsCount - 1)
            {
                result[i * eDiscretizationPointsCount + j] = new Point();
                result[i * eDiscretizationPointsCount + j].setXYZ(controlPoints[controlPointsRowsCount - 1][0].x, 
                                                                  controlPoints[controlPointsRowsCount - 1][0].y,
                                                                  controlPoints[controlPointsRowsCount - 1][0].z);
            }
            else
            {
                result[i * eDiscretizationPointsCount + j] = new Point();
                result[i * eDiscretizationPointsCount + j].setXYZ((x_tmp / tmp), (y_tmp / tmp), (z_tmp / tmp));
            }
        } 
    } 
    
    return result;
}

function CalculateArrayOfKnots(controlPointsCount, basisFunctionOrder, knotVectorType) // [x]
{
    if(knotVectorType == "PERIODIC")
        controlPointsCount += basisFunctionOrder;
    
    var resultKnotsCount = controlPointsCount + basisFunctionOrder + 1;
    var knotVector = new Array();
    
    if(knotVectorType == "PERIODIC")
    {
        for(var i = 0; i < resultKnotsCount; i++)
            knotVector[i] = i;
    }
    // else -> not supported
    else
    {
        var knotValue = 0;
        for(var i = 0; i < resultKnotsCount; i++)
        {
            if(i > basisFunctionOrder && i < (resultKnotsCount - basisFunctionOrder))
                ++knotValue;
            
            knotVector[i] = knotValue;
        }
    }
    return knotVector;
}

function KnotsVectorDiscretization(knotVector, knotVectorType,
                                  basisFunctionOrder, discretizationPointsCount)
{
    var offset;
    var tmpDiscretizationPointsCount;
    if(knotVectorType == "OPEN")
    {
        offset = 0;
        tmpDiscretizationPointsCount = discretizationPointsCount;
    }
    else if(knotVectorType == "PERIODIC")
    {
        offset = basisFunctionOrder;
        tmpDiscretizationPointsCount = discretizationPointsCount + 1;
    }
    else
        console.log("Unhandled/wrong type of knotVector");
    
    var firstKnotIndex = offset;
    var lastKnotIndex = knotVector.length - offset - 1;
    
    var e = new Array(); // result
    var de = (knotVector[lastKnotIndex] - knotVector[firstKnotIndex]) / (tmpDiscretizationPointsCount - 1);

    // fix by DB
    var i;
    for(i = 0; i < discretizationPointsCount - 1; ++i)
        e[i] = knotVector[firstKnotIndex] + i * de;
    
    if(knotVectorType == "OPEN")
        e[i] = knotVector[firstKnotIndex];
    else if(knotVectorType == "PERIODIC")
        e[i] = knotVector[firstKnotIndex] + i * de;
    
    return e;
}

function CoxDeBoor(e, indexOfBasisFunction, basisFunctionOrder, countOfBasisFunctions, knotVector)
{
    if(indexOfBasisFunction == countOfBasisFunctions - 1 && e == knotVector[knotVector.length - 1])
        return 1;
    else if(basisFunctionOrder == 0)
    {
        if(knotVector[indexOfBasisFunction] <= e && e < knotVector[indexOfBasisFunction + 1 ])
            return 1;
        else
            return 0;
    }
    else
    {
        var coef1 = 0;
        var tmp = knotVector[indexOfBasisFunction + basisFunctionOrder] - knotVector[indexOfBasisFunction];
        if(tmp != 0)
            coef1 = (e - knotVector[indexOfBasisFunction]) / tmp;
        
        var coef2 = 0;
        tmp = knotVector[indexOfBasisFunction + basisFunctionOrder + 1] - knotVector[indexOfBasisFunction + 1];
        if(tmp != 0)
            coef2 = (knotVector[indexOfBasisFunction + basisFunctionOrder + 1] - e) / tmp;
        
        return CoxDeBoor(e, indexOfBasisFunction, basisFunctionOrder - 1, countOfBasisFunctions, knotVector) * coef1
             + CoxDeBoor(e, indexOfBasisFunction + 1, basisFunctionOrder - 1, countOfBasisFunctions, knotVector) * coef2;
    }
}

function TrianglesFromSurfacePoints(surfacePoints, discretizationPointsCount /*knotVectorType = PERIODIC*/)
{
    var triangleCount = discretizationPointsCount * (discretizationPointsCount - 1) * 2 
                        - 2 * discretizationPointsCount;
    var triangles = new Array();
    var triangleIndex = 0;
    for(var i = 0; i < discretizationPointsCount - 1; ++i)
    {
        for(var j = 0; j < discretizationPointsCount; ++j)
        {
            var px, py;
            if(i != 0)
            {
                triangles[triangleIndex] = new Triangle();
                
                px = j;
                py = i;
                triangles[triangleIndex].vertices[0] = surfacePoints[py * discretizationPointsCount + px];
                triangles[triangleIndex].verticesNumerals[0] = py * discretizationPointsCount + px;
                px = (j + 1) % discretizationPointsCount;
                py = i;
                triangles[triangleIndex].vertices[1] = surfacePoints[py * discretizationPointsCount + px];
                triangles[triangleIndex].verticesNumerals[1] = py * discretizationPointsCount + px;
                px = (j + 1) % discretizationPointsCount;
                py = i + 1;
                triangles[triangleIndex].vertices[2] = surfacePoints[py * discretizationPointsCount + px];
                triangles[triangleIndex].verticesNumerals[2] = py * discretizationPointsCount + px;
                
                ++triangleIndex;
            }
            if( i != discretizationPointsCount - 2)
            {
                triangles[triangleIndex] = new Triangle();
                
                px = j;
                py = i;
                triangles[triangleIndex].vertices[0] = surfacePoints[py * discretizationPointsCount + px];
                triangles[triangleIndex].verticesNumerals[0] = py * discretizationPointsCount + px;
                px = (j + 1) % discretizationPointsCount;
                py = i + 1;
                triangles[triangleIndex].vertices[1] = surfacePoints[py * discretizationPointsCount + px];
                triangles[triangleIndex].verticesNumerals[1] = py * discretizationPointsCount + px;
                px = j;
                py = i + 1;
                triangles[triangleIndex].vertices[2] = surfacePoints[py * discretizationPointsCount + px];
                triangles[triangleIndex].verticesNumerals[2] = py * discretizationPointsCount + px;
                
                ++triangleIndex;
            }
        }  
    }
    return triangles;
}

function SurfacePointsToGeometry(sPoints)
{
    var geometry = new THREE.Geometry();
    for(var i = 0; i < sPoints.length; i++)
        geometry.vertices.push(new THREE.Vertex(new THREE.Vector3(sPoints[i].x, sPoints[i].y, sPoints[i].z)));
    
    var triangles = TrianglesFromSurfacePoints(sPoints, 20);
    
    for(var j = 0; j < (triangles.length); j++)
        geometry.faces.push(new THREE.Face3(triangles[j].verticesNumerals[0],triangles[j].verticesNumerals[1],triangles[j].verticesNumerals[2]));
    
    geometry.computeFaceNormals();
    return geometry;
}