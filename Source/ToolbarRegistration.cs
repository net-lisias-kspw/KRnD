using UnityEngine;

using ToolbarControl_NS;

namespace KRnD
{
    [KSPAddon(KSPAddon.Startup.MainMenu, true)]
    public class RegisterToolbar : MonoBehaviour
    {
        void Start()
        {
            ToolbarControl.RegisterMod(KRnDGUI.MODID, KRnDGUI.MODNAME);
        }
    }
}