using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class BaseUI : MonoBehaviour
{
    // 미리 딕셔너리에 저장 해둠
    private Dictionary<string, GameObject> gameObjectDic;
    private Dictionary<(string, System.Type), Component> componentDic;

    // 빠른 시간에 게임오브젝트만 바인딩
    protected void Bind()
    {
        //비활성화 되어있는 게임오브젝트까지 모두 다 찾아줌
        Transform[] transforms = GetComponentsInChildren<Transform>(true);
        gameObjectDic = new Dictionary<string, GameObject>(transforms.Length << 2);
        componentDic = new Dictionary<(string, System.Type), Component>();
        foreach (Transform child in transforms)
        {
            // 이름이 같은 오브젝트가 있을 수 있기에 tryadd로 넣어줌
            bool suceess = gameObjectDic.TryAdd(child.gameObject.name, child.gameObject);
            if (suceess == false)
            {
                Debug.Log($"이미 {child.gameObject.name} 게임오브젝트가 있어 추가되지 않음");
            }
        }
    }

    // 비교적 오랜 시간에 게임오브젝트와 모든 컴포넌트 바인딩
    protected void BindAll()
    {
        //비활성화 되어있는 게임오브젝트까지 모두 다 찾아줌
        Transform[] transforms = GetComponentsInChildren<Transform>(true);
        gameObjectDic = new Dictionary<string, GameObject>(transforms.Length << 2);

        foreach (Transform child in transforms)
        {
            gameObjectDic.TryAdd(child.gameObject.name, child.gameObject);
        }

        Component[] components = GetComponentsInChildren<Component>(true);
        componentDic = new Dictionary<(string, System.Type), Component>(components.Length << 4);
        foreach (Component child in components)
        {
            componentDic.TryAdd((child.gameObject.name, components.GetType()), child);
        }
    }



    //이름이 name인 UI 게임 오브젝트 가져오기
    //GetUI("Key 01") : Key 01 이름의 게임오브젝트 가져오기
    public GameObject GetUI(in string name)
    {
        gameObjectDic.TryGetValue(name, out GameObject gameObject);
        return gameObject;
    }


    // 이름이 name인 UI에서 컴포넌트 T가져오기
    // GetUI<Image>("Key") : Key 이름의 게임오브젝트에서 Image 컴포넌트 가져오기
    public T GetUI<T>(in string name) where T : Component
    {
        //  예시) Button 게임오브젝트 안에 Image 컴포넌트의 키: Button_Image
        //  예시) Chest 게임오브젝트 안에 Transform 컴포넌트의 키: Chest_Transform
        (string, System.Type) key = (name, typeof(T));

        componentDic.TryGetValue(key, out Component component);
        if (component != null)
            return component as T;

        gameObjectDic.TryGetValue(name, out GameObject gameObject);
        if (gameObject == null)
            return null;

        component = gameObject.GetComponent<T>();
        if (component == null)
            return null;

        componentDic.TryAdd(key, component);
        return component as T;
    }
}

