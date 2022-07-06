using System;
using System.Collections.Generic;
using Unity.Mathematics;

namespace VRising.PVP.Domain
{
    public class Blood {
        public enum BloodType
        {
            Frailed = 0,
            Creature = 1897056612,
            Warrior = -1128238456,
            Rogue = -1030822544,
            Brute = -1464869978,
            Scholar = -700632469,
            Worker = -1342764880
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
