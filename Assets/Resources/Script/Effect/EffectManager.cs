using System.Collections;
using System.Collections.Generic;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using UnityEngine.UI;

public class EffectManager : MonoBehaviour {

    GameObject prefab_Effect = null;
    readonly float mPixelPerUnit = 100; //エフェクトの倍率変換

	// Use this for initialization

    //オブジェクトプールとして実装する
    public void MakePrefab(string prefabName)
    {
        //プレハブ指定
        if (prefab_Effect == null)
        {
            GameObject parentObject = GameObject.Find("Panel_Effect");

            prefab_Effect
                = Instantiate((GameObject)Resources.Load(prefabName));
            //Load("FT_Infinity_lite/_Prefabs/Charge/Charge_Fire"));

            prefab_Effect.transform.SetParent(parentObject.transform,false);

            prefab_Effect.GetComponent<ParticleSystem>()
                .GetComponent<Renderer>().sortingLayerName = "Effect";

        }
    }

    //エフェクトの位置設定
    public void SetPosition(Vector3 pos)
    {
        if (prefab_Effect != null)
        {
            pos.x = (pos.x - (float)Screen.width/2)/ mPixelPerUnit;
            pos.y = (pos.y - (float)Screen.height/2)/mPixelPerUnit;

            prefab_Effect.transform.position = pos;
        }
    }
}

namespace Samples
{
    /// <summary>
    /// エフェクトを生成する奴
    /// </summary>
    public class EffectManager : MonoBehaviour
    {
        [SerializeField]
        private Button _button;

        [SerializeField]
        private ExplosionEffect _effectPrefab;

        [SerializeField]
        private Transform _hierarchyTransform; //追加

        private EffectPool _effectPool; //追加

        void Start()
        {
            //オブジェクトプールを生成
            _effectPool = new EffectPool(_hierarchyTransform, _effectPrefab);

            //破棄されたときにPoolを解放する
            this.OnDestroyAsObservable().Subscribe(_ => _effectPool.Dispose());

            //ボタンが押されたらエフェクト生成
            _button.OnClickAsObservable()
                .Subscribe(_ =>
                {
                    //ランダムな場所
                    var position = new Vector3(Random.Range(-5f, 5f), 0, Random.Range(-5f, 5f));

                    //poolから1つ取得
                    var effect = _effectPool.Rent();

                    //エフェクトを再生し、再生終了したらpoolに返却する
                    effect.PlayEffect(position)
                        .Subscribe(__ =>
                        {
                            _effectPool.Return(effect);
                        });
                });
        }
    }
}