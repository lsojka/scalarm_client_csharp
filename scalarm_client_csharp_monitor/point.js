// point class
function Point()
{
    this.x = 0;
    this.y = 0;
    this.z = 0;
    this._isBoundaryPoint = false;
    
}
Point.prototype.setXYZ = function(dx,dy,dz)
{
    this.x = dx;
    this.y = dy;
    this.z = dz;
}

Point.prototype.setX = function(dx){this.x = dx;}
Point.prototype.setY = function(dy){this.y = dy;}
Point.prototype.setZ = function(dz){this.z = dz;}
// gettery nie działają z powodu znanego tylko sobie
Point.prototype.getX = function(){ return this.x;}
Point.prototype.getY = function(){ return this.y;}
Point.prototype.getZ = function(){ return this.z;}

Point.prototype.setBoundaryPoint = function(val)
{
    this._isBoundaryPoint = val;
}

