using System.ComponentModel;

namespace Assets.Scripts.World.Model.Tile
{
    public class WorldTile : INotifyPropertyChanged
    {

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            PropertyChanged?.Invoke(this, e);
        }

        protected void OnPropertyChanged(string propertyName)
        {
            OnPropertyChanged(new PropertyChangedEventArgs(propertyName));
        }


        public TileTerrainInfo TerrainInfo { get; set; }
        public int X { get; set; }
        public int Y { get; set; }

        public WorldTile(int x, int y)
        {
            X = x;
            Y = y;
        }
    }
}
