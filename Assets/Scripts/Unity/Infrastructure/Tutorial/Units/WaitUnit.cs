using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using Unity.Infrastructure.Tutorial.Units.BaseUnits;
using Unity.VisualScripting;
using UnityEngine;

namespace Unity.Infrastructure.Tutorial.Units {
	
	[UsedImplicitly]
	[UnitCategory("Custom")]
	[UnitTitle("Wait")]
	public class WaitUnit : AsyncCustomUnit {
		
		private const string DURATION = "duration";
		

		protected override IEnumerable<IUnitValuePort> DefineValuePortsInternal() {
			yield return ValueInput<float>(DURATION, 1);
		}
		
		protected override IEnumerator OnExecute(Flow flow) {
			Debug.Log("Wait unit started");
			float duration = GetValue<float>(flow, DURATION);
			yield return new WaitForSeconds(duration);
			Debug.Log("Wait unit complete");
		}
	}
}