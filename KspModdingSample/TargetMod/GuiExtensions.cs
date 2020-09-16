using System;
using UnityEngine;

namespace KspModdingSample
{
    public static class GuiExtensions
    {
        public static DialogGUIBase WithLabel(this DialogGUIBase gui, string text)
        {
            var label = new DialogGUILabel(text);
            gui.AddChild(label);
            return gui;
        }

        public static DialogGUIBase WithHorizontal(this DialogGUIBase gui, TextAnchor anchor = TextAnchor.MiddleLeft)
        {
            var layout = new DialogGUIHorizontalLayout(anchor);
            gui.AddChild(layout);
            return layout;
        }
        
        public static DialogGUIBase WithFlexible(this DialogGUIBase gui)
        {
            var layout = new DialogGUIFlexibleSpace();
            gui.AddChild(layout);
            return gui;
        }

        public static DialogGUIBase WithButton<T>(this DialogGUIBase gui, string text,
            Callback<T> callback = null,
            T parameter = null, Func<bool> condition = null, bool dismiss = false) where T : class
        {
            callback = callback ?? DefaultCallback;
            condition = condition ?? DefaultCondition;
            
            var button = new DialogGUIButton<T>(text, callback, parameter, condition, dismiss);
            gui.AddChild(button);
            return gui;
        }

        private static bool DefaultCondition() => true;

        private static void DefaultCallback<T>(T data) where T : class
        {
        }
    }
}