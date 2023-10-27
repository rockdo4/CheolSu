using System;
using System.Collections.Generic;
using UnityEngine;

public class GachaSystem<T>
{
	private System.Random random;
	private Dictionary<T, double> itemPercent;
	private Dictionary<T, double> itemPercentNormalize;

	private bool notSummary;
	private double sumOfPercent;

	public GachaSystem() 
	{ 
		random = new System.Random();
		itemPercent = new Dictionary<T, double>();
		itemPercentNormalize = new Dictionary<T, double>();
		notSummary = true;
		sumOfPercent = 0.0;
	}

	public void Add(T item, double percent)
	{
		if(itemPercent.ContainsKey(item))
		{
			throw new Exception($"[{item}] 아이템이 존재합니다.");
		}
		if(percent <= 0.0)
		{
			throw new Exception($"확률은 0보다 커야 합니다");
		}

		itemPercent.Add(item, percent);
		notSummary = true;
	}

	public void Remove(T item)
	{
		if (!itemPercent.ContainsKey(item))
		{
			throw new Exception($"[{item}] 아이템이 목록에 존재하지 않습니다.");
		}

		itemPercent.Remove(item);
		notSummary = true;
	}

	public T GetGacha()
	{
		CalculateSum();

		double randomValue = random.NextDouble(); //0.0 ~ 1.0
		randomValue *= sumOfPercent;

		if (randomValue < 0.0) randomValue = 0.0;
		if (randomValue > sumOfPercent) randomValue = sumOfPercent - 0.000001;

		double current = 0.0;

		foreach (var pair in itemPercent)
		{
			current += pair.Value;

			if (randomValue < current)
			{
				return pair.Key;
			}
		}

		throw new Exception($"Unreachable - [Random Value : {randomValue}, Current Value : {current}]");
	}

	private void NormalizedPercent()
	{
		itemPercentNormalize.Clear();
		foreach (var item in itemPercent)
		{
			itemPercentNormalize.Add(item.Key, item.Value / sumOfPercent); // 개별 가중치 / 전체 가중치 = 확률
		}	
	}

	private void CalculateSum()
	{
		if (!notSummary) return;
		notSummary = false;

		sumOfPercent = 0.0;
		foreach (var item in itemPercent)
		{
			sumOfPercent += item.Value;
		}
		Debug.Log(sumOfPercent);
		NormalizedPercent();
	}
}
