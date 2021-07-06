using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Uis : MonoBehaviour
{
    [SerializeField] List<string> list;

    private void OnEnable()
    {
        var doc = GetComponent<UIDocument>();
        var root = doc.rootVisualElement;
        var listView = root.Q<ListView>();
        var addButton = root.Q<Button>();

        listView.itemsSource = list;
        listView.makeItem = () =>
        {
            return new TextField { label = "field" };
        };
        listView.bindItem = (ve, idx) =>
        {
            var field = (ve as TextField);
            field.value = list[idx];
            field.RegisterValueChangedCallback(evt => list[idx] = evt.newValue);
            field.isDelayed = true;
        };
        listView.reorderable = true;

        listView.RegisterCallback<KeyDownEvent>(evt =>
        {
            if (evt.keyCode == KeyCode.Delete)
            {
                listView.selectedIndices.OrderByDescending(idx => idx).ToList().ForEach(idx => list.RemoveAt(idx));
                listView.Refresh();
                listView.SetSelection(-1);
            }
        });
        addButton.clicked += () =>
        {
            list.Add("");
            listView.Refresh();
        };
    }
}
