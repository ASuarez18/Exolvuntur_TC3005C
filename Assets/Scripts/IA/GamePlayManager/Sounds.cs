using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enemy.Manager;
using Enemy.Behaviour;

namespace GamePlay.IA
{
    public static class Sounds
    {
        public static void MakeSound(SoundGame sound)
        {
            Collider[] col = Physics.OverlapSphere(sound.pos , sound.range);

            for(int i=0; i<col.Length; i++)
            {
                if(col[i].TryGetComponent(out EnemyKormosManager enemyState))
                {
                    Debug.LogWarning("Encontro enemigo");
                    enemyState.dectedsound(sound.pos);
                }
            }

        }

        //Dibujamos un wire esfera
        public static void DrawWireSphere(Vector3 pos, float range)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(pos, range);
        }
    }

}
