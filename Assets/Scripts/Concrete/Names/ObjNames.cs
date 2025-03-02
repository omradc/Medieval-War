using UnityEngine;

namespace Assets.Scripts.Concrete.Names
{
    class ObjNames
    {
        string name;
        public ObjNames(string name)
        {
            this.name = name;
        }
        public string PreviewBuildingName(int index = 0)
        {
            if (name == "Preview_PawnHouse(Clone)")
                return "PawnHouse_Blue";
            if (name == "Preview_WarriorHouse(Clone)")
                return "WarriorHouse_Blue";
            if (name == "Preview_ArcherHouse(Clone)")
                return "ArcherHouse_Blue";
            if (name == "Preview_Tower(Clone)")
                return "Tower_Blue";
            if (name == "Preview_Castle(Clone)")
                return "Castle_Blue";
            if (name == "Preview_Fence2x2(Clone)")
                return "Fence2x2";
            if (name == "Preview_Repo(Clone)")
                return "Repo_Blue";
            if (name == "Preview_Wall(Clone)")
                switch (index)
                {
                    case 0:
                        return "WallHorizontal_Blue";
                    case 1:
                        return "WallDoor_Blue";
                    case 2:
                        return "WallVertical_Blue";
                    case 3:
                        return "WallOne_Blue";
                    default:
                        return "";
                }
            else
            {
                Debug.Log("Previw Name Not Found");
                return "";
            }
        }
        public string BuildingName()
        {
            if (name == "WallHorizontal_Blue(Clone)")
                return "WallHorizontal_Yellow";
            if (name == "WallHorizontal_Yellow(Clone)")
                return "WallHorizontal_Red";
            if (name == "WallHorizontal_Red(Clone)")
                return "WallHorizontal_Purple";

            if (name == "WallDoor_Blue(Clone)")
                return "WallDoor_Yellow";
            if (name == "WallDoor_Yellow(Clone)")
                return "WallDoor_Red";
            if (name == "WallDoor_Red(Clone)")
                return "WallDoor_Purple";

            if (name == "WallOne_Blue(Clone)")
                return "WallOne_Yellow";
            if (name == "WallOne_Yellow(Clone)")
                return "WallOne_Red";
            if (name == "WallOne_Red(Clone)")
                return "WallOne_Purple";

            if (name == "WallVertical_Blue(Clone)")
                return "WallVertical_Yellow";
            if (name == "WallVertical_Yellow(Clone)")
                return "WallVertical_Red";
            if (name == "WallVertical_Red(Clone)")
                return "WallVertical_Purple";

            if (name == "PawnHouse_Blue(Clone)")
                return "PawnHouse_Yellow";
            if (name == "PawnHouse_Yellow(Clone)")
                return "PawnHouse_Red";
            if (name == "PawnHouse_Red(Clone)")
                return "PawnHouse_Purple";

            if (name == "WarriorHouse_Blue(Clone)")
                return "WarriorHouse_Yellow";
            if (name == "WarriorHouse_Yellow(Clone)")
                return "WarriorHouse_Red";
            if (name == "WarriorHouse_Red(Clone)")
                return "WarriorHouse_Purple";

            if (name == "ArcherHouse_Blue(Clone)")
                return "ArcherHouse_Yellow";
            if (name == "ArcherHouse_Yellow(Clone)")
                return "ArcherHouse_Red";
            if (name == "ArcherHouse_Red(Clone)")
                return "ArcherHouse_Purple";

            if (name == "Tower_Blue(Clone)")
                return "Tower_Yellow";
            if (name == "Tower_Yellow(Clone)")
                return "Tower_Red";
            if (name == "Tower_Red(Clone)")
                return "Tower_Purple";

            if (name == "Castle_Blue(Clone)")
                return "Castle_Yellow";
            if (name == "Castle_Yellow(Clone)")
                return "Castle_Red";
            if (name == "Castle_Red(Clone)")
                return "Castle_Purple";

            if (name == "Repo_Blue(Clone)")
                return "Repo_Yellow";
            if (name == "Repo_Yellow(Clone)")
                return "Repo_Red";
            if (name == "Repo_Red(Clone)")
                return "Repo_Purple";

            else
            {
                Debug.Log("Building Name Not Found");
                return "";
            }
        }
        public string DestructedBuildingName()
        {
            // Building names
            if (name == "WallHorizontal_Blue(Clone)")
                return "WallHorizontal_Blue_Destructed";
            if (name == "WallHorizontal_Yellow(Clone)")
                return "WallHorizontal_Yellow_Destructed";
            if (name == "WallHorizontal_Red(Clone)")
                return "WallHorizontal_Red_Destructed";
            if (name == "WallHorizontal_Purple(Clone)")
                return "WallHorizontal_Purple_Destructed";

            if (name == "WallDoor_Blue(Clone)")
                return "WallDoor_Blue_Destructed";
            if (name == "WallDoor_Yellow(Clone)")
                return "WallDoor_Yellow_Destructed";
            if (name == "WallDoor_Red(Clone)")
                return "WallDoor_Red_Destructed";
            if (name == "WallDoor_Purple(Clone)")
                return "WallDoor_Purple_Destructed";

            if (name == "WallOne_Blue(Clone)")
                return "WallOne_Blue_Destructed";
            if (name == "WallOne_Yellow(Clone)")
                return "WallOne_Yellow_Destructed";
            if (name == "WallOne_Red(Clone)")
                return "WallOne_Red_Destructed";
            if (name == "WallOne_Purple(Clone)")
                return "WallOne_Purple_Destructed";

            if (name == "WallVertical_Blue(Clone)")
                return "WallVertical_Blue_Destructed";
            if (name == "WallVertical_Yellow(Clone)")
                return "WallVertical_Yellow_Destructed";
            if (name == "WallVertical_Red(Clone)")
                return "WallVertical_Red_Destructed";
            if (name == "WallVertical_Purple(Clone)")
                return "WallVertical_Purple_Destructed";

            if (name == "PawnHouse_Blue(Clone)")
                return "PawnHouse_Blue_Destructed";
            if (name == "PawnHouse_Yellow(Clone)")
                return "PawnHouse_Yellow_Destructed";
            if (name == "PawnHouse_Red(Clone)")
                return "PawnHouse_Red_Destructed";
            if (name == "PawnHouse_Purple(Clone)")
                return "PawnHouse_Purple_Destructed";

            if (name == "WarriorHouse_Blue(Clone)")
                return "WarriorHouse_Blue_Destructed";
            if (name == "WarriorHouse_Yellow(Clone)")
                return "WarriorHouse_Yellow_Destructed";
            if (name == "WarriorHouse_Red(Clone)")
                return "WarriorHouse_Red_Destructed";
            if (name == "WarriorHouse_Purple(Clone)")
                return "WarriorHouse_Purple_Destructed";

            if (name == "ArcherHouse_Blue(Clone)")
                return "ArcherHouse_Blue_Destructed";
            if (name == "ArcherHouse_Yellow(Clone)")
                return "ArcherHouse_Yellow_Destructed";
            if (name == "ArcherHouse_Red(Clone)")
                return "ArcherHouse_Red_Destructed";
            if (name == "ArcherHouse_Purple(Clone)")
                return "ArcherHouse_Purple_Destructed";

            if (name == "Tower_Blue(Clone)")
                return "Tower_Blue_Destructed";
            if (name == "Tower_Yellow(Clone)")
                return "Tower_Yellow_Destructed";
            if (name == "Tower_Red(Clone)")
                return "Tower_Red_Destructed";
            if (name == "Tower_Purple(Clone)")
                return "Tower_Purple_Destructed";

            if (name == "Castle_Blue(Clone)")
                return "Castle_Blue_Destructed";
            if (name == "Castle_Yellow(Clone)")
                return "Castle_Yelow_Destructed";
            if (name == "Castle_Red(Clone)")
                return "Castle_Red_Destructed";
            if (name == "Castle_Purple(Clone)")
                return "Castle_Purple_Destructed";

            if (name == "Repo_Blue(Clone)")
                return "Repo_Blue_Destructed";
            if (name == "Repo_Yellow(Clone)")
                return "Repo_Yelow_Destructed";
            if (name == "Repo_Red(Clone)")
                return "Repo_Red_Destructed";
            if (name == "Repo_Purple(Clone)")
                return "Repo_Purple_Destructed";

            else
            {
                Debug.Log("Destructed Building Name Not Found");
                return "";
            }
        }
        public string KnightName()
        {
            // Knight names
            if (name == "PawnHouse_Blue(Clone)")
                return "Pawn_Blue";
            if (name == "PawnHouse_Yellow(Clone)")
                return "Pawn_Yellow";
            if (name == "PawnHouse_Red(Clone)")
                return "Pawn_Red";
            if (name == "PawnHouse_Purple(Clone)")
                return "Pawn_Purple";

            if (name == "WarriorHouse_Blue(Clone)")
                return "Warrior_Blue";
            if (name == "WarriorHouse_Yellow(Clone)")
                return "Warrior_Yellow";
            if (name == "WarriorHouse_Red(Clone)")
                return "Warrior_Red";
            if (name == "WarriorHouse_Purple(Clone)")
                return "Warrior_Purple";

            if (name == "ArcherHouse_Blue(Clone)")
                return "Archer_Blue";
            if (name == "ArcherHouse_Yellow(Clone)")
                return "Archer_Yellow";
            if (name == "ArcherHouse_Red(Clone)")
                return "Archer_Red";
            if (name == "ArcherHouse_Purple(Clone)")
                return "Archer_Purple";

            else
            {
                Debug.Log("Knight Name Not Found");
                return "";
            }
        }
    }
}