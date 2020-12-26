using Gizmo.NodeFramework;
using Gizmo.WPF;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Linq;

namespace Gizmo.NodeDesigner
{
    public class SerializationService : ISerialization
    {
        public Project OpenProject(string filename)
        {
            JsonSerializerSettings settings = new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.All,
                ObjectCreationHandling = ObjectCreationHandling.Replace,
                FloatParseHandling = FloatParseHandling.Decimal,
            };

            var Project = JsonConvert.DeserializeObject<Project>(File.ReadAllText(filename), settings);
            foreach (var node in Project.ProjectEngine.Nodes)
            {
                node.OnAdd(Project.ProjectEngine);
                foreach (var variable in node.Inputs)
                {
                    SetValue(variable);
                }
                foreach (var variable in node.Outputs)
                {
                    SetValue(variable);
                }
                foreach (var variable in node.Constants)
                {
                    SetValue(variable);
                }
                foreach (var variable in node.Settings)
                {
                    SetValue(variable);
                }
            }
            return Project;
        }

        private static void SetValue(Variable variable)
        {
            if (variable.DataType == typeof(decimal))
            {
                variable.SetValueInternally(Convert.ToDecimal(variable.Value));
            }
            else if (variable.DataType == typeof(double))
            {
                variable.SetValueInternally(Convert.ToDouble(variable.Value));
            }
            else if (variable.DataType == typeof(int))
            {
                variable.SetValueInternally(Convert.ToInt32(variable.Value));
            }
            else if (variable.DataType == typeof(uint))
            {
                variable.SetValueInternally(Convert.ToUInt32(variable.Value));
            }
            else if (variable.DataType == typeof(bool))
            {
                variable.SetValueInternally(Convert.ToBoolean(variable.Value));
            }
        }

        public void SaveProject(string filename, Project item)
        {
            JsonSerializer serializer = new JsonSerializer
            {
                Formatting = Formatting.Indented,
                TypeNameHandling = TypeNameHandling.All
            };

            using StreamWriter sw = new StreamWriter(filename);
            using JsonWriter writer = new JsonTextWriter(sw);
            serializer.Serialize(writer, item);
        }

        public void SaveSettings(string filename, AppSettings settings)
        {
            JsonSerializer serializer = new JsonSerializer
            {
                Formatting = Formatting.Indented
            };
            using StreamWriter sw = new StreamWriter(filename);
            using JsonWriter writer = new JsonTextWriter(sw);
            serializer.Serialize(writer, settings);
        }

        public AppSettings OpenSettings(string filename)
        {
            return JsonConvert.DeserializeObject<AppSettings>(File.ReadAllText(filename));
        }
    }
}
