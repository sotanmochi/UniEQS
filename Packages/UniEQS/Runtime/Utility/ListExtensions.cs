using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace UniEQS.Utility
{
    public static class ListExtensions
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Span<T> AsSpan<T>(this List<T> list)
        {
            return Unsafe.As<InternalArrayReference<T>>(list).Items.AsSpan(0, list.Count);
        }
        
        private class InternalArrayReference<T>
        {
            public T[] Items;
        }
    }
}