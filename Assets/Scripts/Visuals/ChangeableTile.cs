using System;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(fileName = "new changeable tile", menuName = "Tiles/ Changeable Tile")]
public class ChangeableTile : Tile
{
    public Sprite Easy;
    public Sprite Medium;
    public Sprite Hard;
    public Sprite Failed;

    private CompletionLevel _completionLevel;
    private Sprite _newSprite;

    public override void GetTileData(Vector3Int position, ITilemap tilemap, ref TileData tileData)
    {
        base.GetTileData(position, tilemap, ref tileData);

        if(GameChanger.instance)
            _completionLevel = GameChanger.instance.EnvironmentCompletionLevel;

        switch (_completionLevel)
        {
            case CompletionLevel.Easy:
                _newSprite = Easy;
                break;
            case CompletionLevel.Medium:
                _newSprite = Medium;
                break;
            case CompletionLevel.Hard:
                _newSprite = Hard;
                break;
            case CompletionLevel.Failed:
                _newSprite = Failed;
                break;
        }
        tileData.sprite = _newSprite;
    }
}
