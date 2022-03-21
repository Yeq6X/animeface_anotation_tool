using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using System.Text;
using System.Linq;

public class controller : MonoBehaviour
{
  private GameObject _sphere;
  private SpriteRenderer _sr;
  private Sprite[] _images;
  private int i;
  private int max_num;
  private StreamWriter _sw;

  private float[][] data;

  private Vector3 initialPosition;
  private Vector3 initialAngles;
  private Vector3 initialScale;

  void Start()
  {
    _sr = GameObject.Find("Sprite").GetComponent<SpriteRenderer>();
    _images = Resources.LoadAll<Sprite>("Images/trainB");
    _sphere = GameObject.Find("Sphere");
    initialPosition = _sphere.transform.position;
    initialAngles = _sphere.transform.eulerAngles;
    initialScale = _sphere.transform.localScale;

    // データの最大数
    max_num = 3400;
    data = new float[max_num][];
    for (int j = 0; j < max_num; j++) {
      data[j] = new float[9];
    }

    // 現在のデータ数
    if (File.Exists(Application.dataPath + "/Resources/SaveData.csv")) {
      string[] lines = File.ReadAllLines(Application.dataPath + "/Resources/SaveData.csv");
      i = lines.Length;
      for (int j = 0; j < i; j++) {
        data[j] = lines[j].Split(',').Select(float.Parse).ToArray();
      }
    } else {
      i = 0;
    }

    // スプライト読み込み
    _sr.sprite = _images[i];
    Debug.Log(i);
  }

  void Update()
  {
    // 画像送り
    if (Input.GetKeyDown(KeyCode.Return)) {
      data[i][0] = i;
      data[i][1] = _sphere.transform.position.x;
      data[i][2] = _sphere.transform.position.y;
      data[i][3] = _sphere.transform.eulerAngles.x;
      data[i][4] = _sphere.transform.eulerAngles.y;
      data[i][5] = _sphere.transform.eulerAngles.z;
      data[i][6] = _sphere.transform.localScale.x;
      data[i][7] = _sphere.transform.localScale.y;
      data[i][8] = _sphere.transform.localScale.z;

      i = (i+1)%_images.Length;
      _sr.sprite = _images[i];
      Debug.Log(i);

      _sphere.transform.position = initialPosition;
      _sphere.transform.eulerAngles = initialAngles;
      _sphere.transform.localScale = initialScale;
    }

    // セーブ
    if (Input.GetKeyDown(KeyCode.Space)) {
      string[] dataStr = data.Select(i=>string.Join(",", i)).ToArray();
      dataStr = dataStr.Take(i).ToArray();
      File.WriteAllLines(Application.dataPath + "/Resources/SaveData.csv", dataStr);
    }

    // リセット
    if (Input.GetKeyDown(KeyCode.Alpha0)) {
      _sphere.transform.position = initialPosition;
      _sphere.transform.eulerAngles = initialAngles;
      _sphere.transform.localScale = initialScale;
    }
  }
}
