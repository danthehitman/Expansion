using Assets.Scripts.World.Model.Tile;
using System.ComponentModel;
using UnityEngine;

namespace Assets.Scripts.World.Model
{
    public class WorldModel : INotifyPropertyChanged
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

        private const int HOURS_IN_DAY = 24;
        private const int MINUTES_IN_DAY = 1440;
        private int minute;
        private readonly int width;
        private readonly int height;

        public WorldTile[,] WorldTiles { get; set; }

        public int ActiveWorldX { get; set; }
        public int ActiveWorldY { get; set; }

        public int Width => width;

        public int Height => height;

        public int Day => (Minute / 60) / HOURS_IN_DAY;

        public int Month => Day / 30;

        public int Year => Month / 12;

        public int Hour => Minute / 60;

        public int Minute
        {
            get => minute;

            private set
            {
                minute = value;
                OnPropertyChanged(nameof(Minute));
            }
        }

        public int GetMinuteInDay() => Minute % MINUTES_IN_DAY;

        public void AddMinute(int toAdd = 1) => Minute += toAdd;

        public void AddHour() => Minute += 60;

        public void AddDay() => Minute += (Minute * 60) * HOURS_IN_DAY;

        public WorldModel(int width = 100, int height = 100)
        {
            this.width = width;
            this.height = height;
        }

        public WorldTile GetTileAt(int x, int y)
        {
            if (x > width || x < 0 || y > height || y < 0)
            {
                Debug.LogError($"Tiles({x},{y}) is out of range.");
                return null;
            }
            return WorldTiles[x, y];
        }
    }
}
