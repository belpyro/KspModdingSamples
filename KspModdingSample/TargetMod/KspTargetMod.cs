using System;
using System.Linq;
using KSP.UI.Screens;
using UnityEngine;

namespace KspModdingSample
{
    [KSPAddon(KSPAddon.Startup.Flight, false)]
    public class KspTargetMod : MonoBehaviour
    {
        private static readonly Callback EmptyCallback = () => { };

        private ApplicationLauncherButton _button;

        private PopupDialog _dialog;

        private void Awake()
        {
            var texture = GameDatabase.Instance.GetTexture("KspDebugMod/TargetMod/Icon/target", false) ??
                          new Texture2D(24, 24);
            _button = ApplicationLauncher.Instance.AddModApplication(OnTrue, OnFalse,
                EmptyCallback, EmptyCallback
                , EmptyCallback, EmptyCallback,
                ApplicationLauncher.AppScenes.MAPVIEW | ApplicationLauncher.AppScenes.FLIGHT, texture);
        }

        private void OnFalse()
        {
            if (_dialog == null) return;
            _dialog.Dismiss();
            _dialog = null;
        }

        private void OnTrue()
        {
            if (!FlightGlobals.ready || FlightGlobals.ActiveVessel == null) return;

            DialogGUIBase guiBase = new DialogGUIVerticalLayout();

            bool Filter(VesselType t) => VesselType.Lander == t || VesselType.Probe == t || VesselType.Rover == t ||
                                         VesselType.Ship == t || VesselType.Station == t;

            foreach (var vessel in FlightGlobals.Vessels.Where(v =>
                v.id != FlightGlobals.ActiveVessel.id && Filter(v.vesselType)))
            {
                bool IsSelected() => FlightGlobals.ActiveVessel.targetObject != null;
                guiBase
                    .WithHorizontal()
                    .WithLabel(vessel.DiscoveryInfo.displayName.Value)
                    .WithFlexible()
                    .WithButton<ITargetable>("Set Target", target
                        => FlightGlobals.fetch.SetVesselTarget(target, true), vessel, () => !IsSelected())
                    .WithButton<ITargetable>("Clear Target", target
                        => FlightGlobals.fetch.SetVesselTarget(target, true), null, IsSelected);
            }

            var optionDialog = new MultiOptionDialog("Target Selector", string.Empty, "Select target", HighLogic.UISkin,
                guiBase);
            _dialog = PopupDialog.SpawnPopupDialog(Vector2.one * .5f, Vector2.one * .5f, optionDialog, false,
                UISkinManager.GetSkin("MainMenuSkin"), false);
        }

        private void OnDestroy()
        {
            ApplicationLauncher.Instance.RemoveModApplication(_button);
            _button = null;
            if (_dialog == null) return;
            _dialog.Dismiss();
            _dialog = null;
        }
    }
}