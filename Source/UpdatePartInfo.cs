using System;
using System.Collections.Generic;
//using System.Data.SqlTypes;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using UnityEngine;
using System.Text.RegularExpressions;
using KSP.UI.Screens; // For "ApplicationLauncherButton"

namespace KRnD
{
    [KSPAddon(KSPAddon.Startup.SpaceCentre, false)]
    class UpdatePartInfo : MonoBehaviour
    {
        // This happens every time Space Center is entered, may be a place of optimization
        // if it take noticable time
        void Start()
        {
            for (int i = 0; i < PartLoader.LoadedPartsList.Count; i++)
            {
                AvailablePart ap = PartLoader.LoadedPartsList[i];
                var part = ap.partPrefab;
                for (int i1 = 0; i1 < ap.moduleInfos.Count; i1++)
                {
                    var m = ap.moduleInfos[i1];

                    if (m.moduleName == "R&D")
                    {
                        AvailablePart.ModuleInfo info = m;
                        m.info = KRnDModule.GetInfo(part);
                        break;
                    }
                }
            }
        }
    }
}
