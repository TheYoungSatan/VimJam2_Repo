// Bresenham's line algorithm - converted to unity (js) - mgear - http://unitycoder.com/blog/
// original source: http://en.wikipedia.org/wiki/Bresenham%27s_line_algorithm#Simplification
// *attach this scrip to a plane, then draw with mouse: click startpoint, click endpoint

private var texture:Texture2D;
public var texture_width:int=256;
public var texture_height:int=256;
private var point1:boolean = false;
private var point2:boolean = false;
private var startpoint:Vector2;
private var endpoint:Vector2;

function Start()
{
	texture = new Texture2D (texture_width, texture_height);
	renderer.material.mainTexture = texture;
}

function Update()
{
	// draw lines with mouse
	if (Input.GetMouseButtonDown(0))
	{
		var ray:Ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		var hit:RaycastHit;
		if (Physics.Raycast(ray,hit, 100))
		{
			if (!point1)
			{
				pixelUV = hit.textureCoord;
				pixelUV.x *= texture.width;
				pixelUV.y *= texture.height;
				startpoint = Vector2(pixelUV.x, pixelUV.y);
				point1=true;
				
				print (startpoint);
			}else{
				pixelUV = hit.textureCoord;
				pixelUV.x *= texture.width;
				pixelUV.y *= texture.height;
				endpoint = Vector2(pixelUV.x, pixelUV.y);
				point1=false;
				drawLine(startpoint.x,startpoint.y,endpoint.x,endpoint.y);
				print (endpoint);
			}
		}
	}
}



// from wikipedia
 function drawLine(x0:int, y0:int, x1:int, y1:int)
 {
   dx= Mathf.Abs(x1-x0);
   dy= Mathf.Abs(y1-y0);
   if (x0 < x1) {sx=1;}else{sx=-1;}
   if (y0 < y1) {sy=1;}else{sy=-1;}
   err=dx-dy;
 
	loop=true;
	while (loop) 
	{
		texture.SetPixel (x0,y0,Color(1,1,1,1));
		 if ((x0 == x1) && (y0 == y1)) loop=false;
		 e2 = 2*err;
		 if (e2 > -dy)
		 {
		   err = err - dy;
		   x0 = x0 + sx;
		 }
		 if (e2 <  dx)
		 {
		   err = err + dx;
		   y0 = y0 + sy;
		 }
   }
   texture.Apply();
}