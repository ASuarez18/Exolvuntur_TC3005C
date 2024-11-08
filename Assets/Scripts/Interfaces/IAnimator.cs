using UnityEngine;


namespace Interfaces
{
    public interface IAnimator
    {
        public Animator Animator { get; set; }

        /// <summary>
        /// Moves the character in the given direction
        /// </summary>
        /// <param name="direction"></param>
        public void SetFloat(string name, float value);

        /// <summary>
        /// Character jumps
        /// </summary>
        /// <returns></returns>
        public void SetBool(string name, bool value);

        public void SetInteger(string name, int value);


        /// <summary>
        /// Character is grounded
        /// </summary>
        /// 
        public void SetTrigger(string name);
        public void PlayAnimationByHash(int hash);

    }


}

