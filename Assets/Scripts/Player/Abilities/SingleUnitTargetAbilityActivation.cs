using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.Player.Abilities {
    class SingleUnitTargetAbilityActivation : AAbilityActivation {

        public BaseUnit Target;

        public SingleUnitTargetAbilityActivation(IAbilityTemplate templatedFrom) : base(templatedFrom) {
        }

        public override void clickedCell(HexCell cell, KeyCode key) {
            if(cell != null && cell.Unit != null) {
                Target = cell.Unit;
            }
        }

        public override bool isReady() {
            return (Target != null);
        }
    }
}
