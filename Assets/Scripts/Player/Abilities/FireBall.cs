using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.Player.Abilities {
    class FireBall : IAbilityTemplate {
        public int calculateManaCost(AAbilityActivation activation, HexCell cell) {
            return 10;
        }

        public void execute(AAbilityActivation activation) {

        }

        public AAbilityActivation makeAbilityActivation(HexCell cell, KeyCode keyCode) {
            AAbilityActivation activation = new SingleUnitTargetAbilityActivation(this);
            activation.clickedCell(cell, keyCode);
            return activation;
            
        }
    }
}
