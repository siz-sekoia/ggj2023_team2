//--------------------------------------------------------------------------------------------------------------------------------
// Cartoon FX
// (c) 2012-2020 Jean Moreno
//--------------------------------------------------------------------------------------------------------------------------------

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public enum eEffec
{
	CFXR_Explosion_1,
	CFXR_Explosion_Smoke_2_Solo,
	CFXR3_Fire_Explosion_B,
	CFXR3_Hit_Fire_B,
	CFXR_Fire,
	CFXR_Fire_Breath,
	CFXR3_Hit_Ice_B,
	CFXR_Hit_A,
	CFXR_Hit_D_3D,
	CFXR3_Hit_Misc_A,
	CFXR3_Hit_Misc_F_Smoke,
	CFXR2_WW_Explosion,
	CFXR_Impact_Glowing_HDR,
	CFXR2_WW_Enemy_Explosion,
	CFXR4_Firework_HDR_Shoot_Single_,
	CFXR3_Hit_Light_B,
	CFXR3_LightGlow_A,
	CFXR2_Shiny_Item,
	CFXR3_Magic_Aura_A,
	CFXR4_Bouncing_Glows_Bubbl,
	CFXR4_Falling_Stars,
	CFXR_Magic_Poof,
	CFXR2_Broken_Heart,
	CFXR_Smoke_Source_3D,
	CFXR2_Poison_Cloud,
	CFXR3_Ambient_Glows,
	CFXR4_Rain_Falling,
	CFXR__BOING_,
	CFXR__BOOM_,
	CFXR2__CURSED_,
	CFXR4__POISONED_,
};

public class effect : MonoBehaviour
{
	public void NextEffect()
	{
		if (RandmFlag)
		{
			index = Random.Range(0, effectsList.Length - 1);
		}
		else
		{
			index++;
		}
		effec = (eEffec)index;
		WrapIndex();
		EffectEvent();
	}

	public void PreviousEffect()
	{
		index--;
		WrapIndex();
		EffectEvent();
	}
		


	[System.NonSerialized] public GameObject currentEffect;
	[System.NonSerialized] GameObject[] effectsList;
	int index = 0;

	public bool RandmFlag = false;

	public eEffec effec = eEffec.CFXR_Explosion_1;
	[Button("EffectEvent", "effect")]
	public int EffectButton;

	public float lifeTime = 10.0f;

	public float time = 10.0f;

	void Awake()
	{
		time = lifeTime;
			var list = new List<GameObject>();
		for (int i = 0; i < this.transform.childCount; i++)
		{
			var effect = this.transform.GetChild(i).gameObject;
			list.Add(effect);
			var cfxrEffect = effect.GetComponent<CartoonFX.CFXR_Effect>();
			if (cfxrEffect != null) cfxrEffect.clearBehavior = CartoonFX.CFXR_Effect.ClearBehavior.Disable;
		}
		effectsList = list.ToArray();
	}
	public void Update()
	{
		time -= Time.deltaTime;
		if (time < 0)
		{
			currentEffect.SetActive(false);
		}
	}

	public void EffectEvent()
	{
		time = lifeTime;

		if (RandmFlag)
		{
			var i = Random.Range(0, effectsList.Length - 1);
			effec = (eEffec)i;
		}
		index = (int)effec;

		if (currentEffect != null)
		{
			currentEffect.SetActive(false);
		}
		currentEffect = effectsList[index];
		currentEffect.SetActive(true);

	}

	void WrapIndex()
	{
		if (index < 0) index = effectsList.Length - 1;
		if (index >= effectsList.Length) index = 0;
	}
}