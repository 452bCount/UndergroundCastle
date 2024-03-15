using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class CharacterBaseSet : MonoBehaviour
{
    [BoxGroup("Box1", ShowLabel = false)]
    [BoxGroup("Box1/Box2", ShowLabel = false)]
    [TitleGroup("Box1/Box2/Character Setup")]
    [OnValueChanged("GetData")]
    [OnValueChanged("UpdateModel")]
    [OnValueChanged("LoadModel")]
    [SerializeField] 
    public CharacterBase 
        _BaseSet,
        _CostumeSet;

    SetBodyParts
        _BaseBodyPartsSet,
        _CostumeBodyPartsSet;

    private void GetData() {
        if (_BaseSet != null) { _BaseBodyPartsSet = _BaseSet._characterSuit; }
        if (_BaseSet == null) { _BaseBodyPartsSet = null; }

        if (_CostumeSet != null) { _CostumeBodyPartsSet = _CostumeSet._characterSuit; }
        if (_CostumeSet == null) { _CostumeBodyPartsSet = null; }
    }

    private void UpdateModel() { 
    if (_BaseBodyPartsSet != null || _CostumeBodyPartsSet != null) { this._PreviewModel = this.gameObject; } 
    if (_BaseBodyPartsSet == null && _CostumeBodyPartsSet == null) { this._PreviewModel = null; } }

    [FoldoutGroup("Box1/Body Point", expanded: false)]
    [SerializeField] 
    Transform 
    _FacePoint,
    _HeadPoint,
    _NeckPoint,
    _BodyPoint,
    _TailPoint,
    _LeftShoulderPoint,
    _RightShoulderPoint,
    _LeftHandPoint,
    _RightHandPoint,
    _LeftHipPoint,
    _RightHipPoint,
    _LeftKneePoint,
    _RightKneePoint,
    _LeftFootPoint,
    _RightFootPoint;

    //[FoldoutGroup("Box1/Instantiate Check", expanded: false)]
    //[SerializeField]
    //[TitleGroup("Box1/Instantiate Check/Base Parts")]
    //[ReadOnly]
    private Transform 
    _BaseFace,
    _BaseHead,
    _BaseNeck,
    _BaseBody,
    _BaseTail,
    _BaseLeftShoulder,
    _BaseRightShoulder,
    _BaseLeftHand,
    _BaseRightHand,
    _BaseLeftHip,
    _BaseRightHip,
    _BaseLeftKnee,
    _BaseRightKnee,
    _BaseLeftFoot,
    _BaseRightFoot;

    //[FoldoutGroup("Box1/Instantiate Check")]
    //[SerializeField]
    //[TitleGroup("Box1/Instantiate Check/Costume Parts")]
    //[ReadOnly]
    private Transform
    _CostumeFace,
    _CostumeHead,
    _CostumeNeck,
    _CostumeBody,
    _CostumeTail,
    _CostumeLeftShoulder,
    _CostumeRightShoulder,
    _CostumeLeftHand,
    _CostumeRightHand,
    _CostumeLeftHip,
    _CostumeRightHip,
    _CostumeLeftKnee,
    _CostumeRightKnee,
    _CostumeLeftFoot,
    _CostumeRightFoot;


    [ReadOnly, InlineEditor(InlineEditorModes.LargePreview)]
    [BoxGroup("Box1")]
    [HideLabel]
    public GameObject _PreviewModel;

    void Start()
    {
        if (_BaseBodyPartsSet != null) { LoadModel(); }
    }

    void LoadModel()
    {
        if (Application.isEditor)
        {
            if (_BaseFace != null) DestroyImmediate(_BaseFace.gameObject);
            if (_BaseHead != null) DestroyImmediate(_BaseHead.gameObject);
            if (_BaseNeck != null) DestroyImmediate(_BaseNeck.gameObject);
            if (_BaseBody != null) DestroyImmediate(_BaseBody.gameObject);
            if (_BaseTail != null) DestroyImmediate(_BaseTail.gameObject);
            if (_BaseLeftShoulder != null) DestroyImmediate(_BaseLeftShoulder.gameObject);
            if (_BaseRightShoulder != null) DestroyImmediate(_BaseRightShoulder.gameObject);
            if (_BaseLeftHand != null) DestroyImmediate(_BaseLeftHand.gameObject);
            if (_BaseRightHand != null) DestroyImmediate(_BaseRightHand.gameObject);
            if (_BaseLeftHip != null) DestroyImmediate(_BaseLeftHip.gameObject);
            if (_BaseRightHip != null) DestroyImmediate(_BaseRightHip.gameObject);
            if (_BaseLeftKnee != null) DestroyImmediate(_BaseLeftKnee.gameObject);
            if (_BaseRightKnee != null) DestroyImmediate(_BaseRightKnee.gameObject);
            if (_BaseLeftFoot != null) DestroyImmediate(_BaseLeftFoot.gameObject);
            if (_BaseRightFoot != null) DestroyImmediate(_BaseRightFoot.gameObject);

            if (_CostumeFace != null) DestroyImmediate(_CostumeFace.gameObject);
            if (_CostumeHead != null) DestroyImmediate(_CostumeHead.gameObject);
            if (_CostumeNeck != null) DestroyImmediate(_CostumeNeck.gameObject);
            if (_CostumeBody != null) DestroyImmediate(_CostumeBody.gameObject);
            if (_CostumeTail != null) DestroyImmediate(_CostumeTail.gameObject);
            if (_CostumeLeftShoulder != null) DestroyImmediate(_CostumeLeftShoulder.gameObject);
            if (_CostumeRightShoulder != null) DestroyImmediate(_CostumeRightShoulder.gameObject);
            if (_CostumeLeftHand != null) DestroyImmediate(_CostumeLeftHand.gameObject);
            if (_CostumeRightHand != null) DestroyImmediate(_CostumeRightHand.gameObject);
            if (_CostumeLeftHip != null) DestroyImmediate(_CostumeLeftHip.gameObject);
            if (_CostumeRightHip != null) DestroyImmediate(_CostumeRightHip.gameObject);
            if (_CostumeLeftKnee != null) DestroyImmediate(_CostumeLeftKnee.gameObject);
            if (_CostumeRightKnee != null) DestroyImmediate(_CostumeRightKnee.gameObject);
            if (_CostumeLeftFoot != null) DestroyImmediate(_CostumeLeftFoot.gameObject);
            if (_CostumeRightFoot != null) DestroyImmediate(_CostumeRightFoot.gameObject);
        }
        else
        {
            if (_BaseFace != null) Destroy(_BaseFace.gameObject);
            if (_BaseHead != null) Destroy(_BaseHead.gameObject);
            if (_BaseNeck != null) Destroy(_BaseNeck.gameObject);
            if (_BaseBody != null) Destroy(_BaseBody.gameObject);
            if (_BaseTail != null) Destroy(_BaseTail.gameObject);
            if (_BaseLeftShoulder != null) Destroy(_BaseLeftShoulder.gameObject);
            if (_BaseRightShoulder != null) Destroy(_BaseRightShoulder.gameObject);
            if (_BaseLeftHand != null) Destroy(_BaseLeftHand.gameObject);
            if (_BaseRightHand != null) Destroy(_BaseRightHand.gameObject);
            if (_BaseLeftHip != null) Destroy(_BaseLeftHip.gameObject);
            if (_BaseRightHip != null) Destroy(_BaseRightHip.gameObject);
            if (_BaseLeftKnee != null) Destroy(_BaseLeftKnee.gameObject);
            if (_BaseRightKnee != null) Destroy(_BaseRightKnee.gameObject);
            if (_BaseLeftFoot != null) Destroy(_BaseLeftFoot.gameObject);
            if (_BaseRightFoot != null) Destroy(_BaseRightFoot.gameObject);

            if (_CostumeFace != null) Destroy(_CostumeFace.gameObject);
            if (_CostumeHead != null) Destroy(_CostumeHead.gameObject);
            if (_CostumeNeck != null) Destroy(_CostumeNeck.gameObject);
            if (_CostumeBody != null) Destroy(_CostumeBody.gameObject);
            if (_CostumeTail != null) Destroy(_CostumeTail.gameObject);
            if (_CostumeLeftShoulder != null) Destroy(_CostumeLeftShoulder.gameObject);
            if (_CostumeRightShoulder != null) Destroy(_CostumeRightShoulder.gameObject);
            if (_CostumeLeftHand != null) Destroy(_CostumeLeftHand.gameObject);
            if (_CostumeRightHand != null) Destroy(_CostumeRightHand.gameObject);
            if (_CostumeLeftHip != null) Destroy(_CostumeLeftHip.gameObject);
            if (_CostumeRightHip != null) Destroy(_CostumeRightHip.gameObject);
            if (_CostumeLeftKnee != null) Destroy(_CostumeLeftKnee.gameObject);
            if (_CostumeRightKnee != null) Destroy(_CostumeRightKnee.gameObject);
            if (_CostumeLeftFoot != null) Destroy(_CostumeLeftFoot.gameObject);
            if (_CostumeRightFoot != null) Destroy(_CostumeRightFoot.gameObject);
        }

        if (_BaseBodyPartsSet != null)
        {
            if (_FacePoint != null && _BaseBodyPartsSet.Face != null) { SetPreview(ref _BaseFace, _BaseBodyPartsSet.Face, _FacePoint); }
            if (_HeadPoint != null && _BaseBodyPartsSet.Head != null) { SetPreview(ref _BaseHead, _BaseBodyPartsSet.Head, _HeadPoint); }
            if (_NeckPoint != null && _BaseBodyPartsSet.Neck != null) { SetPreview(ref _BaseNeck, _BaseBodyPartsSet.Neck, _NeckPoint); }
            if (_BodyPoint != null && _BaseBodyPartsSet.Body != null) { SetPreview(ref _BaseBody, _BaseBodyPartsSet.Body, _BodyPoint); }
            if (_TailPoint != null && _BaseBodyPartsSet.Tail != null) { SetPreview(ref _BaseTail, _BaseBodyPartsSet.Tail, _TailPoint); }
            if (_LeftShoulderPoint != null && _BaseBodyPartsSet.LeftShoulder != null) { SetPreview(ref _BaseLeftShoulder, _BaseBodyPartsSet.LeftShoulder, _LeftShoulderPoint); }
            if (_RightShoulderPoint != null && _BaseBodyPartsSet.RightShoulder != null) { SetPreview(ref _BaseRightShoulder, _BaseBodyPartsSet.RightShoulder, _RightShoulderPoint); }
            if (_LeftHandPoint != null && _BaseBodyPartsSet.LeftHand != null) { SetPreview(ref _BaseLeftHand, _BaseBodyPartsSet.LeftHand, _LeftHandPoint); }
            if (_RightHandPoint != null && _BaseBodyPartsSet.RightHand != null) { SetPreview(ref _BaseRightHand, _BaseBodyPartsSet.RightHand, _RightHandPoint); }
            if (_LeftHipPoint != null && _BaseBodyPartsSet.LeftHip != null) { SetPreview(ref _BaseLeftHip, _BaseBodyPartsSet.LeftHip, _LeftHipPoint); }
            if (_RightHipPoint != null && _BaseBodyPartsSet.RightHip != null) { SetPreview(ref _BaseRightHip, _BaseBodyPartsSet.RightHip, _RightHipPoint); }
            if (_LeftKneePoint != null && _BaseBodyPartsSet.LeftKnee != null) { SetPreview(ref _BaseLeftKnee, _BaseBodyPartsSet.LeftKnee, _LeftKneePoint); }
            if (_RightKneePoint != null && _BaseBodyPartsSet.RightKnee != null) { SetPreview(ref _BaseRightKnee, _BaseBodyPartsSet.RightKnee, _RightKneePoint); }
            if (_LeftFootPoint != null && _BaseBodyPartsSet._LeftFoot != null) { SetPreview(ref _BaseLeftFoot, _BaseBodyPartsSet.LeftFoot, _LeftFootPoint); }
            if (_RightFootPoint != null && _BaseBodyPartsSet._RightFoot != null) { SetPreview(ref _BaseRightFoot, _BaseBodyPartsSet.RightFoot, _RightFootPoint); }
        }

        if (_CostumeBodyPartsSet != null)
        {
            if (_FacePoint != null && _CostumeBodyPartsSet.Face != null) { SetPreview(ref _CostumeFace, _CostumeBodyPartsSet.Face, _FacePoint); }
            if (_HeadPoint != null && _CostumeBodyPartsSet.Head != null) { SetPreview(ref _CostumeHead, _CostumeBodyPartsSet.Head, _HeadPoint); }
            if (_NeckPoint != null && _CostumeBodyPartsSet.Neck != null) { SetPreview(ref _CostumeNeck, _CostumeBodyPartsSet.Neck, _NeckPoint); }
            if (_BodyPoint != null && _CostumeBodyPartsSet.Body != null) { SetPreview(ref _CostumeBody, _CostumeBodyPartsSet.Body, _BodyPoint); }
            if (_TailPoint != null && _CostumeBodyPartsSet.Tail != null) { SetPreview(ref _CostumeTail, _CostumeBodyPartsSet.Tail, _TailPoint); }
            if (_LeftShoulderPoint != null && _CostumeBodyPartsSet.LeftShoulder != null) { SetPreview(ref _CostumeLeftShoulder, _CostumeBodyPartsSet.LeftShoulder, _LeftShoulderPoint); }
            if (_RightShoulderPoint != null && _CostumeBodyPartsSet.RightShoulder != null) { SetPreview(ref _CostumeRightShoulder, _CostumeBodyPartsSet.RightShoulder, _RightShoulderPoint); }
            if (_LeftHandPoint != null && _CostumeBodyPartsSet.LeftHand != null) { SetPreview(ref _CostumeLeftHand, _CostumeBodyPartsSet.LeftHand, _LeftHandPoint); }
            if (_RightHandPoint != null && _CostumeBodyPartsSet.RightHand != null) { SetPreview(ref _CostumeRightHand, _CostumeBodyPartsSet.RightHand, _RightHandPoint); }
            if (_LeftHipPoint != null && _CostumeBodyPartsSet.LeftHip != null) { SetPreview(ref _CostumeLeftHip, _CostumeBodyPartsSet.LeftHip, _LeftHipPoint); }
            if (_RightHipPoint != null && _CostumeBodyPartsSet.RightHip != null) { SetPreview(ref _CostumeRightHip, _CostumeBodyPartsSet.RightHip, _RightHipPoint); }
            if (_LeftKneePoint != null && _CostumeBodyPartsSet.LeftKnee != null) { SetPreview(ref _CostumeLeftKnee, _CostumeBodyPartsSet.LeftKnee, _LeftKneePoint); }
            if (_RightKneePoint != null && _CostumeBodyPartsSet.RightKnee != null) { SetPreview(ref _CostumeRightKnee, _CostumeBodyPartsSet.RightKnee, _RightKneePoint); }
            if (_LeftFootPoint != null && _CostumeBodyPartsSet._LeftFoot != null) { SetPreview(ref _CostumeLeftFoot, _CostumeBodyPartsSet.LeftFoot, _LeftFootPoint); }
            if (_RightFootPoint != null && _CostumeBodyPartsSet._RightFoot != null) { SetPreview(ref _CostumeRightFoot, _CostumeBodyPartsSet.RightFoot, _RightFootPoint); }
        }
    }

    void SetPreview(ref Transform instancePreview, GameObject characterBasePart, Transform transformPoint)
    {
        instancePreview = Instantiate(characterBasePart).transform;
        instancePreview.transform.SetParent(transformPoint);
        instancePreview.transform.localPosition = Vector3.zero;
        instancePreview.transform.localRotation = Quaternion.Euler(Vector3.zero);
    }
}
