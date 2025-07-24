using UnityEngine;
using System.Collections;

namespace Characters.Actions
{
	public interface IActionHandler
	{
		void Handle(Action action);
	}
}