﻿using System;
using System.Collections.Generic;
using System.Text;

namespace MultiProcessorExtensions
{
    /// <summary>
    /// Represents a processor group-specific affinity, such as the affinity of a thread.
    /// </summary>
    public sealed class GroupAffinity
    {
        /// <summary>
        /// A bitmap that specifies the affinity for zero or more processors within the specified group.
        /// </summary>
        public UIntPtr Mask { get; private set; }

        /// <summary>
        /// The processor group number.
        /// </summary>
        public UInt16 Group { get; private set; }

        internal GroupAffinity(UInt16 group, UIntPtr mask)
        {
            this.Group = group;
            this.Mask = mask;
        }

        internal GroupAffinity(GROUP_AFFINITY affinity)
        {
            this.Mask = affinity.Mask;
            this.Group = affinity.Group;
        }

        public bool IsProcessorActive(int maskIndex)
        {
            // check that the given index is a valid bit offset in the mask
            int totalMaskBits = (UIntPtr.Size * 8) - 1;
            if (maskIndex < 0 || maskIndex > totalMaskBits)
            {
                throw new ArgumentOutOfRangeException(nameof(maskIndex), $"Mask index was not in the range 0 to {totalMaskBits}.");
            }

            // return true if the bit is set in the mask
            return (Mask.ToUInt64() & (1u << maskIndex)) != 0;
        }
    }
}
