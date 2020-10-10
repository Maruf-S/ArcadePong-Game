using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
public class RefreshVideoContentSettings : MonoBehaviour
{
    public Toggle fullScreen;
    public Dropdown dropdown;
    public Toggle Vsync;
    public void RefreshVideoSettings(){
        dropdown.ClearOptions();
        //Ignore refresh rates so not to get duplicate resolutions
        var resolutions = Screen.resolutions.Select(resolution => new Resolution { width = resolution.width, height = resolution.height, refreshRate = 0}).Distinct().ToArray();
        List<string> m_DropOptions = new List<string>();
        int selectedRes = 0;
        for (int s = resolutions.Length-1; s >0; s--)
        {
            if(resolutions.Length-s>6){
                break;
            }
            if(resolutions[s].width == Screen.currentResolution.width && resolutions[s].height == Screen.currentResolution.height){
                selectedRes = resolutions.Length -s-1;
                // Debug.Log(resolutions[selectedRes].width + "x" + resolutions[selectedRes].height);
            }
            m_DropOptions.Add($"{resolutions[s].width}X{resolutions[s].height}");
        }
        dropdown.AddOptions(m_DropOptions);
        dropdown.RefreshShownValue();
        dropdown.value = selectedRes;

        fullScreen.isOn = Screen.fullScreen;
        Vsync.isOn = IntToBool(QualitySettings.vSyncCount);
    }
    private static int BoolToInt(bool value){
			return value ? 1 : 0;
		}
    private static bool IntToBool(int value){
			return value == 0 ? false : true;
		}
}
