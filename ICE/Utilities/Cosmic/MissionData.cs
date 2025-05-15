﻿using ECommons.GameHelpers;
using ICE.Enums;
using Lumina.Excel.Sheets;
using System.Collections.Generic;

namespace ICE.Utilities;

public static unsafe partial class CosmicHelper
{

    public static readonly HashSet<uint> Ranks = [1, 2, 3, 4];
    public static readonly HashSet<uint> ARankIds = [4, 5, 6];

    public static readonly HashSet<int> CrafterJobList = [8, 9, 10, 11, 12, 13, 14, 15];
    public static readonly HashSet<int> GatheringJobList = [16, 17, 18];

    public static readonly HashSet<int> WeatherMissionList = [30, 31, 32,];
    public static readonly HashSet<int> TimedMissionList = [40, 43,];
    public static readonly HashSet<int> CriticalMissions = [512, 513, 514,];

    public static readonly HashSet<int> LimitQuantityMission = [362, 361, 368, 374];
    public static readonly HashSet<int> QuantityMission = [366, 365, 376, ];
    public static readonly HashSet<int> TimedMissions = [364, 363, 369, 375, ];
    public static readonly HashSet<int> ChainedMissions = [370, 377, ];
    public static readonly HashSet<int> BoonMissions = [371, 378, ];
    public static readonly HashSet<int> ChainBoonMissions = [372, 379, ];
    public static readonly HashSet<int> CollectableMissions = [373, 380, ];
    public static readonly HashSet<int> ReductionMissions = [381, ];

    public static readonly int MinimumLevel = 10;
    public static readonly int MaximumLevel = Player.MaxLevel;

    #region Dictionaries

    /// <summary>
    /// Some things to note that I didn't realize until after I really dug into the sheet a bit more/cleaned this up. <br></br>
    /// Sheet is: WKSMissionUnit <br></br>
    /// <b>- Row 0: Mission Name </b><br></br>
    /// <b>- Row 2: JobId attached to the quest (so 9 is crp, 10 is Bsm... ect)</b><br></br>
    /// <b>- Row 3: 2nd Required job???</b><br></br>
    /// <b>- Row 4: 3rd Required job???</b><br></br>
    /// <b>- Row 5: Bool -> Is it a critical mission?</b><br></br>
    /// <b>- Row 6: Rank -> D = 1 | C = 2 | B = 3 | 4 = A-1 | 5 = A-2 | 6 = A-3</b>
    /// <b>- Row 7: Mission time limit (seconds)
    /// <b>- Row 18: Recipe # -> Corresponds to the recipeID that is in </b>
    /// </summary>
    public class MissionListInfo
    {
        public string Name { get; set; }
        public uint JobId { get; set; }
        public uint JobId2 { get; set; } = 0;
        public uint JobId3 { get; set; } = 0;
        public uint ToDoSlot { get; set; }
        public uint Rank { get; set; }
        public bool IsCriticalMission { get; set; }
        public uint TimeLimit { get; set; }
        public uint Time { get; set; }
        public CosmicWeather Weather { get; set; }
        public uint RecipeId { get; set; } = 0;
        public uint SilverRequirement { get; set; }
        public uint GoldRequirement { get; set; }
        public uint CosmoCredit { get; set; }
        public uint LunarCredit { get; set; }
        public uint PreviousMissionID { get; set; }
        public uint MarkerId { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public int Radius { get; set; }

        public List<(int Type, int Amount)> ExperienceRewards { get; set; }
    }

    public static Dictionary<uint, MissionListInfo> MissionInfoDict = [];
    public class MoonRecipieInfo
    {
        public Dictionary<ushort, int> MainCraftsDict = [];
        public bool PreCrafts { get; set; } = false;
        public Dictionary<ushort, int> PreCraftDict = [];
    }

    public static Dictionary<uint, MoonRecipieInfo> MoonRecipies = [];

    public class GatheringInfo
    {
        public Dictionary<uint, int> MinGatherItems = [];
    }

    public static Dictionary<uint, GatheringInfo> GatheringItemDict = new();

    #endregion
}