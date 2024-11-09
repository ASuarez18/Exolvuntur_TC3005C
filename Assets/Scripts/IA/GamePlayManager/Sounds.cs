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
                    //Primero verificamos si el enemigo se encuntra en Idle
                    if(enemyState.enemyMachine.currentState is KormosCaution CautionState)
                    {
                        CautionState.playerSound =true;
                    }
                    else if(enemyState.enemyMachine.currentState is KormosHunt HuntState)
                    {
                        // Debug.Log(sound.pos + sound.range);
                        HuntState.Hunt(sound);

                    }
                }
            }

        }
    }

}
