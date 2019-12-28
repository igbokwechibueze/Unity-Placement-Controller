using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TurretGunAmmo : MonoBehaviour
{
        /// <summary>
        /// Gun whose information is displayed.
        /// </summary>
        [Tooltip("Gun whose information is displayed.")]
        public MountedGun mountedGun;

        /// <summary>
        /// Determines if the display is hidden when there is no gun.
        /// </summary>
        [Tooltip("Determines if the display is hidden when there is no gun.")]
        public bool HideWhenNone = false;

        private Text _text;

        private void Awake()
        {
            _text = GetComponent<Text>();
        }

        private void LateUpdate()
        {
            if (mountedGun== null)
                return;

            if (mountedGun != null)
                _text.text = mountedGun.name + " " + mountedGun.currentAmmo.ToString();
                gameObject.GetComponent<TextMeshPro>().text = mountedGun.name + " " + mountedGun.currentAmmo.ToString();

            if (Application.isPlaying)
            {
                var isVisible = true;

                if (mountedGun == null)
                    isVisible = !HideWhenNone;
                else
                    isVisible = (mountedGun != null || !HideWhenNone);

                _text.enabled = isVisible;
                //gameObject.GetComponent<TextMeshPro>().text.enabled = isVisible;
            }
        }
}
