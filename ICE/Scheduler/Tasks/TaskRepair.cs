﻿using FFXIVClientStructs.FFXIV.Client.Game;
using FFXIVClientStructs.FFXIV.Component.GUI;

namespace ICE.Scheduler.Tasks
{
    internal static class TaskRepair
    {
        public static void GatherCheck()
        {
            var currentJob = PlayerHelper.GetClassJobId();

            if (CosmicHelper.GatheringJobList.Contains((int)currentJob))
            {
                if (C.SelfRepairGather && PlayerHelper.NeedsRepair(C.RepairPercent))
                {
                    P.TaskManager.Enqueue(() => OpenSelfRepair(), "Opening repair menu");
                    P.TaskManager.Enqueue(() => SelfRepair(), "Repairing self");
                    // P.TaskManager.Enqueue(() => CloseRepair(), "Closing Repair Window");
                }
            }
        }

        internal unsafe static bool OpenSelfRepair()
        {
            if (GenericHelpers.TryGetAddonByName<AtkUnitBase>("Repair", out var x) && GenericHelpers.IsAddonReady(x))
            {
                return true;
            }

            if (EzThrottler.Throttle("Opening Self Repair", 1000))
                ActionManager.Instance()->UseAction(ActionType.GeneralAction, 6);
            return false;
        }

        internal unsafe static bool SelfRepair()
        {
            if (!PlayerHelper.NeedsRepair(C.RepairPercent))
            {
                return true;
            }
            else if (Svc.Condition[Dalamud.Game.ClientState.Conditions.ConditionFlag.Mounted])
            {
                if (EzThrottler.Throttle("Attempting to dismount for spiritbonding"))
                    ActionManager.Instance()->UseAction(ActionType.GeneralAction, 9);
            }
            else if (GenericHelpers.TryGetAddonByName<AtkUnitBase>("SelectYesno", out var addon) && GenericHelpers.IsAddonReady(addon))
            {
                if (FrameThrottler.Throttle("SelectYesnoThrottle", 300))
                {
                    Svc.Log.Debug("SelectYesno Callback");
                    ECommons.Automation.Callback.Fire(addon, true, 0);
                }
            }
            else if (GenericHelpers.TryGetAddonByName<AtkUnitBase>("Repair", out var addon2) && GenericHelpers.IsAddonReady(addon2))
            {
                if (FrameThrottler.Throttle("GlobalTurnInGenericThrottle", 300))
                {
                    Svc.Log.Debug("Repair Callback");
                    ECommons.Automation.Callback.Fire(addon2, true, 0);
                }
            }
            return false;
        }

        internal unsafe static bool CloseRepair()
        {
            if (GenericHelpers.TryGetAddonByName<AtkUnitBase>("Repair", out var repairWindow))
            {
                if (GenericHelpers.IsAddonReady(repairWindow))
                {
                    if (EzThrottler.Throttle("GlobalTurnInGenericThrottle", 300))
                    {
                        Svc.Log.Debug("Repair Close Window");
                        ECommons.Automation.Callback.Fire(repairWindow, true, 0);
                    }
                }
                else
                {
                    return true;
                }
            }

            return false;
        }
    }
}
