using System.Collections;
using System.Collections.Generic;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using UnityEngine.UI;

public class EffectManager : MonoBehaviour {

/*
    [SerializeField]
    private EffectObject _effectPrefab;

    [SerializeField]
    private Transform _hierarchyTransform;

    private EffectPool _effectPool;
*/
    private GameObject prefab_Effect;

    void Awake(){
        //親オブジェクトの指定
        GameObject parentObject
            = GameObject.Find("Panel_Effect");

        //プレハブ指定
        prefab_Effect = Instantiate(
            (GameObject)Resources.Load("Prefabs/SkillEffect"));
        prefab_Effect.transform.SetParent(parentObject.transform,false);


        /*
        //オブジェクトプールを生成
        _effectPool = new EffectPool(_hierarchyTransform, _effectPrefab);

        //破棄されたときにPoolを解放する
        this.OnDestroyAsObservable().Subscribe(_ => _effectPool.Dispose());
        */
    }

    //
    public void SetEffect(Vector3 position)
    {
        //スケールは1/100
        //オフセット　-200、-100

        Vector3 _position = new Vector3((position.x-640-200)/100,(position.y-480+100)/100);
        //位置設定
        prefab_Effect.transform.position = _position;
        //エフェクト再生
        prefab_Effect.transform.GetComponent<EffekseerEmitter>().Play();

        /*
        //poolから1つ取得
        var effect = _effectPool.Rent();

        //エフェクトを再生し、再生終了したらpoolに返却する
        effect.PlayEffect(position)
            .Subscribe(__ =>
            {
                _effectPool.Return(effect);
            });
        */
    }
}
