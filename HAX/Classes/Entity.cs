using System;
using System.Numerics;

namespace HAX.Classes
{
    public class Entity
    {
        public Vector2 originPosition;
        public Vector2 headPosition;
        public int Index;

        public Entity(int Index) {
            this.Index = Index;
        }

        public int Health
        {
            get
            {
                return Memory.ReadMemory<int>(this.PawnAddress + Offsets.m_iHealth);
            }
        }

        public Team Team
        {
            get
            {
                return (Team)Memory.ReadMemory<int>(this.EntityAddress + Offsets.m_iTeamNum);
            }
        }

        public int PawnId
        {
            get
            {
                return Memory.ReadMemory<int>(this.EntityAddress + Offsets.m_hPlayerPawn);
            }
        }

        public Vector3 GetBonePosition(Bones Bone)
        {
            IntPtr BoneArray = Memory.ReadMemory<IntPtr>(this.GameSceneNode + 0x160 + 0x80);
            Vector3 BonePosition = Memory.ReadMemory<Vector3>(BoneArray + (int)Bone * 32);
            return BonePosition;
        }

        public Vector2 GetBonePosition2D(Bones Bone)
        {
            Vector3 pos = this.GetBonePosition(Bone);
            return Utils.WorldToScreen(pos);
        }

        public Vector3 Position
        {
            get
            {
                return Memory.ReadMemory<Vector3>(this.PawnAddress + Offsets.m_vOldOrigin);
            }
        }

        public Vector2 Position2D
        {
            get
            {
                return Utils.WorldToScreen(this.Position);
            }
        }

        public IntPtr GameSceneNode
        {
            get
            {
                return Memory.ReadMemory<IntPtr>(this.PawnAddress + Offsets.m_pGameSceneNode);
            }
        }

        public IntPtr PawnAddress
        {
            get
            {
                IntPtr listEntry2 = Memory.ReadMemory<IntPtr>(Main.EntityListAddress + 0x8 * ((this.PawnId & 0x7FFF) >> 9) + 16);
                return Memory.ReadMemory<IntPtr>(listEntry2 + 120 * (this.PawnId & 0x1FF));
            }
        }

        public IntPtr EntityAddress
        {
            get
            {
                return Memory.ReadMemory<IntPtr>(this.ListEntry + (120 * (this.Index & 0x1FF)));
            }
        }

        public IntPtr ListEntry
        {
            get
            {
                return Memory.ReadMemory<IntPtr>(Main.EntityListAddress + (8 * (this.Index & 0x7FFF) >> 9) + 16);
            }
        }
    }
}
