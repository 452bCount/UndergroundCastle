using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

[CreateAssetMenu(fileName = "Character", menuName = "Character/Create New Parts")]
public class CharacterBase : ScriptableObject
{
    [BoxGroup("Box1", ShowLabel = false)]
    [TitleGroup("Box1/CHARACTER BODY PARTS")]
    [SerializeField] 
    string _Name;

    [BoxGroup("Box1", ShowLabel = false)]
    [OnValueChanged("UpdateModel")]
    [SerializeField] 
    public SetBodyParts _characterSuit;

    [BoxGroup("Box1", ShowLabel = false)]
    [ReadOnly, InlineEditor(InlineEditorModes.LargePreview)]
    [SerializeField] 
    public GameObject _PreviewModel;
    
    private void UpdateModel()
    {
        if (_characterSuit != null) { this._PreviewModel = _characterSuit.gameObject; }
        if (_characterSuit == null) { this._PreviewModel = null; }
    }
}
