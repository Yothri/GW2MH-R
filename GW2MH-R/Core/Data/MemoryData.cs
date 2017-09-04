using System;

namespace GW2MH.Core.Data
{
    internal static class MemoryData
    {

        public static readonly string ContextCalcPattern = "8B 0D CA 90 B7 01";
        public static readonly int ContextCalcJumpPatchOffset = 6;
        public static byte[] ContextCalcJumpPatch(IntPtr jmpLocation)
        {
            var b = BitConverter.GetBytes(jmpLocation.ToInt64());

            return new byte[] 
            {
                0x56,                                                       // PUSH RSI
                0x48, 0xbe, b[0], b[1], b[2], b[3], b[4], b[5], b[6], b[7], // MOVABS RSI, jmpLocation
                0xff, 0xe6,                                                 // JMP RSI
                0x5E,                                                       // POP RSI
                0xC3,                                                       // RET
                0xCC,                                                       // INT 3
                0xCC,                                                       // INT 3
                0xCC,                                                       // INT 3
                0xCC,                                                       // INT 3
                0xCC,                                                       // INT 3
                0xCC,                                                       // INT 3
                0xCC,                                                       // INT 3
                0xCC,                                                       // INT 3
            };
        }
        public static byte[] ContextCalcJumpShellCode(IntPtr pointerLocation, IntPtr jumpBackLocation)
        {
            var b = BitConverter.GetBytes(pointerLocation.ToInt64());
            var b2 = BitConverter.GetBytes(jumpBackLocation.ToInt64());

            return new byte[]
            {
                0x65, 0x48, 0x8B, 0x04, 0x25, 0x58, 0x00, 0x00, 0x00,               // MOV RAX,GS:[00000058]
                0xBA, 0x08, 0x00, 0x00, 0x00,                                       // MOV EDX,00000008
                0x48, 0x8B, 0x04, 0xC8,                                             // MOV RAX,[RAX+RCX*8]
                0x48, 0x8B, 0x04, 0x02,                                             // MOV RAX,[RDX+RAX]
                0x48, 0xA3, b[0], b[1], b[2], b[3], b[4], b[5], b[6], b[7],         // MOV DS:[pointerLocation],RAX
                0x48, 0xbe, b2[0], b2[1], b2[2], b2[3], b2[4], b2[5], b2[6], b2[7], // MOVABS RSI, jumpBackLocation
                0xff, 0xe6,                                                         // JMP RSI
            };
        }
        public static readonly byte[] ContextCalcRestore = new byte[]
        {
            0x65, 0x48, 0x8B, 0x04, 0x25, 0x58, 0x00, 0x00, 0x00,           // MOV RAX,GS:[00000058]
            0xBA, 0x08, 0x00, 0x00, 0x00,                                   // MOV EDX,00000008
            0x48, 0x8B, 0x04, 0xC8,                                         // MOV RAX,[RAX+RCX*8]
            0x48, 0x8B, 0x04, 0x02,                                         // MOV RAX,[RDX+RAX]
            0xC3                                                            // RET
        };

        public static IntPtr ContextPtr { get; set; }

        public static readonly int[] MoveSpeedOffsets = new int[] 
        {
            144, // ChCliContext
            152, // Character
            136, // Agent
            64, // Transform
            564 // MaxSpeed
        };

        public static readonly int[] GravityOffsets = new int[]
        {
            144, // ChCliContext
            152, // Character
            136, // Agent
            64, // Transform
            532 // Gravity
        };
    }
}