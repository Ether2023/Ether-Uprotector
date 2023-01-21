using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class GUIComponetResolver
{
    public HashSet<string> ReferencedGuiMethodHashSet = new HashSet<string>();
    public void ResolveGUIComponent(Component _Component)
    {
        if (_Component is UIBehaviour)
        {
            if (_Component is Button)
            {
                Button button = _Component as Button;
                this.ResolveGUIComponentIterateEvent(_Component, button.onClick);
            }
            else if (_Component is Dropdown)
            {
                Dropdown dropdown = _Component as Dropdown;
                this.ResolveGUIComponentIterateEvent(_Component, dropdown.onValueChanged);
            }
            else if (_Component is InputField)
            {
                InputField inputFiled = _Component as InputField;
                this.ResolveGUIComponentIterateEvent(_Component, inputFiled.onValueChanged);
                this.ResolveGUIComponentIterateEvent(_Component, inputFiled.onEndEdit);
            }
            else if (_Component is MaskableGraphic)
            {
                MaskableGraphic maskableGraphic = _Component as MaskableGraphic;
                this.ResolveGUIComponentIterateEvent(_Component, maskableGraphic.onCullStateChanged);
            }
            else if (_Component is Scrollbar)
            {
                Scrollbar scrollbar = _Component as Scrollbar;
                this.ResolveGUIComponentIterateEvent(_Component, scrollbar.onValueChanged);
            }
            else if (_Component is ScrollRect)
            {
                ScrollRect scrollRect = _Component as ScrollRect;
                this.ResolveGUIComponentIterateEvent(_Component, scrollRect.onValueChanged);
            }
            else if (_Component is Slider)
            {
                Slider slider = _Component as Slider;
                this.ResolveGUIComponentIterateEvent(_Component, slider.onValueChanged);
            }
            else if (_Component is Toggle)
            {
                Toggle toggle = _Component as Toggle;
                this.ResolveGUIComponentIterateEvent(_Component, toggle.onValueChanged);
            }
        }
        if (_Component is EventTrigger)
        {
                EventTrigger Trigger = _Component as EventTrigger;
            for (int i = 0; i < Trigger.triggers.Count; i++)
            {
                if (Trigger.triggers[i] != null)
                {
                    this.ResolveGUIComponentIterateEvent(_Component, Trigger.triggers[i].callback);
                }
            }
        }
        return;
    }
    private void ResolveGUIComponentIterateEvent(Component _Component, UnityEventBase EventBase)
    {
        if (EventBase == null)
        {
            return;
        }
        int EventCount = EventBase.GetPersistentEventCount();
        if (EventCount == 0)
        {
            return;
        }
        string[] EventMethodNameArray = new string[EventCount];
        for (int i = 0; i < EventCount; i++)
        {
                string MethodName = EventBase.GetPersistentMethodName(i);
                if (!this.ReferencedGuiMethodHashSet.Contains(MethodName))
                {
                    this.ReferencedGuiMethodHashSet.Add(MethodName);
                }
                EventMethodNameArray[i] = MethodName;
        }
    }
}
