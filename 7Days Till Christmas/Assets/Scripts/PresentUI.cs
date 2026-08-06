using UnityEngine;
using UnityEngine.UIElements;

public class PresentUI : MonoBehaviour
{
    [SerializeField] private UIDocument uiDocument;
    private VisualElement Bar;

    void Start()
    {
        Bar = uiDocument.rootVisualElement.Q<VisualElement>("Panel")
            .Q<VisualElement>("Container").Q<VisualElement>("Bar");
    }

    public void UpdateBar(float healthPercentage)
    {
        if (healthPercentage > 100f) return;

        if (healthPercentage < 0f) healthPercentage = 0f;
        Bar.style.width = new Length(healthPercentage, LengthUnit.Percent);
        
    }

    public void ResetBar()
    {
        Bar.style.width = new Length(100, LengthUnit.Percent);
    }
}
