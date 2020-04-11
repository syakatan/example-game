using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockGenerator : MonoBehaviour
{

	public GameObject rockPrefab;

	void Start()
	{
		InvokeRepeating("GenRock", 1, 1);
		//InvokeRepeating関数は第一引数の関数を第二引数の秒数ごとに実行してくれる関数。
	}

	void GenRock()
	{
		Instantiate(rockPrefab, new Vector3(-2.5f + 5 * Random.value, 6, 0), Quaternion.identity);
	}
}
