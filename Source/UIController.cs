using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JetBrains.Annotations;
using KSP.UI.Screens;
using UnityEngine;
using UnityEngine.Assertions;

namespace KRnD.Source
{
	///////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	/// <summary> Handles the button that appears in the button-bar. This is the only way the player
	/// 		  can bring up the Mod's UI.</summary>
	[KSPAddon(KSPAddon.Startup.SpaceCentre, true)]
	internal class UIController : MonoBehaviour
	{
		///////////////////////////////////////////////////////////////////////////////////////////////////////////////
		/// <summary> The toolbar button object that is used to bring up (and dismiss) the mod UI window.</summary>
		private ApplicationLauncherButton _toolbarButton;


		///////////////////////////////////////////////////////////////////////////////////////////////////////////////
		/// <summary> Reference to the singleton version of this object. There should be only one of these
		/// 		  since it manages the application button.</summary>
		///
		/// <value> The instance.</value>
		public static UIController Instance { get; private set; }


		///////////////////////////////////////////////////////////////////////////////////////////////////////////////
		/// <summary> Initializes this object and makes sure this object remains a singleton.</summary>
		[UsedImplicitly]
		private void Awake()
		{
			if (Instance != null && Instance != this) {
				Destroy(this);
				return;
			}

			Assert.IsTrue(Instance == null, "Awake apparently being called twice for the same object.");

			DontDestroyOnLoad(this);
			Instance = this;
			GameEvents.onGUIApplicationLauncherReady.Add(OnGUIApplicationLauncherReady);
			GameEvents.onGameSceneSwitchRequested.Add(OnGameSceneSwitchRequested);
		}


		///////////////////////////////////////////////////////////////////////////////////////////////////////////////
		/// <summary> Executes the destroy action. </summary>
		[UsedImplicitly]
		private void OnDestroy()
		{
			GameEvents.onGUIApplicationLauncherReady.Remove(OnGUIApplicationLauncherReady);
			GameEvents.onGameSceneSwitchRequested.Remove(OnGameSceneSwitchRequested);
		}


		///////////////////////////////////////////////////////////////////////////////////////////////////////////////
		/// <summary> Handles clicking on the application button. This will bring up the Mod's UI if hidden
		/// 		  or hide it if visible. It ensures that the settings window will also hide if the
		/// 		  main UI becomes hidden.</summary>
		private void ApplicationButtonClicked()
		{
			UpgradeUI.showGui = !UpgradeUI.showGui;
		}


		///////////////////////////////////////////////////////////////////////////////////////////////////////////////
		/// <summary> Executes the game scene switch requested action to make sure that the Mod's button
		/// 		  does not appear in scenes it should not appear in, such as the Main Menu.</summary>
		///
		/// <param name="data"> Specification of the scene being switch from and to.</param>
		private void OnGameSceneSwitchRequested(GameEvents.FromToAction<GameScenes, GameScenes> data)
		{
			if (_toolbarButton == null) return;
			UpgradeUI.showGui = false;

			UpgradeUI.selectedPart = null;


			// If going to the main menu, then make sure the button has been removed.
			if (data.to == GameScenes.MAINMENU) {
				ApplicationLauncher.Instance.RemoveModApplication(_toolbarButton);
				_toolbarButton = null;
			}
		}


		///////////////////////////////////////////////////////////////////////////////////////////////////////////////
		/// <summary> Executes the graphical user interface application launcher ready action which lets
		/// 		  the mod know when it is ok to add the button to the toolbar. The button is added
		/// 		  to the toolbar by this method.</summary>
		public void OnGUIApplicationLauncherReady()
		{
			// The button should appear in the toolbar only when not at the Main Menu and only for Career games.
			if (HighLogic.CurrentGame.Mode != Game.Modes.CAREER || HighLogic.LoadedScene == GameScenes.MAINMENU) return;

			if (_toolbarButton == null) {
				var texture_file = StringConstants.MOD_DIRECTORY + StringConstants.APP_ICON;

				_toolbarButton = ApplicationLauncher.Instance.AddModApplication(ApplicationButtonClicked,
					ApplicationButtonClicked, null, null,
					null, null, ApplicationLauncher.AppScenes.SPH | ApplicationLauncher.AppScenes.VAB,
					GameDatabase.Instance.GetTexture(texture_file, false));
			}
		}
	}
}
