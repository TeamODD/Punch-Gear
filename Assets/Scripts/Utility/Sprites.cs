using System.Collections;

using UnityEngine;

namespace PunchGear.Utility
{
    public static class Sprites
    {
        public static Coroutine StartBlink(
            this MonoBehaviour monoBehaviour,
            SpriteRenderer spriteRenderer,
            int count,
            float blinkRate,
            bool recover = false)
        {
            Material material = spriteRenderer.material;
            return monoBehaviour.StartCoroutine(StartBlinkAnimation(material, count, blinkRate, recover));
        }

        private static IEnumerator StartBlinkAnimation(
            Material material,
            int count,
            float blinkRate,
            bool recover)
        {
            Color originalColor = material.color;
            float originalAlpha = originalColor.a;
            for (int i = 0; i < count; i++)
            {
                yield return new WaitForSeconds(blinkRate);
                originalColor.a = 0;
                material.color = originalColor;
                yield return new WaitForSeconds(blinkRate);
                originalColor.a = originalAlpha;
                material.color = originalColor;
            }
            yield return new WaitForSeconds(blinkRate);
            originalColor.a = recover ? originalAlpha : 0;
            material.color = originalColor;
        }
    }
}
