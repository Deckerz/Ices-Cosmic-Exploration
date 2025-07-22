using System.Collections.Generic;
using System.IO;

namespace ICE.Config;

public class WaypointInfo : IYamlConfig
{
    private int version = 1;
    public Dictionary<Vector2, uint> NodeDict { get; set; } = new()
    {

    };

    public List<GatheringUtil.GathNodeInfo> CustomNodeInfoList { get; set; } = new();
    public uint NodeId { get; set; } = 0;
    public int GatheringType { get; set; } = 2;
    public int ZoneId { get; set; } = 0;
    public uint Nodeset {  get; set; } = 0;

    public static string ConfigPath => Path.Combine(Svc.PluginInterface.ConfigDirectory.FullName, "Node Waypoints.yaml");
    public void Save() => YamlConfig.Save(this, ConfigPath);
}
