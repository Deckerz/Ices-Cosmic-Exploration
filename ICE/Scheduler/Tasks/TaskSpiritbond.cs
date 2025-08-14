using ECommons.Automation;
using ECommons.UIHelpers.AddonMasterImplementations;
using FFXIVClientStructs.FFXIV.Client.Game;
using FFXIVClientStructs.FFXIV.Component.GUI;

namespace ICE.Scheduler.Tasks
{
    public unsafe static class TaskSpiritbond
    {
        public static ushort Weapon { get => InventoryManager.Instance()->GetInventoryContainer(InventoryType.EquippedItems)->Items[0].SpiritbondOrCollectability; }
        public static ushort Offhand { get => InventoryManager.Instance()->GetInventoryContainer(InventoryType.EquippedItems)->Items[1].SpiritbondOrCollectability; }
        public static ushort Helm { get => InventoryManager.Instance()->GetInventoryContainer(InventoryType.EquippedItems)->Items[2].SpiritbondOrCollectability; }
        public static ushort Body { get => InventoryManager.Instance()->GetInventoryContainer(InventoryType.EquippedItems)->Items[3].SpiritbondOrCollectability; }
        public static ushort Hands { get => InventoryManager.Instance()->GetInventoryContainer(InventoryType.EquippedItems)->Items[4].SpiritbondOrCollectability; }
        public static ushort Legs { get => InventoryManager.Instance()->GetInventoryContainer(InventoryType.EquippedItems)->Items[6].SpiritbondOrCollectability; }
        public static ushort Feet { get => InventoryManager.Instance()->GetInventoryContainer(InventoryType.EquippedItems)->Items[7].SpiritbondOrCollectability; }
        public static ushort Earring { get => InventoryManager.Instance()->GetInventoryContainer(InventoryType.EquippedItems)->Items[8].SpiritbondOrCollectability; }
        public static ushort Neck { get => InventoryManager.Instance()->GetInventoryContainer(InventoryType.EquippedItems)->Items[9].SpiritbondOrCollectability; }
        public static ushort Wrist { get => InventoryManager.Instance()->GetInventoryContainer(InventoryType.EquippedItems)->Items[10].SpiritbondOrCollectability; }
        public static ushort Ring1 { get => InventoryManager.Instance()->GetInventoryContainer(InventoryType.EquippedItems)->Items[11].SpiritbondOrCollectability; }
        public static ushort Ring2 { get => InventoryManager.Instance()->GetInventoryContainer(InventoryType.EquippedItems)->Items[12].SpiritbondOrCollectability; }
        public static bool IsSpiritbondReadyAny()
        {
            if (Weapon == 10000) return true;
            if (Offhand == 10000) return true;
            if (Helm == 10000) return true;
            if (Body == 10000) return true;
            if (Hands == 10000) return true;
            if (Legs == 10000) return true;
            if (Feet == 10000) return true;
            if (Earring == 10000) return true;
            if (Neck == 10000) return true;
            if (Wrist == 10000) return true;
            if (Ring1 == 10000) return true;
            if (Ring2 == 10000) return true;

            return false;
        }

        public unsafe static bool TryExtractMateria()
        {
            if (!EzThrottler.Throttle("Extract", 250))
                return false;

            if (InventoryManager.Instance()->GetEmptySlotsInBag() < 1 || !IsSpiritbondReadyAny() || !C.SelfSpiritbondGather || !((Job)PlayerHelper.GetClassJobId()).IsDol())
                return true;

            if (ECommons.GameHelpers.Player.IsBusy)
                return false;

            if (Svc.Condition[Dalamud.Game.ClientState.Conditions.ConditionFlag.Mounted])
            {
                if (EzThrottler.Throttle("Attempting to dismount for spiritbonding"))
                    ActionManager.Instance()->UseAction(ActionType.GeneralAction, 9);
                return false;
            }

            if (GenericHelpers.TryGetAddonByName("MaterializeDialog", out AtkUnitBase* addonMaterializeDialog) && GenericHelpers.IsAddonReady(addonMaterializeDialog))
            {
                new AddonMaster.MaterializeDialog(addonMaterializeDialog).Materialize();
                return false;
            }

            if (!GenericHelpers.TryGetAddonByName("Materialize", out AtkUnitBase* addonMaterialize))
            {
                ActionManager.Instance()->UseAction(ActionType.GeneralAction, 14);
                return false;
            }
            else if (GenericHelpers.IsAddonReady(addonMaterialize))
            {
                var list = addonMaterialize->GetNodeById(12)->GetAsAtkComponentList();

                if (list == null)
                    return false;

                var spiritbondTextNode = list->UldManager.NodeList[2]->GetComponent()->GetTextNodeById(5)->GetAsAtkTextNode();
                var categoryTextNode = addonMaterialize->GetNodeById(4)->GetAsAtkComponentDropdownList()->UldManager.NodeList[1]->GetAsAtkComponentCheckBox()->GetTextNodeById(3)->GetAsAtkTextNode();

                if (spiritbondTextNode == null || categoryTextNode == null)
                    return false;

                if (spiritbondTextNode->NodeText.ToString().Replace(" ", string.Empty) == "100%")
                    ECommons.Automation.Callback.Fire(addonMaterialize, true, 2, 0);
            }
            else
            {
                addonMaterialize->Close(true);
                return true;
            }
            return false;
        }
    }
}
