using System;
using UnityEngine;

namespace MTMTVFX.Internal
{
    public class Fadeout : MonoBehaviour
    {
        public float time;
        public float TgtAlpha;
        private Material mat;
        private Color initialColor;
        private Color targetColor;
        private float initialAlpha;

        private void Start()
        {
            LineRenderer component = GetComponent<LineRenderer>();
            mat = component.material;
            targetColor = initialColor = mat.GetColor("_TintColor");
            initialAlpha = initialColor.a;
            targetColor.a = TgtAlpha;
        }

        private void OnEnable()
        {
            mat.SetColor("_TintColor", initialColor);
            time = 0f;
        }

        private void Update()
        {
            time += Time.deltaTime;
            mat.SetColor("_TintColor", Color.Lerp(initialColor, targetColor, Mathf.Min(1f, time)));
        }
    }
}
