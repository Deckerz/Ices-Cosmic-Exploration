using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static ECommons.UIHelpers.AddonMasterImplementations.AddonMaster;

namespace ICE.Ui.DebugWindowTabs
{
    internal class PlayerInfo
    {
        public static void Draw()
        {
            ImGui.Text("Need to actually put the player info here. It got lost");
            ImGui.Spacing();
            ImGui.Text($"Items on person: ");
            foreach (var item in ConsumableInfo.Food)
            {
                if (PlayerHelper.GetItemCount((int)item.Id, out var count) && count > 0)
                    ImGui.Text($"{item.Name} | {item.Id}");
            }
        }
    }
}
