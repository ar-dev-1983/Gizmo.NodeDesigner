using System;
using System.Collections.Generic;
using System.Linq;

namespace Gizmo.NodeFramework
{
    public class DataStruct : IDisposable
    {
        public Guid Id { set; get; }
        public object Value { set; get; }
        public DateTime TimeStamp { set; get; }
        public int Confidence { set; get; }

        public DataStruct()
        {
            Id = Guid.NewGuid();
            Value = 0;
            TimeStamp = DateTime.Now;
            Confidence = -1;
        }

        public DataStruct(object _Value, DateTime _TimeStamp, int _Confidence)
        {
            Id = Guid.NewGuid();
            Value = _Value;
            TimeStamp = _TimeStamp;
            Confidence = _Confidence;
        }

        public bool IsValueMoreThanMax(double Max)
        {
            return (double)Value > Max;
        }

        public bool IsValueLessThanMin(double Min)
        {
            return (double)Value < Min;
        }

        public DataStructCheckTypeResult CheckValue(DataStructCheckType CheckType, double Min, double Max, int MinConfidence)
        {
            DataStructCheckTypeResult result = DataStructCheckTypeResult.None;

            if (CheckType != DataStructCheckType.None)
            {
                if (CheckType == DataStructCheckType.CheckConfidense)
                {
                    result = Confidence > MinConfidence ? DataStructCheckTypeResult.None : DataStructCheckTypeResult.BadConfidence;
                }
                else if (CheckType == DataStructCheckType.CheckAll)
                {
                    result = (double)Value > Max ? DataStructCheckTypeResult.BadMaximum : DataStructCheckTypeResult.None;
                    result = (double)Value < Min ? DataStructCheckTypeResult.BadMinimum : result;
                    result = Confidence > MinConfidence ? result : DataStructCheckTypeResult.BadConfidence;
                }
            }
            return result;
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        public static DataStruct AverageOnInterval(List<DataStruct> Input, DateTime Start, DateTime End)
        {
            var result = new DataStruct();
            var rangeValues = (from item in Input.AsEnumerable() where item.TimeStamp > Start && item.TimeStamp <= End select item).ToList();
            result.Value = double.Parse(rangeValues.Average(s => double.Parse(s.Value.ToString())).ToString());
            result.Confidence = (int)rangeValues.Average(s => s.Confidence);
            result.TimeStamp = rangeValues[index: rangeValues.Count - 1].TimeStamp;
            rangeValues = null;
            return result;
        }

        public static DataStruct SumOnInterval(List<DataStruct> Input, DateTime Start, DateTime End)
        {
            var result = new DataStruct();
            var rangeValues = (from item in Input.AsEnumerable() where item.TimeStamp > Start && item.TimeStamp <= End select item).ToList();
            result.Value = double.Parse(rangeValues.Sum(s => double.Parse(s.Value.ToString())).ToString());
            result.Confidence = (int)rangeValues.Average(s => s.Confidence);
            result.TimeStamp = rangeValues[index: rangeValues.Count - 1].TimeStamp;
            rangeValues = null;
            return result;
        }
    }

}
