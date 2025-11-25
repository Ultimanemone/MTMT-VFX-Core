using UnityEngine;


namespace BMEffects_Remaster.Effects.Projectiles
{
    public class SM_Fadeout : MonoBehaviour
    {
        public float time;
        public float spd = 1f;
        public float TgtAlpha;
        private Material mat;
        private Color DefColor;
        private Color TgtColor;
        private float DefAlpha;

        private void Start()
        {
            Renderer component = GetComponent<Renderer>();
            mat = component.material;
            DefColor = mat.GetColor("_TintColor");
            DefAlpha = DefColor.a;
            TgtColor = DefColor;
            TgtColor.a = TgtAlpha;
        }

        private void Update()
        {
            if (transform.parent.parent.GetComponent<SM_AutoKill>().nowInit)
            {
                mat.SetColor("_TintColor", DefColor);
                time = 0f;
            }
            time += transform.parent.parent.GetComponent<SM_AutoKill>().deltaTime * spd;
            mat.SetColor("_TintColor", Color.Lerp(DefColor, TgtColor, Mathf.Min(1f, time)));
        }
    }
}
