using UnityEngine;
using System.Collections;
using UnityEngine.InputSystem;

public class BresenHamplswork : MonoBehaviour
{

    #region Input
    public void InputMouseClick(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            DrawLine();
        }
    }
    #endregion

    private Texture2D texture;
	public int texture_width = 256;
	public int texture_height = 256;
	private bool point1 = false;
	private bool point2 = false;
	private Vector2 startpoint;
	private Vector2 endpoint;

    private Vector2 _mousePos;
    private PlayerInput _playerInput;
    private InputAction _clickAction;

    private void Awake()
    {
        _playerInput = FindObjectOfType<PlayerInput>();

        _clickAction = _playerInput.currentActionMap.FindAction("MouseClick");

        _clickAction.performed += InputMouseClick;
    }

    void Start()
	{
		texture = new Texture2D(texture_width, texture_height, TextureFormat.ARGB32, false);
        Color fillColor = Color.clear;
        Color[] fillPixels = new Color[texture_width * texture_height];

        for (int i = 0; i < fillPixels.Length; i++)
        {
            fillPixels[i] = fillColor;
        }

        texture.SetPixels(fillPixels);
        texture.Apply();

		GetComponent<Renderer>().material.mainTexture = texture;
	}

    void Update()
	{
        _mousePos = Mouse.current.position.ReadValue();
    }

    private void DrawLine()
    {
        Ray ray = Camera.main.ScreenPointToRay(_mousePos);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            if (!point1)
            {
                var pixelUV = hit.textureCoord;
                pixelUV.x *= texture.width;
                pixelUV.y *= texture.height;
                startpoint = new Vector2(pixelUV.x, pixelUV.y);
                point1 = true;

                print(startpoint);
            }
            else
            {
                var pixelUV = hit.textureCoord;
                pixelUV.x *= texture.width;
                pixelUV.y *= texture.height;
                endpoint = new Vector2(pixelUV.x, pixelUV.y);
                point1 = false;
                drawLine((int)startpoint.x, (int)startpoint.y, (int)endpoint.x, (int)endpoint.y);
                print(endpoint);
            }
        }
    }



    // from wikipedia
    void drawLine(int x0, int y0, int x1, int y1)
	{
		var dx = Mathf.Abs(x1 - x0);
		var sx = x0 < x1 ? 1 : -1;
		var dy = Mathf.Abs(y1 - y0);
		var sy = y0 < y1 ? 1 : -1;

		var err = dx - dy; 

		var loop = true;
		while (loop)
		{
			texture.SetPixel(x0, y0, Color.red);
			if ((x0 == x1) && (y0 == y1)) loop = false;
			var e2 = 2 * err;
			if (e2 > -dy)
			{
				err = err - dy;
				x0 = x0 + sx;
			}
			if (e2 < dx)
			{
				err = err + dx;
				y0 = y0 + sy;
			}
		}
		texture.Apply();
	}
}