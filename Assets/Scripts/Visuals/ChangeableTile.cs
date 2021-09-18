using System;
using UnityEngine;
using UnityEngine.Tilemaps;

public enum VisualCompletionLevel { Easy, Medium, Hard, Failed}

[CreateAssetMenu(fileName = "new changeable tile", menuName = "Tiles/ Changeable Tile")]
public class ChangeableTile : Tile
{
    public Sprite Easy;
    public Sprite Medium;
    public Sprite Hard;
    private Sprite _failed;

    private VisualCompletionLevel _completionLevel;
    private Sprite _newSprite;

    public override void GetTileData(Vector3Int position, ITilemap tilemap, ref TileData tileData)
    {
        base.GetTileData(position, tilemap, ref tileData);

        if(GameChanger.instance)
            _completionLevel = GameChanger.instance.CompletionLevel;

        switch (_completionLevel)
        {
            case VisualCompletionLevel.Easy:
                _newSprite = Easy;
                break;
            case VisualCompletionLevel.Medium:
                _newSprite = Medium;
                break;
            case VisualCompletionLevel.Hard:
                _newSprite = Hard;
                break;
            case VisualCompletionLevel.Failed:
                CreateFailedSprite();
                _newSprite = _failed;
                break;
        }
        tileData.sprite = _newSprite;
    }

    private void CreateFailedSprite()
    {
        Texture2D texture = new Texture2D(Easy.texture.width, Easy.texture.height);
        for (int x = 0; x < texture.width; x++)
        {
            for (int y = 0; y < texture.height; y++)
            {
                int r = UnityEngine.Random.Range(0, 2);

                if (r == 0)
                    texture.SetPixel(x, y, Color.white);
                else 
                {
                    Color color = new Color(Easy.texture.GetPixel(x, y).r, Easy.texture.GetPixel(x, y).g, Easy.texture.GetPixel(x, y).b, 1);
                    texture.SetPixel(x, y, color);
                }
            }
        }
        _failed = Sprite.Create(texture, Easy.textureRect, Easy.pivot); 
    }
}
