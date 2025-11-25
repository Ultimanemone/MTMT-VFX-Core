using MTMTVFX.Core;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace MTMTVFX.Internal
{
    internal class SM_LightController : MonoBehaviour
    {
        public static bool bEnable;
        public static List<Light> SMLC_LightList = new List<Light>();
        public float FadeSpd = 1f;
        public Color StartColor;
        private float time = 1f;
        private Light WaterLight;
        private Light MyLight;
        private GameObject child;
        private float BaseRange;

        public void Start()
        {
            MyLight = GetComponent<Light>();
            BaseRange = MyLight.range;
            GameObject prefab;
            CorePlugin.GetAsset("LightChild", out prefab);
            child = Instantiate<GameObject>(prefab, transform);
            WaterLight = child.GetComponent<Light>();
            WaterLight.cullingMask = 1 << LayerMask.NameToLayer("Water");
            SM_LightController.SMLC_LightList.Add(MyLight);
            SM_LightController.SMLC_LightList.Add(WaterLight);
            FadeSpd = 1f;
            MyLight.color = StartColor;
            time = 1f;
        }

        public void Update()
        {
            time -= FadeSpd * Time.deltaTime;
            MyLight.color = Color.Lerp(new Color(0f, 0f, 0f, 1f), StartColor, Mathf.Max(0f, time));
            MyLight.range = BaseRange * time * Mathf.Max(1f, transform.parent.localScale.x);
            WaterLight.color = MyLight.color;
            WaterLight.intensity = MyLight.intensity * 1f;
            WaterLight.range = MyLight.range * 2f;
        }
    }
}
