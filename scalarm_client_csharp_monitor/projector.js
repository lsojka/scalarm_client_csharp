/* Viewer - WebGL & THREE.js projector
 *
 */

        var renderer = null;
	var scene = null;
	var camera = null;
	var mesh = null;
        // directional lights
        var dlights = new Array();
        var models = new Array();
        
Viewer = function()
{
	Sim.App.call(this);
}
Viewer.prototype = new Sim.App();

Viewer.prototype.init = function(param)
{
    Sim.App.prototype.init.call(this, param);
    this.createCameraControls();
    
    dlights[0] = new THREE.DirectionalLight( 0xffffff, 1 /*0.8*/ );
    dlights[0].position.set(-20, 0, 0);
    this.scene.add(dlights[0]);

    var amb = new THREE.AmbientLight( 0x808080, 1);
    this.scene.add(amb);
    
}

Viewer.prototype.CreateModelThenAddToScene = function(geometry)
{
    model = new Model();
    models.push(model);
    model.init(geometry, false);
    this.addObject(model);
    //console.log("Mesh created & added to scene.");
}

Viewer.prototype.createLocalCube = function()
{
    var cube = new GlobalCube();
    cube.init();
    this.addObject(cube);
}

// trackball
Viewer.prototype.createCameraControls = function()
{
    var controls = new THREE.TrackballControls( this.camera, this.renderer.domElement );
    var radius = Viewer.CAMERA_RADIUS;
    
    controls.rotateSpeed = Viewer.ROTATE_SPEED;
    controls.zoomSpeed = Viewer.ZOOM_SPEED;
    controls.panSpeed = Viewer.PAN_SPEED;
    controls.dynamicDampingFactor = Viewer.DAMPING_FACTORS;
    controls.noZoom = false;
    controls.noPan = false;
    controls.staticMoving = false;
    
    controls.minDistance = radius * Viewer.MIN_DISTANCE_FACTOR;
    controls.maxDistance = radius * Viewer.MAX_DISTANCE_FACTOR;
    
    this.controls = controls;
    
    this.camera.position.z = 3000;
}

Viewer.prototype.update = function()
{
    this.controls.update();
    Sim.App.prototype.update.call(this);
    dlights[0].position = camera.position;
}
Viewer.CAMERA_START_Z = 3000;
Viewer.CAMERA_RADIUS = 2000;
Viewer.MIN_DISTANCE_FACTOR = 1.1;
Viewer.MAX_DISTANCE_FACTOR = 2000;
Viewer.ROTATE_SPEED = 1.0;
Viewer.ZOOM_SPEED = 3;
Viewer.PAN_SPEED = 0.2;
Viewer.DAMPING_FACTORS = 0.3;
