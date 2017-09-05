using GW2MH.Core.Memory;
using System;

namespace GW2MH.Core.Data
{
    internal class CharacterData
    {

        public float DefaultMoveSpeed { get; set; }
        public float DefaultGravity { get; set; }
        public bool IsCharacterIngame
        {
            get
            {
                if (Memory != null && Memory.IsRunning)
                {
                    return Memory.Read<IntPtr>(MemoryData.ContextPtr, new int[] { MemoryData.MoveSpeedOffsets[0], MemoryData.MoveSpeedOffsets[1] }) != IntPtr.Zero;
                }
                else
                    return false;
            }
        }

        public MemSharp Memory { get; private set; }

        public CharacterData(MemSharp memory)
        {
            Memory = memory;
        }

    }
}