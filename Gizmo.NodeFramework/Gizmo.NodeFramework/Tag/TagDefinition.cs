using System;
using System.ComponentModel;

namespace Gizmo.NodeFramework
{
    public class TagDefinition
    {
        [Category("Tag Configuration")]
        [Description("Timestamp")]
        public DateTime TimeStamp { set; get; }

        [Category("Tag Configuration")]
        [Description("Tag Name")]
        public string TagName { set; get; }

        [Category("Tag Configuration")]
        [Description("Tag Number")]
        public uint? Tagno { set; get; }

        [Category("Tag Configuration")]
        [Description("Description")]
        public string Description { set; get; }

        [Category("Tag Configuration")]
        [Description("Parent Tag Name")]
        public string ParentTagname { set; get; }

        [Category("Tag Configuration")]
        [Description("Engineering Units")]
        public string Units { set; get; }

        [Category("Enable Flag Configuration")]
        [Description("Active")]
        public bool? Active { set; get; }

        [Category("Enable Flag Configuration")]
        [Description("Parent Tag Flag")]
        public bool? ClassTag { set; get; }

        [Category("Enable Flag Configuration")]
        [Description("Collection Enable")]
        public bool? CollectionEnable { set; get; }

        [Category("Enable Flag Configuration")]
        [Description("Allow Data Store")]
        public bool? StoreEnable { set; get; }

        [Category("Collection Configuration")]
        [Description("Source Tag Name")]
        public string SourceTagname { set; get; }

        [Category("Collection Configuration")]
        [Description("Source Tag Units")]
        public string SourceUnits { set; get; }

        [Category("Collection Configuration")]
        [Description("Scan Frequency")]
        public uint? ScanFrequency { set; get; }

        [Category("Collection Configuration")]
        [Description("High Extreme")]
        public double? HighExtreme { set; get; }

        [Category("Collection Configuration")]
        [Description("Low Extreme")]
        public double? LowExtreme { set; get; }

        [Category("Collection Configuration")]
        [Description("Scan Timestamp Unit")]
        public string ScanUnit { set; get; }

        [Category("Collection Configuration")]
        [Description("Extended Source Tag Name")]
        public string ExtendedSourceTag { set; get; }

        [Category("Collection Configuration")]
        [Description("RDI / Link Collector")]
        public string SourceCollector { set; get; }

        [Category("Collection Configuration")]
        [Description("Source System")]
        public string SourceSystem { set; get; }

        [Category("Collection Configuration")]
        [Description("Source Tag Type")]
        public string SourceTagtype { set; get; }

        [Category("Collection Configuration")]
        [Description("Source Tag Attribute")]
        public string SourceAttribute { set; get; }

        [Category("Collection Configuration")]
        [Description("Source Name")]
        public string SourceName { set; get; }

        [Category("Alarm Configuration")]
        [Description("HIHI Limit")]
        public double? HiHiLimit { set; get; }

        [Category("Alarm Configuration")]
        [Description("HI Limit")]
        public double? HiLimit { set; get; }

        [Category("Alarm Configuration")]
        [Description("LO Limit")]
        public double? LoLimit { set; get; }

        [Category("Alarm Configuration")]
        [Description("LOLO Limit")]
        public double? LoLoLimit { set; get; }

        [Category("Alarm Configuration")]
        [Description("HIHI Enable")]
        public bool? HiHiEnable { set; get; }

        [Category("Alarm Configuration")]
        [Description("HI Enable")]
        public bool? HiEnable { set; get; }

        [Category("Alarm Configuration")]
        [Description("LO Enable")]
        public bool? LoEnable { set; get; }

        [Category("Alarm Configuration")]
        [Description("LOLO Enable")]
        public bool? LoLoEnable { set; get; }
    }
}
