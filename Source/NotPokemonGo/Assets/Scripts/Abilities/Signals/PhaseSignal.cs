using System;

namespace Abilities.Signals
{
    public enum PhaseSignal
    {
        None = 0,

        Attack1 = 10,
        Attack2 = 11,
        Attack3 = 12,
        Attack4 = 13,
        Attack5 = 14,
        Attack6 = 15,

        Particle1 = 20,
        Particle2 = 21,
        Particle3 = 22,

        Launch = 30,
        Impact = 31,

        Finish = 99,
    }

    public static class PhaseSignalUtil
    {
        public static PhaseSignal FromInt(int id)
        {
            return Enum.IsDefined(typeof(PhaseSignal), id)
                ? (PhaseSignal)id
                : PhaseSignal.None;
        }
    }
}