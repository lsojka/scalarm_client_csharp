/* klasa Model
 * - pojedyncze wtrącenie, posiadające własną (skomplikowaną geometrię)
 * - nowsza wersja klasy Pocket
*/
Model = function()
{
	Sim.Object.call(this);
}
Model.prototype = new Sim.Object();
Model.prototype.init = function(geometry, if_wireframe)
{
    var material = new THREE.MeshPhongMaterial();
    material.wireframe = if_wireframe;
    material.color = new THREE.Color("0x00ffff");
    
    var mesh = new THREE.Mesh( geometry, material ); 
    mesh.position = new THREE.Vector3(0,0,0); // !
    this.setObject3D(mesh);   
}

Model.prototype.update = function()
{
    //this.object3D.rotation.x += 0.0025;
    //this.object3D.rotation.y += 0.0025;   
}

GlobalCube = function()
{
    Sim.Object.call(this);
}
GlobalCube.prototype = new Sim.Object();
GlobalCube.prototype.init = function()
{
    var cgeometry = new THREE.CubeGeometry(2000, 2000, 2000);
    //var cmaterial = new THREE.MeshPhongMaterial();
    var cmaterial = new THREE.MeshBasicMaterial();
    cmaterial.wireframe = true;
    cmaterial.color = new THREE.Color("0xffffff");
    var cmesh = new THREE.Mesh(cgeometry, cmaterial);
    cmesh.position = new THREE.Vector3(0,0,0);
    this.setObject3D(cmesh);
}