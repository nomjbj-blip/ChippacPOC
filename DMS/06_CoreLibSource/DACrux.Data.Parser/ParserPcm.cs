using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace DACrux.Data.Parser
{
    /// <summary>
    /// PCM Parser
    /// </summary>
    public class ParserPcm : ParserTest
    {
        protected ParserPcm(string fileName)
            : base(fileName)
        {
            ReTest = false;
        }

        public string RecipeName { get; protected set; }
        public string Notch { get; protected set; }
        public bool ReTest { get; protected set; }
        public WaferDataList WaferDataList { get; protected set; }

        public override DateTime GetBackupDateTime()
        {
            return StartTime;
        }

        public virtual void SetCorrection(
            int rotationAngle
            )
        {
            if (rotationAngle == 0)
                return;

            int xmin, ymin, xmax, ymax;
            GetMinMax(out xmin, out xmax, out ymin, out ymax);

            foreach (PcmWaferData wafer in WaferDataList)
            {
                for (int idx = 0; idx < wafer.Count; idx++)
                {
                    int x = Int32.Parse(wafer[idx].X);
                    int y = Int32.Parse(wafer[idx].Y);

                    Rotate(rotationAngle, xmin, xmax, ymin, ymax, ref x, ref y);

                    wafer[idx].X = x.ToString();
                    wafer[idx].Y = y.ToString();
                }
            }
        }

        protected virtual void GetMinMax(
            out int xmin,
            out int xmax,
            out int ymin,
            out int ymax
            )
        {
            xmin = ymin = Int32.MaxValue;
            xmax = ymax = Int32.MinValue;

            foreach (PcmWaferData wafer in WaferDataList)
            {
                for (int idx = 0; idx < wafer.Count; idx++)
                {
                    int x = Int32.Parse(wafer[idx].X);
                    int y = Int32.Parse(wafer[idx].Y);

                    xmin = Math.Min(xmin, x);
                    ymin = Math.Min(ymin, y);
                    xmax = Math.Max(xmax, x);
                    ymax = Math.Max(ymax, y);
                }
            }
        }
    }

    public class WaferDataList : List<PcmWaferData>
    {
        public override string ToString()
        {
            if (Count == 0)
                return "(empty)";

            StringBuilder sb = new StringBuilder();
            sb.Append("(");

            for (int i = 0; i < Count; i++)
            {
                sb.Append(this[i].WaferID);

                if (i < Count - 1)
                    sb.Append(",");
            }

            sb.Append(")");

            return sb.ToString();
        }
    }

    public class PcmWaferData : List<PcmShotData>
    {
        public PcmWaferData(string waferID)
        {
            WaferID = waferID;
        }

        public string WaferID { get; private set; }

        public PcmShotData GetShotData(int die, int x, int y)
        {
            foreach (var item in this)
            {
                if (item.CurrentDie == die.ToString())
                    return item;
            }

            PcmShotData newData = new PcmShotData(die.ToString(), x.ToString(), y.ToString(), null);
            Add(newData);
            return newData;
        }

        public PcmShotData this[string die]
        {
            get
            {
                foreach (var item in this)
                {
                    if (item.CurrentDie == die)
                        return item;
                }

                throw new Exception(String.Format("해당하는 Die({0})를 찾을 수 없습니다.", die));
            }
        }

        public bool Contains(string die)
        {
            foreach (var item in this)
            {
                if (item.CurrentDie == die)
                    return true;
            }

            return false;
        }
    }

    public class PcmShotData : List<PcmTestData>, IComparable<PcmShotData>
    {
        public PcmShotData(int x, int y)
        {
            X = x.ToString();
            Y = y.ToString();
        }

        public PcmShotData(string dieNumberWithIndex)
        {
            CurrentDie = ParserPcm.GetValue(dieNumberWithIndex, null, null, "(");
            X = ParserPcm.GetValue(dieNumberWithIndex, "(", null, ",");
            Y = ParserPcm.GetValue(dieNumberWithIndex, ",", null, ")");
        }

        public PcmShotData(string currentDie, string dieType)
        {
            CurrentDie = currentDie;
            DieType = dieType;
        }

        public PcmShotData(string currentDie, string x, string y, string dieType)
        {
            CurrentDie = currentDie;
            X = x;
            Y = y;
            DieType = dieType;
        }

        public string CurrentDie { get; protected set; }
        public string DieType { get; protected set; }

        public string X { get; internal set; }
        public string Y { get; internal set; }

        public int CompareTo(PcmShotData data)
        {
            return data != null ? this.CurrentDie.CompareTo(data.CurrentDie) : 1;
        }
    }

    public class PcmTestData : IComparable<PcmTestData>
    {
        public PcmTestData(string name, string value, string description)
        {
            Name = name;
            Value = value;
            Description = description;
        }

        public PcmTestData(string name, string value)
            : this(name, value, null)
        {
        }

        public int CompareTo(PcmTestData data)
        {
            return data != null ? this.Name.CompareTo(data.Name) : 1;
        }

        public override string ToString()
        {
            return String.Format("{0} : {1}", Name, Value);
        }

        public string Name { get; private set; }
        public string Value { get; private set; }
        public string Description { get; private set; }
    }
}
