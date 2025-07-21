using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICE.Ui.DebugWindowTabs
{
    internal class MoonGatheringTable
    {
        public static unsafe void Draw()
        {
            var itemName = ExcelHelper.ItemSheet;

            if (ImGui.BeginTable("Gathering Mission Dictionary", 10, ImGuiTableFlags.RowBg | ImGuiTableFlags.Borders | ImGuiTableFlags.SizingFixedFit))
            {
                ImGui.TableSetupColumn("MissionId");
                ImGui.TableSetupColumn("Mission Name");
                ImGui.TableSetupColumn("Flag Location");
                ImGui.TableSetupColumn("Nodeset");
                ImGui.TableSetupColumn("Item 1");
                ImGui.TableSetupColumn("Item Amount###Item1Amount");
                ImGui.TableSetupColumn("Item 2");
                ImGui.TableSetupColumn("Item Amount###Item2Amount");
                ImGui.TableSetupColumn("Item 3");
                ImGui.TableSetupColumn("Item Amount###Item3Amount");

                ImGui.TableHeadersRow();

                foreach (var entry in CosmicHelper.GatheringItemDict)
                {
                    ImGui.TableNextRow();

                    ImGui.TableSetColumnIndex(0);
                    ImGui.Text($"{entry.Key}");

                    var mission = CosmicHelper.MissionInfoDict[entry.Key];
                    ImGui.TableNextColumn();
                    ImGui.Text($"{mission.Name}");

                    ImGui.TableNextColumn();
                    ImGui.Text($"{mission.X}, {mission.Y}");
                    if (ImGui.IsItemHovered() && ImGui.IsItemClicked())
                    {
                        ImGui.SetClipboardText($"new Vector2({mission.X}, {mission.Y}), ");
                    }

                    ImGui.TableNextColumn();
                    ImGui.Text($"Nodeset: {mission.NodeSet}");

                    foreach (var item in entry.Value.MinGatherItems)
                    {
                        ImGui.TableNextColumn();
                        string name = itemName.GetRow(item.Key).Name.ToString();
                        ImGui.Text(name);
                        if (ImGui.IsItemHovered())
                        {
                            ImGui.BeginTooltip();
                            ImGui.Text($"{item.Key}");
                            ImGui.EndTooltip();
                        }

                        ImGui.TableNextColumn();
                        ImGui.Text($"{item.Value}");
                    }
                }

                ImGui.EndTable();
            }
        }
    }
}
