using UnityEngine;
using UnityEditor;

namespace Characters.Actions
{
	public class Action
	{
        public enum Type
        {
            MoveOnGround,
            MoveOnAir,
            AttackMelee,
            AttackRange
        }
        public Type type;
        public CharacterFacade targetCharacter;
        public Vector3 targetPos;
	}
}