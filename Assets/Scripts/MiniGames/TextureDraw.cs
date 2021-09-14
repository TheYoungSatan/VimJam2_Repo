using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class TextureDraw
{
	public static void DrawLine(Texture2D tex, int x1, int y1, int x2, int y2, Color col)
	{
		int dy = (int)(y2 - y1);
		int dx = (int)(x2 - x1);
		int stepx, stepy;

		if (dy < 0) { dy = -dy; stepy = -1; }
		else { stepy = 1; }
		if (dx < 0) { dx = -dx; stepx = -1; }
		else { stepx = 1; }
		dy <<= 1;
		dx <<= 1;

		float fraction = 0;

		tex.SetPixel(x1, y1, col);
		if (dx > dy)
		{
			fraction = dy - (dx >> 1);
			while (Mathf.Abs(x1 - x2) > 1)
			{
				if (fraction >= 0)
				{
					y1 += stepy;
					fraction -= dx;
				}
				x1 += stepx;
				fraction += dy;
				tex.SetPixel(x1, y1, col);
			}
		}
		else
		{
			fraction = dx - (dy >> 1);
			while (Mathf.Abs(y1 - y2) > 1)
			{
				if (fraction >= 0)
				{
					x1 += stepx;
					fraction -= dy;
				}
				y1 += stepy;
				fraction += dx;
				tex.SetPixel(x1, y1, col);
			}
		}
	}
}
