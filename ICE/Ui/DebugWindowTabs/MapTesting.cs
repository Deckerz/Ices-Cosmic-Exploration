﻿using FFXIVClientStructs.FFXIV.Client.UI.Agent;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICE.Ui.DebugWindowTabs
{
    internal class MapTesting
    {
        private static int TableRow = 1;
        private static int posX = 0;
        private static int posY = 0;
        private static int posRadius = 0;

        public static unsafe void Draw()
        {
            ImGui.InputInt("TableId", ref TableRow);

            var MapInfo = ExcelHelper.MarkerSheet;

            if (ImGui.Button($"Test Radius"))
            {
                var agent = AgentMap.Instance();

                int _x = MapInfo.GetRow((uint)TableRow).Unknown1.ToInt() - 1024;
                int _y = MapInfo.GetRow((uint)TableRow).Unknown2.ToInt() - 1024;
                int _radius = MapInfo.GetRow((uint)TableRow).Unknown3.ToInt();
                IceLogging.Debug($"X: {_x} Y: {_y} Radius: {_radius}");

                Utils.SetGatheringRing(1237, _x, _y, _radius);
            }
            ImGui.SetNextItemWidth(125);
            ImGui.InputInt("Map X (Sheet)", ref posX);
            ImGui.SameLine();
            ImGui.SetNextItemWidth(125);
            ImGui.InputInt("Map Y (Sheet)", ref posY);
            ImGui.SameLine();
            ImGui.SetNextItemWidth(125);
            ImGui.InputInt("Map Radius", ref posRadius);
            if (ImGui.Button($"Test Map Marker from coords"))
            {
                var agent = AgentMap.Instance();
                int _x = posX - 1024;
                int _y = posY - 1024;
                IceLogging.Debug($"X: {_x} Y: {_y}");

                Utils.SetGatheringRing(agent->CurrentTerritoryId, _x, _y, posRadius);
            }
        }
    }
}
