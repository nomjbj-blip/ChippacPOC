using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DACrux.Map
{
    public interface IDefectMap
    {
        void Redraw();
        void ClearSelectedDefect();

        bool VisibleImageMark { get; set; }
        string DrawDefects { get; set; }
    }

    public interface IShot
    {
        event EventHandler ShotOptionChanged;

        void Redraw();

        int ShotArrayX { get; set; }
        int ShotArrayY { get; set; }
        int ShotStartX { get; set; }
        int ShotStartY { get; set; }
    }
}
