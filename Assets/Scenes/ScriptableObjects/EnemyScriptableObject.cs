using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Enemy.Stats
{
    [CreateAssetMenu(fileName = "EnemyScriptableObject", menuName = "EnemyScriptableObject", order = 0)]
    public class EnemyScriptableObject : ScriptableObject
    {
        //Estadisticas de movimiento
        [SerializeField, Tooltip("Velocidad del enemigo al caminar")]
        private float _speed = 5.0f;

        [SerializeField, Tooltip("Velocidad del enemigo al correr")]
        private float _runningSpeed = 10.0f;

        [SerializeField, Tooltip("Aceleracion del enemigo")]
        private float _acceleration = 5.0f;

        //Estadisticas de percepcion

        [SerializeField, Tooltip("Rango de vision del enemigo")]
        private float _viewRange = 10.0f;

        [SerializeField, Tooltip("Rango de ataque del enemigo")]
        private float _attackRange = 3.0f;

        [SerializeField, Tooltip("Tiempo que dura el enemigo antes de entrar en modo agresivo")]
        private float _aggroTime = 3.0f;

        [SerializeField, Tooltip("Timepo que dura el estado agresivo")]
        private float _aggroDuration = 5.0f;

        //Estadisticas fisicas

        [SerializeField, Tooltip("Vida del enemigo")]
        private float _health = 100.0f;

        //Data Getters
        public float Speed => _speed;
        public float RunningSpeed => _runningSpeed;
        public float Acceleration => _acceleration;
        public float ViewRange => _viewRange;
        public float AttackRange => _attackRange;
        public float AggroTime => _aggroTime;
        public float AggroDuration => _aggroDuration;
        public float Health => _health;

    }
}


