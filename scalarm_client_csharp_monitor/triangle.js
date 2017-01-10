// triangle class

function Triangle()
{
    this.vertices = [new Point, new Point, new Point];
    this.verticesNumerals = new Array();
}
Triangle.prototype.setPoints = function(point1, point2, point3)
{
    this.vertices[0] = point1;
    this.vertices[1] = point2;
    this.vertices[2] = point3;
}
Triangle.prototype.printPoints = function()
{
    console.log("Triangle \n\
("+this.vertices[0].x+","+this.vertices[0].y+","+this.vertices[0].z+") \n\
("+this.vertices[1].x+","+this.vertices[1].y+","+this.vertices[1].z+")\n\
("+this.vertices[2].x+","+this.vertices[2].y+","+this.vertices[2].z);
}

