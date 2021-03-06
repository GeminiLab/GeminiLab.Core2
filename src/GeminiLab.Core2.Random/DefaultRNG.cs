using System;
using System.Runtime.InteropServices;
using GeminiLab.Core2.Random.RNG;

namespace GeminiLab.Core2.Random {
    public static class DefaultRNG {
        private static readonly IRNG<int> InnerOne;
        private static readonly IRNG<ulong> InnerOneU64;

        public static Tuple<ulong, ulong> GetDefaultRandomSeed() {
            ulong seed0 = 0x0fe12dc34ba56987ul;
            ulong seed1 = 0x02468acefdb97531ul;

            unsafe {
                var p = Marshal.AllocHGlobal(8);
                seed0 ^= (ulong)p.ToInt64();
                seed1 ^= *(ulong*)p.ToPointer();
                Marshal.FreeHGlobal(p);

                // be careful working on stack!!!
                var q = 16 + (byte*)&seed0;
                seed1 ^= (ulong)q;
                seed0 ^= *(ulong*)q;
            }

            unchecked {
                seed0 ^= (ulong)DateTime.UtcNow.Ticks << ((Environment.ProcessorCount + Environment.CurrentManagedThreadId) & 32);
                seed1 ^= (ulong)Environment.CurrentDirectory.GetHashCode();

                for (int i = 0; i < 16; ++i) {
                    seed1 ^= (ulong)DateTime.Now.Ticks << 32;
                    seed0 ^= ((ulong)$"{seed0 - seed1:x16}".GetHashCode() << 32) | (uint)$"{seed0 + seed1:x16}".GetHashCode();
                    seed1 ^= ((ulong)$"{seed0 + seed1:x16}".GetHashCode() << 32) | (uint)$"{seed1 - seed0:x16}".GetHashCode();
                    seed0 ^= (ulong)Environment.TickCount;
                }
            }

            return new Tuple<ulong, ulong>(seed0, seed1);
        }

        static DefaultRNG() {
            var (seed0, seed1) = GetDefaultRandomSeed();

            InnerOne = new PCG(seed0, seed1);
            InnerOneU64 = InnerOne.AsU64RNG();
        }

        public static int Next() {
            lock (InnerOne) {
                return InnerOne.Next();
            }
        }
        public static uint NextU32() {
            lock (InnerOne) {
                return unchecked((uint)InnerOne.Next());
            }
        }

        public static ulong NextU64() {
            lock (InnerOne) {
                return InnerOneU64.Next();
            }
        }

        public static double NextDouble() {
            lock (InnerOne) {
                return InnerOne.NextDouble();
            }
        }

        public static IRNG<int> I32 { get; } = new DefaultI32RNG();

        public static IRNG<ulong> U64 { get; } = new DefaultU64RNG();

        public static IRNG<bool> Coin { get; } = new Coin();
    }

    internal class DefaultI32RNG : IRNG<int> {
        public int Next() {
            return DefaultRNG.Next();
        }
    }

    internal class DefaultU64RNG : IRNG<ulong> {
        public ulong Next() {
            return DefaultRNG.NextU64();
        }
    }

    internal class Coin : IRNG<bool> {
        public bool Next() {
            return DefaultRNG.Next() % 2 == 0;
        }
    }
}
