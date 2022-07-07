using System;
using System.Collections.Generic;
using Unity.Mathematics;

namespace VRising.PVP.Domain
{
    public class Blood {
        public enum DebugBloodType
        {
            Frailed = 0,
            Creature = 1897056612,
            Warrior = -1128238456,
            Rogue = -1030822544,
            Brute = -1464869978,
            Scholar = -700632469,
            Worker = -1342764880
        }

        public enum BloodType
        {
            Frailed = -899826404,
            Creature = -77658840,
            Warrior = -1094467405,
            Rogue = 793735874,
            Brute = 581377887,
            Scholar = -586506765,
            Worker = -540707191
        }

        public static DebugBloodType GetDebugBloodTypeByName(string name)
        {
            var bloodTypeMap = new Dictionary<string, DebugBloodType>()
            {
                {"frailed", DebugBloodType.Frailed},
                {"creature", DebugBloodType.Creature},
                {"warrior", DebugBloodType.Warrior},
                {"rogue", DebugBloodType.Rogue},
                {"brute", DebugBloodType.Brute},
                {"scholar", DebugBloodType.Scholar},
                {"worker", DebugBloodType.Worker},
            };
            if (bloodTypeMap.TryGetValue(name, out DebugBloodType bloodType))
                return bloodType;
            else
                return DebugBloodType.Frailed;
        }
        public static BloodType GetBloodTypeByName(string name)
        {
            var bloodTypeMap = new Dictionary<string, BloodType>()
            {
                {"frailed", BloodType.Frailed},
                {"creature", BloodType.Creature},
                {"warrior", BloodType.Warrior},
                {"rogue", BloodType.Rogue},
                {"brute", BloodType.Brute},
                {"scholar", BloodType.Scholar},
                {"worker", BloodType.Worker},
            };
            if (bloodTypeMap.TryGetValue(name, out BloodType bloodType))
                return bloodType;
            else
                return BloodType.Frailed;
        }
        public static DebugBloodType GetDebugBloodTypeByBloodType(BloodType bloodType)
        {
            var bloodTypeMap = new Dictionary<BloodType, DebugBloodType>()
            {
                {BloodType.Frailed, DebugBloodType.Frailed},
                {BloodType.Creature, DebugBloodType.Creature},
                {BloodType.Warrior, DebugBloodType.Warrior},
                {BloodType.Rogue, DebugBloodType.Rogue},
                {BloodType.Brute, DebugBloodType.Brute},
                {BloodType.Scholar, DebugBloodType.Scholar},
                {BloodType.Worker, DebugBloodType.Worker},
            };
            if (bloodTypeMap.TryGetValue(bloodType, out DebugBloodType debugBloodType))
                return debugBloodType;
            else
                return DebugBloodType.Frailed;
        }
    }

    struct NullableInt
    {
        public int value;
        public bool has_value;
    }

    struct NullableFloat3
    {
        public float3 value;
        public bool has_value;
    }

    public class Item
    {
        public enum ItemGUID
        {
            BloodmoonChestguard = 488592933,
            BloodmoonGloves = 1634690081,
            BloodmoonLeggings = 1292986377,
            BloodmoonBoots = -556769032,
            ShardOfTheFrozenCrypt = 1380368392,
            JewelOfTheWickedProphet = -175650376,
            NightstoneOfTheBeast = -296161379,
            SanguineAxes = -2044057823,
            SanguineSlashers = 1322545846,
            SanguineReaper = -2053917766,
            SanguineSpear = -850142339,
            SanguineCrossbow = 1389040540,
            SanguineMace = -126076280,
            SanguineSword = -774462329,
            GeneralsSoulReaper = 1887724512,
            RoyalMantleOfAshfolkKings = 584164197
        }

        public static int[] DefaultItemKit = new int[] {
            (int)ItemGUID.BloodmoonChestguard,
            (int)ItemGUID.BloodmoonGloves,
            (int)ItemGUID.BloodmoonLeggings,
            (int)ItemGUID.BloodmoonBoots,
            (int)ItemGUID.ShardOfTheFrozenCrypt,
            (int)ItemGUID.JewelOfTheWickedProphet,
            (int)ItemGUID.NightstoneOfTheBeast,
            (int)ItemGUID.SanguineAxes,
            (int)ItemGUID.SanguineSlashers,
            (int)ItemGUID.SanguineReaper,
            (int)ItemGUID.SanguineSpear,
            (int)ItemGUID.SanguineCrossbow,
            (int)ItemGUID.SanguineMace,
            (int)ItemGUID.SanguineSword,
            (int)ItemGUID.GeneralsSoulReaper,
            (int)ItemGUID.RoyalMantleOfAshfolkKings
        };
    }
}
