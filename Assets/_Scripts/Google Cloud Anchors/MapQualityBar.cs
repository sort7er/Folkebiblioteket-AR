using UnityEngine;

public class MapQualityBar : MonoBehaviour
{


    public float colorCurve;
    public Renderer meshRenderer;

    public Color initialColor = Color.white;
    public Color lowQualityColor = Color.red;
    public Color mediumQualityColor = Color.yellow;
    public Color highQualityColor = Color.green;

    private bool isVisited = false;
    private int state = 0;
    private float alpha = 1.0f;


    public bool IsVisited
    {
        get
        {
            return isVisited;
        }

        set
        {
            isVisited = value;

            //Animator.SetBool(paramIsVisited, isVisited);
        }
    }
    public int QualityState
    {
        get
        {
            return state;
        }

        set
        {
            state = value;
            transform.localScale = new Vector3(1, 1 + state, 1) * 0.01f;
            //Animator.SetInteger(paramQuality, state);
        }
    }
    public float Weight
    {
        get
        {
            if (IsVisited)
            {
                switch (state)
                {
                    case 0: return 0.1f; // Insufficient quality
                    case 1: return 0.5f; // Sufficient quality
                    case 2: return 1.0f; // Good quality
                    default: return 0.0f;
                }
            }
            else
            {
                return 0.0f;
            }
        }
    }
    public void SetAlpha(float alpha)
    {
        this.alpha = alpha;
    }

    public void Update()
    {
        // Sync map quality bar color with current state animation.

        Color color = initialColor;
        if (state == 0)
        {
            color = Color.Lerp(initialColor, lowQualityColor, colorCurve);
        }
        else if (state == 1)
        {
            color = Color.Lerp(lowQualityColor, mediumQualityColor, colorCurve);
        }
        else if (state == 2)
        {
            color = Color.Lerp(mediumQualityColor, highQualityColor, colorCurve);
        }

        color.a = alpha;
        meshRenderer.material.color = color;
    }
}
