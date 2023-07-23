#if UNITY_EDITOR
using UnityEditor;
#endif

using Helpers;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

// Sets the script to be executed later than all default scripts
// This is helpful for UI, since other things may need to be initialized before setting the UI
[DefaultExecutionOrder(1000)]
public class MenuUIHandler : MonoBehaviour
{
    public ColorPicker ColorPicker;
    private ColorSaver colorSaver;

    public void NewColorSelected(Color color)
    {
        // add code here to handle when a color is selected
        colorSaver.TeamColor = color;
    }

    [Inject]
    private void Construct(ColorSaver colorSaverRef)
    {
        colorSaver = colorSaverRef;
    }
    private void Start()
    {
        ColorPicker.Init();
        //this will call the NewColorSelected function when the color picker have a color button clicked.
        ColorPicker.onColorChanged += NewColorSelected;
        ColorPicker.SelectColor(colorSaver.TeamColor);
       
    }
    public void StartNew()
    {
        SceneManager.LoadScene(1);
    }
    public void Exit()
    {
#if UNITY_EDITOR
        EditorApplication.ExitPlaymode();
#else
        Application.Quit();
#endif
        colorSaver.SaveColor();
    }

    public void SaveColorPicked()
    {
        colorSaver.SaveColor();
    }
    public void LoadColorPicked()
    {
        colorSaver.LoadColor();
        ColorPicker.SelectColor(colorSaver.TeamColor);
    }
}
