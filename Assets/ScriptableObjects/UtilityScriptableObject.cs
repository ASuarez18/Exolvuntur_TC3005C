using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace player.Weapons
{
    [CreateAssetMenu(fileName = "Weapon_", menuName = "ScriptableObjects/UtilityScriptableObject", order = 1)]
    public class UtilityScriptableObject : ScriptableObject
    {
        [SerializeField, Tooltip("type of weapon")]
        private WeaponsEnum _weaponType;

        [SerializeField, Tooltip("range of weapon")]
        private float _range =2f ;

        [SerializeField, Tooltip("damage of weapon")]

        private float _damage=2f;

        [SerializeField, Tooltip("Force of weapon")]
        private float _force = 85f;

        //data Getters
        public WeaponsEnum weaponType => _weaponType;

        public float Range => _range;

        public float Damage => _damage;
        
        public float Force => _force;


        [Space(10)]
        [SerializeField]
        private Sprite _weaponSprite;

        [SerializeField]
        private GameObject _weaponPrefab;

        public Sprite WeaponSprite => _weaponSprite;

        public GameObject WeaponPrefab => _weaponPrefab;

        
        

    }
}
