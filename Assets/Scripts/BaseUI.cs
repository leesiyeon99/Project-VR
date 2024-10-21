using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class BaseUI : MonoBehaviour
{
    // �̸� ��ųʸ��� ���� �ص�
    private Dictionary<string, GameObject> gameObjectDic;
    private Dictionary<(string, System.Type), Component> componentDic;

    // ���� �ð��� ���ӿ�����Ʈ�� ���ε�
    protected void Bind()
    {
        //��Ȱ��ȭ �Ǿ��ִ� ���ӿ�����Ʈ���� ��� �� ã����
        Transform[] transforms = GetComponentsInChildren<Transform>(true);
        gameObjectDic = new Dictionary<string, GameObject>(transforms.Length << 2);
        componentDic = new Dictionary<(string, System.Type), Component>();
        foreach (Transform child in transforms)
        {
            // �̸��� ���� ������Ʈ�� ���� �� �ֱ⿡ tryadd�� �־���
            bool suceess = gameObjectDic.TryAdd(child.gameObject.name, child.gameObject);
            if (suceess == false)
            {
                Debug.Log($"�̹� {child.gameObject.name} ���ӿ�����Ʈ�� �־� �߰����� ����");
            }
        }
    }

    // ���� ���� �ð��� ���ӿ�����Ʈ�� ��� ������Ʈ ���ε�
    protected void BindAll()
    {
        //��Ȱ��ȭ �Ǿ��ִ� ���ӿ�����Ʈ���� ��� �� ã����
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



    //�̸��� name�� UI ���� ������Ʈ ��������
    //GetUI("Key 01") : Key 01 �̸��� ���ӿ�����Ʈ ��������
    public GameObject GetUI(in string name)
    {
        gameObjectDic.TryGetValue(name, out GameObject gameObject);
        return gameObject;
    }


    // �̸��� name�� UI���� ������Ʈ T��������
    // GetUI<Image>("Key") : Key �̸��� ���ӿ�����Ʈ���� Image ������Ʈ ��������
    public T GetUI<T>(in string name) where T : Component
    {
        //  ����) Button ���ӿ�����Ʈ �ȿ� Image ������Ʈ�� Ű: Button_Image
        //  ����) Chest ���ӿ�����Ʈ �ȿ� Transform ������Ʈ�� Ű: Chest_Transform
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

