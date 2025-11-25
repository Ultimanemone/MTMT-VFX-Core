using System.Collections.Generic;
using UnityEngine;


namespace MTMTVFX.Internal
{
    internal class SM_LightController : MonoBehaviour
    {
        public static bool bEnable;
        public static List<Light> SMLC_LightList = new List<Light>();
        public float lifetime = 1f;
        public Color StartColor;

        [SerializeField] private GameObject lightPrefab;
        private Light WaterLight;
        private Light MyLight;
        private GameObject child;
        private float BaseRange;

        private void Start()
        {
            MyLight = GetComponent<Light>();
            BaseRange = MyLight.range;
            child = Instantiate<GameObject>(lightPrefab);
            WaterLight = child.GetComponent<Light>();
            WaterLight.cullingMask = 1 << LayerMask.NameToLayer("Water");
            SMLC_LightList.Add(MyLight);
            SMLC_LightList.Add(WaterLight);
            MyLight.color = StartColor;
        }

        private void Update()
        {
            lifetime = Mathf.Max(0, lifetime - Time.deltaTime);
            MyLight.color = Color.Lerp(new Color(0f, 0f, 0f, 1f), StartColor, Mathf.Max(0f, lifetime));
            MyLight.range = BaseRange * lifetime * Mathf.Max(1f, transform.parent.localScale.x);
            WaterLight.color = MyLight.color;
            WaterLight.intensity = MyLight.intensity * 1f;
            WaterLight.range = MyLight.range * 2f;
        }
    }
}
