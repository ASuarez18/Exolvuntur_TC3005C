using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GamePlay.IA
{
    public class SoundGame
    {
        public enum SoundType{Deafult = -1, Interesting,Danger};
        public SoundGame(Vector3 _pos , float _range)
        {
            pos = _pos;
            range = _range;
        }

        public SoundType soundType;
        public readonly Vector3 pos;
        public readonly float range;
    }

}
