[System.Serializable]
public class Tile
{
    public int x, y;
    public TileType tileType;

    
    public Tile(int x, int y, TileType tileType)
    {
        this.x = x;
        this.y = y;
        this.tileType = tileType;
    }
}