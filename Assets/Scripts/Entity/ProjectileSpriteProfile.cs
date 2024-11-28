using System;

using UnityEngine;

namespace PunchGear.Entity
{
    [Serializable]
    public class ProjectileSpriteProfile
    {
        [field: SerializeField]
        public Sprite DefaultImage { get; private set; }

        [field: SerializeField]
        public Sprite AssembleImage { get; private set; }

        [field: SerializeField]
        public Sprite DisassembleImage { get; private set; }
    }
}
